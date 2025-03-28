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

namespace BookAPI.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly BookDbContext _context;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<IFeedbackRepository> _logger;
        private readonly string _cacheKeyPrefix = "Feedback_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);

        public FeedbackRepository(BookDbContext context, IConnectionMultiplexer redis, ILogger<IFeedbackRepository> logger)
        {
            _context = context;
            _redisDatabase = redis.GetDatabase();
            _logger = logger;
        }

        public async Task CreateAsync(Feedback entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            _context.Feedbacks.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException("Feedback not found");
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            await _redisDatabase.KeyDeleteAsync($"{_cacheKeyPrefix}{id}");
        }

        public async Task<PaginatedResult<Feedback>> GetAllAsync(int pageNumber, int pageSize, FeedbackFilter? filter, FeedbackSort? sort)
        {
            IQueryable<Feedback> feedbacks;
            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedFeedbacks = await _redisDatabase.HashGetAllAsync(cacheKey);

            if (cachedFeedbacks.Length > 0)
            {
                feedbacks = cachedFeedbacks.Select(entry => JsonSerializer.Deserialize<Feedback>(entry.Value!)!).AsQueryable();
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                feedbacks = _context.Feedbacks.AsQueryable();
                _logger.LogInformation("Fetched from DB.");

                var hashEntries = feedbacks.ToDictionary(
                    feedback => feedback.Id.ToString(),
                    feedback => JsonSerializer.Serialize(feedback)
                );

                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    [.. hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value))]
                );
                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            feedbacks = filter?.Apply(feedbacks) ?? feedbacks;
            feedbacks = sort?.Apply(feedbacks) ?? feedbacks;

            var totalFeedbacks = feedbacks.Count();
            feedbacks = feedbacks.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PaginatedResult<Feedback>
            {
                Items = feedbacks.ToList(),
                TotalCount = totalFeedbacks,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Feedback?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedFeedback = await _redisDatabase.StringGetAsync(cacheKey);

            if (!cachedFeedback.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<Feedback>(cachedFeedback!);
            }

            _logger.LogInformation("Fetched from DB.");
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == id);

            if (feedback != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(feedback), _cacheExpiration);
            }

            return feedback;
        }

        public async Task UpdateAsync(Feedback entity)
        {
            var feedbackToUpdate = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == entity.Id) ?? throw new KeyNotFoundException("Feedback not found");
            _context.Entry(feedbackToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            await _redisDatabase.StringSetAsync($"{_cacheKeyPrefix}{entity.Id}", JsonSerializer.Serialize(entity), _cacheExpiration);
        }
    }
}