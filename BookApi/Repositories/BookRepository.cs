﻿using System.Linq.Expressions;
using System.Text;
using BookAPI.Data;
using BookAPI.Data.CachHelper;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Library.Common;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookOrderDetails = Library.DTOs.Order.BookOrderDetails;
using Amazon.Runtime.Internal;
using AutoMapper;
using Library.DTOs.Book;
using Book = BookAPI.Models.Book;
using Feedback = BookAPI.Models.Feedback;
using SubCategory = BookAPI.Models.SubCategory;
using Microsoft.IdentityModel.Tokens;

namespace BookAPI.Repositories
{
    public class BookRepository(
        BookDbContext context,
        ICacheService cacheService, ISubCategoryRepository subCategoryRepository, ILogger<BookRepository> logger, IMapper mapper) : IBookRepository
    {
        private readonly BookDbContext _context = context;
        private readonly ISubCategoryRepository _subCategoryRepository = subCategoryRepository;
        private readonly ILogger<IBookRepository> _logger = logger;
        private readonly string _cacheKeyPrefix = "Book_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);
        private readonly ICacheService _cacheService = cacheService;
        private readonly IMapper _mapper = mapper;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public async Task<PaginatedResult<BookCard>> GetAllAsync(
             int pageNumber,
             int pageSize,
             string? searchTerm,
             BookFilter? filter,
             BookSort? sort)
        {
            var filterJson = filter != null ? JsonSerializer.Serialize(filter, _jsonOptions) : "";
            var sortJson = sort != null ? JsonSerializer.Serialize(sort, _jsonOptions) : "";
            string cacheKey = $"{_cacheKeyPrefix}Page_{pageNumber}_Size_{pageSize}_Search_{searchTerm}_Filter_{filterJson}_Sort_{sortJson}";

            var cachedResult = await _cacheService.GetAsync<PaginatedResult<BookCard>>(cacheKey, _jsonOptions);
            if (cachedResult != null)
            {
                _logger.LogInformation("GetAllAsync: Fetched paginated result from CACHE.");
                return cachedResult;
            }

            IQueryable<Book> query = _context.Books
                .AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Feedbacks)
                .Include(b => b.Author)
                .Include(b => b.Subcategories);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.SearchBy(searchTerm, b => b.Title, b => b.Author.Name);
            }

            if (filter != null)
            {
                query = filter.Apply(query);
            }

            if (sort != null)
            {
                query = sort.Apply(query);
            }
            else
            {
                query = query.OrderBy(b => b.Title);
            }

            int totalCount = await query.CountAsync();

            List<Book> books = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            List<BookCard> items = new List<BookCard>();

            foreach (Book book in books)
            {

                int rating = 0;
                foreach (Feedback feedback in book.Feedbacks)
                {
                    rating += feedback.Rating;
                }

                items.Add(new BookCard()
                {
                    BookId = book.Id,
                    Title = book.Title,
                    AuthorId = book.AuthorId,
                    AuthorName = book.Author.Name,
                    ImageUrl = book.ImageUrl,
                    CategoryName = book.Category.Name,
                    IsAvailable = book.Quantity > 0,
                    Price = book.Price,
                    Rating = new BookFeedbacks()
                    {
                        AvgRating = rating / (book.Feedbacks.Count() > 0 ? book.Feedbacks.Count() : 1),
                        FeedbackAmount = book.Feedbacks.Count()
                    }
                });
            }

            var result = new PaginatedResult<BookCard>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            await _cacheService.SetAsync(cacheKey, result, _cacheExpiration, _jsonOptions);
            _logger.LogInformation("GetAllAsync: Fetched from DB and set paginated result to CACHE.");

            return result;
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedBook = await _cacheService.GetAsync<Book>(cacheKey, _jsonOptions);

            if (cachedBook != null)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return cachedBook;
            }

            _logger.LogInformation("Fetched from DB.");
            var book = await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Feedbacks)
                .Include(b => b.Subcategories)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _cacheService.SetAsync(cacheKey, book, _cacheExpiration, _jsonOptions);
            }

            return book;
        }

        public async Task<BookDetails?> GetDetailsAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedBook = await _cacheService.GetAsync<BookDetails>(cacheKey, _jsonOptions);

            if (cachedBook != null)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return cachedBook;
            }

            _logger.LogInformation("Fetched from DB.");
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Feedbacks)
                .Include(b => b.Subcategories)
                .FirstOrDefaultAsync(b => b.Id == id);

            int rating = 0;
            List<FeedbackDto> latestFeedbacks = new List<FeedbackDto>();
            if (book.Feedbacks != null)
            {
                foreach (Feedback feedback in book.Feedbacks)
                {
                    rating += feedback.Rating;
                    if (latestFeedbacks.Count < 2)
                    {
                        latestFeedbacks.Add(new FeedbackDto()
                        {
                            FeedbackId = feedback.Id,
                            UserId = feedback.UserId,
                            BookId = feedback.BookId,
                            Date = feedback.Date,
                            Comment = feedback.Comment,
                            IsPurchased = feedback.IsPurchased,
                            Rating = feedback.Rating
                        });
                    }
                }

            }

            List<string> tags = new List<string>();

            foreach (SubCategory subCategory in book.Subcategories)
            {
                tags.Add(subCategory.Name);
            }

            var bookDetails = new BookDetails()
            {
                BookId = book.Id,
                hasDigital = !string.IsNullOrEmpty(book.PdfFileUrl),
                Price = book.Price,
                Cover = book.Cover,
                Language = book.Language,
                ImageUrl = book.ImageUrl,
                Description = book.Description,
                Quantity = book.Quantity,
                Title = book.Title,
                Year = book.Year,

                PublisherName = book.Publisher.Name,

                Subcategories = tags,

                CategoryName = book.Category.Name,

                AuthorId = book.AuthorId,
                AuthorName = book.Author.Name,
                AuthorDescription = book.Author.Biography ?? "",
                AuthorImageUrl = book.Author.ImageUrl,
                /*LatestFeedback = latestFeedbacks,*/

                BookFeedbacks = new BookFeedbacks()
                {
                    AvgRating = rating / (book.Feedbacks.Count() > 0 ? book.Feedbacks.Count() : 1),
                    FeedbackAmount = book.Feedbacks.Count()
                }
            };

            if (bookDetails != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _cacheService.SetAsync(cacheKey, bookDetails, _cacheExpiration, _jsonOptions);
            }

            return bookDetails;
        }


        public async Task<ICollection<string>> GetAllForUserDetailsAsync(ICollection<Guid> ids)
        {
            if (ids.Count == 0)
                return new List<string>();

            string keySuffix = string.Join("_", ids.OrderBy(id => id));
            string cacheKey = $"{_cacheKeyPrefix}userDetails_{keySuffix}";

            var cachedTitles = await _cacheService.GetAsync<ICollection<string>>(cacheKey, _jsonOptions);
            if (cachedTitles != null)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return cachedTitles;
            }

            var titles = await _context.Books
                .AsNoTracking()
                .Where(book => ids.Contains(book.Id))
                .Select(book => book.Title)
                .ToListAsync();
            _logger.LogInformation("Fetched from DB.");

            if (titles.Count > 0)
            {
                await _cacheService.SetAsync(cacheKey, titles, _cacheExpiration, _jsonOptions);
                _logger.LogInformation("Set to CACHE.");
            }

            return titles;
        }

        public async Task CreateAsync(Book entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            if (entity.Subcategories != null)
            {
                var newSubcategories = await _context.Subcategories
                    .Where(sc => entity.Subcategories.Select(s => s.Id).Contains(sc.Id))
                    .ToListAsync();

                entity.Subcategories.Clear();
                foreach (var sub in newSubcategories)
                {
                    entity.Subcategories.Add(sub);
                }
            }
            _context.Books.Add(entity);
            await _context.SaveChangesAsync();

            string cacheKey = $"{_cacheKeyPrefix}{entity.Id}";
            await _cacheService.SetAsync(cacheKey, entity, _cacheExpiration, _jsonOptions);

            string allBooksCacheKey = $"{_cacheKeyPrefix}All";
            var cachedBooks = await _cacheService.GetAsync<List<Book>>(allBooksCacheKey, _jsonOptions) ??
                              new List<Book>();

            cachedBooks.Add(entity);
            await _cacheService.SetAsync(allBooksCacheKey, cachedBooks, _cacheExpiration, _jsonOptions);

            _logger.LogInformation("New Book added to DB and cached.");
        }


        public async Task UpdateAsync(Book entity)
        {
            var existingBook = await _context.Books
                .Include(b => b.Subcategories).FirstOrDefaultAsync(b => b.Id == entity.Id)
                               ?? throw new KeyNotFoundException("Book not found");

            if (entity.Subcategories != null)
            {
                var newSubcategories = await _context.Subcategories
                    .Where(sc => entity.Subcategories.Select(s => s.Id).Contains(sc.Id))
                    .ToListAsync();

                existingBook.Subcategories.Clear();
                foreach (var sub in newSubcategories)
                {
                    existingBook.Subcategories.Add(sub);
                }
            }
            _context.Entry(existingBook).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            await _cacheService.SetAsync($"{_cacheKeyPrefix}{entity.Id}", entity, _cacheExpiration, _jsonOptions);

            string allBooksCacheKey = $"{_cacheKeyPrefix}All";
            var cachedBooks = await _cacheService.GetAsync<List<Book>>(allBooksCacheKey, _jsonOptions);

            if (cachedBooks != null)
            {
                await _cacheService.UpdateListAsync(allBooksCacheKey, entity, null, _cacheExpiration);
            }

            _logger.LogInformation("Book updated in DB and cached.");
        }


        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id) ??
                       throw new KeyNotFoundException("Book not found");
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}{id}");

            string allBooksCacheKey = $"{_cacheKeyPrefix}All";
            var cachedBooks = await _cacheService.GetAsync<List<Book>>(allBooksCacheKey, _jsonOptions);

            if (cachedBooks != null)
            {
                await _cacheService.UpdateListAsync(allBooksCacheKey, default(Book), id, _cacheExpiration);
            }

            _logger.LogInformation("Book deleted from DB and cache.");
        }

        public async Task<BookOrderDetails> GetAllForOrderDetailsAsync(Guid bookId)
        {
            /*            string cacheKey = $"{_cacheKeyPrefix}{bookId}_details";
                        var cachedDetails = await _cacheService.GetAsync<BookOrderDetails>(cacheKey);
                        if (cachedDetails != null)
                        {
                            _logger.LogInformation("Fetched from CACHE.");
                            return cachedDetails;
                        }*/
            try
            {
                var book = await _context.Books
                        .AsNoTracking()
                        .Where(b => b.Id == bookId)
                        .Include(b => b.Author)
                        .Select(b => new BookOrderDetails()
                        {
                            BookId = bookId,
                            AuthorName = b.Author.Name,
                            //CoverType = b.Cover,
                            Price = b.Price,
                            Title = b.Title,
                            ImageUrl = b.ImageUrl
                        }).FirstOrDefaultAsync();
                _logger.LogInformation("Fetched from DB.");
                /*
                                if (book != null)
                                {
                                    await _cacheService.SetAsync(cacheKey, book, _cacheExpiration);
                                }*/

                return book;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        public async Task<ICollection<BookLibraryItem>> GetAllLibraryItems(ICollection<Guid> ids)
        {
            try
            {
                var books = await _context.Books
                    .AsNoTracking()
                    .Include(b => b.Author)
                    .Where(b => ids.Contains(b.Id) &&
                     (!string.IsNullOrEmpty(b.AudioFileUrl) || !string.IsNullOrEmpty(b.PdfFileUrl)))
                    .Select(b => new BookLibraryItem()
                    {
                        AuthorName = b.Author.Name,
                        ImageUrl = b.ImageUrl,
                        Title = b.Title,
                        AudioUrl = b.AudioFileUrl,
                        PdfFileUrl = b.PdfFileUrl,
                    })
                    .ToListAsync();

                return books;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

    }
}