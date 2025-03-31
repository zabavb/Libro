using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Library.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;
using IDatabase = StackExchange.Redis.IDatabase;

namespace BookAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<IBookRepository> _logger;
        private readonly string _cacheKeyPrefix = "Book_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);

        public BookRepository(BookDbContext context, IConnectionMultiplexer redis, ILogger<IBookRepository> logger)
        {
            _context = context;
            _redisDatabase = redis.GetDatabase();
            _logger = logger;
        }
        public BookRepository(BookDbContext context, IDatabase redisDatabase, ILogger<BookRepository> logger)
        {
            _context = context;
            _redisDatabase = redisDatabase;
            _logger = logger;
        }

        public async Task<PaginatedResult<Book>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            BookFilter? filter,
            BookSort? sort)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            string cacheKey = $"Books:Page:{pageNumber}:Size:{pageSize}:Search:{searchTerm}";
            var cachedBooks = await _redisDatabase.HashGetAllAsync(cacheKey);

            if (cachedBooks.Length > 0)
            {
                _logger.LogInformation("Fetched from CACHE.");
                var bookList = cachedBooks
                    .Select(entry => JsonSerializer.Deserialize<Book>(entry.Value!, options)!)
                    .ToList();
                ICollection<Book> bookCollection = bookList;

                return new PaginatedResult<Book>
                {
                    Items = bookCollection,
                    TotalCount = bookList.Count,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }

            var booksQuery = _context.Books
                .Include(x => x.Subcategories)
                .Include(x => x.Feedbacks)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                booksQuery = booksQuery.SearchBy(searchTerm, b => b.Title, b => b.Author.Name);

            if (filter != null)
                booksQuery = filter.Apply(booksQuery);

            if (sort != null)
                booksQuery = sort.Apply(booksQuery);

            var totalBooks = await booksQuery.CountAsync();
            var pagedBooks = await booksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation("Fetched from DB.");
            
            var hashEntries = pagedBooks.ToDictionary(
                book => book.Id.ToString(),
                book => JsonSerializer.Serialize(book,options)
            );

            await _redisDatabase.HashSetAsync(cacheKey, [.. hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value))]);
            await _redisDatabase.KeyExpireAsync(cacheKey, TimeSpan.FromMinutes(10));

            return new PaginatedResult<Book>
            {
                Items = pagedBooks,
                TotalCount = totalBooks,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
       
        
        //public async Task<PaginatedResult<Book>> GetAllAsync(
        //    int pageNumber,
        //    int pageSize,
        //    string searchTerm,
        //    BookFilter? filter,
        //    BookSort? sort)
        //{
        //    IQueryable<Book> books;
        //    string cacheKey = $"{_cacheKeyPrefix}All";
        //    var cachedBooks = await _redisDatabase.HashGetAllAsync(cacheKey);

        //    if (cachedBooks.Length > 0)
        //    {
        //        books = (IQueryable<Book>)cachedBooks.Select(entry => JsonSerializer.Deserialize<Book>(entry.Value!)!);
        //        _logger.LogInformation("Fetched from CACHE.");
        //    }
        //    else
        //    {
        //        books = (IQueryable<Book>)await _context.Books
        //            .Include(x => x.Subcategories)
        //            .Include(x => x.Feedbacks)
        //            .AsNoTracking()
        //            .ToListAsync();
        //        _logger.LogInformation("Fetched from DB.");

        //        var hashEntries = books.ToDictionary(
        //            book => book.Id.ToString(),
        //            book => JsonSerializer.Serialize(book)
        //        );

        //        await _redisDatabase.HashSetAsync(
        //            cacheKey,
        //            [.. hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value))]
        //        );
        //        await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
        //        _logger.LogInformation("Set to CACHE.");
        //    }

        //    if (books.Any() && !string.IsNullOrWhiteSpace(searchTerm))
        //        books = books.Search(searchTerm, b => b.Title, b => b.Author.Name);
        //    books = filter?.Apply(books.AsQueryable()) ?? books;
        //    books = sort?.Apply(books.AsQueryable()) ?? books;

        //    var totalBooks = books.Count();
        //    books = books.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        //    return new PaginatedResult<Book>
        //    {
        //        Items = books.ToList(),
        //        TotalCount = totalBooks,
        //        PageNumber = pageNumber,
        //        PageSize = pageSize
        //    };
        //}

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{_cacheKeyPrefix}{id}";
            string fieldKey = id.ToString();

            var cachedBook = await _redisDatabase.HashGetAsync(cacheKey, fieldKey);
            if (!cachedBook.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<Book>(cachedBook!);
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
                await _redisDatabase.HashSetAsync(cacheKey, fieldKey, JsonSerializer.Serialize(book));
                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
            }

            return book;
        }

        public async Task CreateAsync(Book entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Books.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book entity)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(a => a.Id == entity.Id)
                ?? throw new KeyNotFoundException("Book not found");

            _context.Entry(existingBook).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}