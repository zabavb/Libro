using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Library.Common;
using BookAPI.Data.CachHelper;

namespace BookAPI.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AuthorRepository> _logger;
        private readonly string _cacheKeyPrefix = "Author_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);

        public AuthorRepository(
            BookDbContext context, 
            ICacheService cacheService, ILogger<AuthorRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task CreateAsync(Author entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            _context.Authors.Add(entity);
            await _context.SaveChangesAsync();

            string cacheKey = $"{_cacheKeyPrefix}{entity.Id}";
            await _cacheService.SetAsync(cacheKey, entity, _cacheExpiration);

            string allAuthorsCacheKey = $"{_cacheKeyPrefix}All";
            var cachedAuthors = await _cacheService.GetAsync<List<Author>>(allAuthorsCacheKey) ?? new List<Author>();

            cachedAuthors.Add(entity);
            await _cacheService.SetAsync(allAuthorsCacheKey, cachedAuthors, _cacheExpiration);

            _logger.LogInformation("New Author added to DB and cached.");
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException("Author not found");
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}{id}");

            string allAuthorsCacheKey = $"{_cacheKeyPrefix}All";
            var cachedAuthors = await _cacheService.GetAsync<List<Author>>(allAuthorsCacheKey);

            if (cachedAuthors != null)
            {
                await _cacheService.UpdateListAsync(allAuthorsCacheKey, default(Author), id, _cacheExpiration);
            }
        }

        public async Task<PaginatedResult<Author>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, AuthorFilter? filter, AuthorSort? sort)
        {
            List<Author> authors;
            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedAuthors = await _cacheService.GetAsync<List<Author>>(cacheKey);

            if (cachedAuthors != null && cachedAuthors.Count > 0)
            {
                authors = cachedAuthors;
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                authors = await _context.Authors.ToListAsync();
                _logger.LogInformation("Fetched from DB.");

                await _cacheService.SetAsync(cacheKey, authors, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            IQueryable<Author> authorQuery = authors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                authorQuery = authorQuery.SearchBy(searchTerm, p => p.Name);

            if (filter != null)
                authorQuery = filter.Apply(authorQuery);

            if (sort != null)
                authorQuery = sort.Apply(authorQuery);

            var totalAuthors = authorQuery.Count();
            var paginatedAuthors = authorQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedResult<Author>
            {
                Items = paginatedAuthors,
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
            var cachedAuthor = await _cacheService.GetAsync<Author>(cacheKey);

            if (cachedAuthor != null)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return cachedAuthor;
            }

            _logger.LogInformation("Fetched from DB.");
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (author != null)
            {
                await _cacheService.SetAsync(cacheKey, author, _cacheExpiration);
            }

            return author;
        }

        public async Task UpdateAsync(Author entity)
        {
            var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == entity.Id)
                                 ?? throw new KeyNotFoundException("Author not found");

            _context.Entry(existingAuthor).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            await _cacheService.SetAsync($"{_cacheKeyPrefix}{entity.Id}", entity, _cacheExpiration);

            string allAuthorsCacheKey = $"{_cacheKeyPrefix}All";
            var cachedAuthors = await _cacheService.GetAsync<List<Author>>(allAuthorsCacheKey);

            if (cachedAuthors != null)
            {
                await _cacheService.UpdateListAsync(allAuthorsCacheKey, entity, null, _cacheExpiration);
            }

            _logger.LogInformation("Author updated in DB and cached.");
        }
    }

}
