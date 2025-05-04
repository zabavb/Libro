using System.Linq.Expressions;
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
using IDatabase = StackExchange.Redis.IDatabase;

namespace BookAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;
        private readonly ILogger<IBookRepository> _logger;
        private readonly string _cacheKeyPrefix = "Book_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);
        private readonly ICacheService _cacheService;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Ігнорування null значень
        };

        public BookRepository(
            BookDbContext context,
            ICacheService cacheService, ILogger<BookRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<PaginatedResult<Book>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            BookFilter? filter,
            BookSort? sort)
        {
            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}All");
            List<Book> books;
            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedBooks = await _cacheService.GetAsync<List<Book>>(cacheKey, _jsonOptions);

            if (cachedBooks != null && cachedBooks.Count > 0)
            {
                books = cachedBooks;
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                books = await _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .Include(b => b.Feedbacks)
                    .Include(b => b.Subcategories)
                    .ToListAsync();
                _logger.LogInformation("Fetched from DB.");

                await _cacheService.SetAsync(cacheKey, books, _cacheExpiration, _jsonOptions);
                _logger.LogInformation("Set to CACHE.");
            }

            IQueryable<Book> bookQuery = books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                bookQuery = bookQuery.SearchBy(searchTerm, b => b.Title, b => b.Author.Name);

            if (filter != null)
                bookQuery = filter.Apply(bookQuery);

            if (sort != null)
                bookQuery = sort.Apply(bookQuery);

            var totalBooks = bookQuery.Count();
            var paginatedBooks = bookQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedResult<Book>
            {
                Items = paginatedBooks,
                TotalCount = totalBooks,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
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
            var a = _context.Books.ToList();
            var b = entity.Id;
            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == entity.Id)
                               ?? throw new KeyNotFoundException("Book not found");

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

        //condition - expression to filter books quantity
        // example of condition: b => b.Quantity > 0
        public async Task<List<Book>> GetBooksByConditionAsync(Expression<Func<Book, bool>> condition)
        {
            return await _context.Books
                                 .Where(condition)
                                 .ToListAsync();
        }

        public async Task<int> GetQuantityById(Guid id)
        {
            var book = await GetByIdAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }
            return book.Quantity;
        }

        public async Task AddQuantityById(Guid id, int quantity)
        {
            var book = await GetByIdAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }
            book.Quantity += quantity;
            if (book.Quantity < 0)
            {
                book.Quantity = 0;
            }
            await UpdateAsync(book);
            string cacheKey = $"{_cacheKeyPrefix}{id}";
            await _cacheService.SetAsync(cacheKey, book, _cacheExpiration, _jsonOptions);
            string allBooksCacheKey = $"{_cacheKeyPrefix}All";
            var cachedBooks = await _cacheService.GetAsync<List<Book>>(allBooksCacheKey, _jsonOptions);
            if (cachedBooks != null)
            {
                await _cacheService.UpdateListAsync(allBooksCacheKey, book, null, _cacheExpiration);
            }
        }
    }
}