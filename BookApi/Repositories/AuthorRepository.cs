using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Extensions;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Library.Common;

namespace BookAPI.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookDbContext _context;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<IAuthorRepository> _logger;
        private readonly string _cacheKeyPrefix = "Author_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);

        public AuthorRepository(BookDbContext context, IConnectionMultiplexer redis, ILogger<IAuthorRepository> logger)
        {
            _context = context;
            _redisDatabase = redis.GetDatabase();
            _logger = logger;
        }

        public async Task CreateAsync(Author entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            _context.Authors.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id)
                          ?? throw new KeyNotFoundException("Author not found");
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            await _redisDatabase.KeyDeleteAsync($"{_cacheKeyPrefix}{id}");
        }

        public async Task<PaginatedResult<Author>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, AuthorFilter? filter, AuthorSort? sort)
        {
            IQueryable<Author> authors;
            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedAuthors = await _redisDatabase.HashGetAllAsync(cacheKey);

            if (cachedAuthors.Length > 0)
            {
                authors = cachedAuthors.Select(entry => JsonSerializer.Deserialize<Author>(entry.Value!)!).AsQueryable();
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                authors = _context.Authors.AsQueryable();
                _logger.LogInformation("Fetched from DB.");

                var hashEntries = authors.ToDictionary(
                    author => author.Id.ToString(),
                    author => JsonSerializer.Serialize(author)
                );

                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    [.. hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value))]
                );
                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
                authors = authors.Search(searchTerm, p => p.Name);
            authors = filter?.Apply(authors) ?? authors;
            authors = sort?.Apply(authors) ?? authors;

            var totalAuthors = authors.Count();
            authors = authors.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PaginatedResult<Author>
            {
                Items = authors.ToList(),
                TotalCount = totalAuthors,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Author?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedAuthor = await _redisDatabase.StringGetAsync(cacheKey);

            if (!cachedAuthor.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<Author>(cachedAuthor!);
            }

            _logger.LogInformation("Fetched from DB.");
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (author != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(author), _cacheExpiration);
            }

            return author;
        }

        public async Task UpdateAsync(Author entity)
        {
            var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == entity.Id)
                                 ?? throw new KeyNotFoundException("Author not found");
            _context.Entry(existingAuthor).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            await _redisDatabase.StringSetAsync($"{_cacheKeyPrefix}{entity.Id}", JsonSerializer.Serialize(entity), _cacheExpiration);
        }
    }
}
