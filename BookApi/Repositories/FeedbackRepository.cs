using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Library.Common;
using BookAPI.Data.CachHelper;
using Library.DTOs.UserRelated.User;

namespace BookAPI.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly BookDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILogger<FeedbackRepository> _logger;
        private readonly string _cacheKeyPrefix = "Feedback_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);

        public FeedbackRepository(BookDbContext context, ICacheService cacheService, ILogger<FeedbackRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task CreateAsync(Feedback entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _context.Feedbacks.Add(entity);
            await _context.SaveChangesAsync();

            string cacheKey = $"{_cacheKeyPrefix}{entity.Id}";
            await _cacheService.SetAsync(cacheKey, entity, _cacheExpiration);

            string allFeedbacksCacheKey = $"{_cacheKeyPrefix}All";
            var cachedFeedbacks = await _cacheService.GetAsync<List<Feedback>>(allFeedbacksCacheKey) ??
                                  new List<Feedback>();

            cachedFeedbacks.Add(entity);
            await _cacheService.SetAsync(allFeedbacksCacheKey, cachedFeedbacks, _cacheExpiration);

            _logger.LogInformation("New Feedback added to DB and cached.");
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == id) ??
                           throw new KeyNotFoundException("Feedback not found");
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}{id}");

            string allFeedbacksCacheKey = $"{_cacheKeyPrefix}All";
            var cachedFeedbacks = await _cacheService.GetAsync<List<Feedback>>(allFeedbacksCacheKey);

            if (cachedFeedbacks != null)
            {
                await _cacheService.UpdateListAsync(allFeedbacksCacheKey, default(Feedback), id, _cacheExpiration);
            }
        }


        public async Task<PaginatedResult<Feedback>> GetAllAsync(int pageNumber, int pageSize, FeedbackFilter? filter,
            FeedbackSort? sort)
        {
            string cacheKey = $"{_cacheKeyPrefix}All";
            List<Feedback>? feedbacks = await _cacheService.GetAsync<List<Feedback>>(cacheKey);
            bool isFromCache = feedbacks != null && feedbacks.Count > 0;

            if (isFromCache)
            {
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                feedbacks = await _context.Feedbacks.ToListAsync();
                _logger.LogInformation("Fetched from DB.");

                await _cacheService.SetAsync(cacheKey, feedbacks, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            IQueryable<Feedback> feedbackQuery = feedbacks.AsQueryable();

            if (filter != null)
            {
                feedbackQuery = filter.Apply(feedbackQuery);
            }

            if (sort != null)
            {
                feedbackQuery = sort.Apply(feedbackQuery);
            }

            var totalFeedbacks = feedbackQuery.Count();
            var paginatedFeedbacks = feedbackQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedResult<Feedback>
            {
                Items = paginatedFeedbacks,
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
            var cachedFeedback = await _cacheService.GetAsync<Feedback>(cacheKey);

            if (cachedFeedback != null)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return cachedFeedback;
            }

            _logger.LogInformation("Fetched from DB.");
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == id);

            if (feedback != null)
            {
                await _cacheService.SetAsync(cacheKey, feedback, _cacheExpiration);
            }

            return feedback;
        }

        public async Task<ICollection<FeedbackForUserDetails>> GetAllForUserDetailsAsync(Guid userId)
        {
            string cacheKey = $"{_cacheKeyPrefix}{userId}";
            var cachedFeedback = await _cacheService.GetAsync<ICollection<FeedbackForUserDetails>>(cacheKey);
            if (cachedFeedback != null)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return cachedFeedback;
            }

            var feedbacks = _context.Feedbacks
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .Include(f => f.Book)
                .AsEnumerable()
                .Select(f => new FeedbackForUserDetails()
                {
                    HeadLabel = $"{f.Book.Title} - {f.Id.ToString().Split('-')[4]}",
                    Rating = f.Rating,
                    Comment = f.Comment,
                    Date = f.Date
                }).ToList();
            _logger.LogInformation("Fetched from DB.");

            if (feedbacks.Count > 0)
            {
                await _cacheService.SetAsync(cacheKey, feedbacks, _cacheExpiration);
            }

            return feedbacks;
        }

        public async Task UpdateAsync(Feedback entity)
        {
            var feedbackToUpdate = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == entity.Id) ??
                                   throw new KeyNotFoundException("Feedback not found");
            _context.Entry(feedbackToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            await _cacheService.SetAsync($"{_cacheKeyPrefix}{entity.Id}", entity, _cacheExpiration);

            string allFeedbacksCacheKey = $"{_cacheKeyPrefix}All";
            var cachedFeedbacks = await _cacheService.GetAsync<List<Feedback>>(allFeedbacksCacheKey);

            if (cachedFeedbacks != null)
            {
                await _cacheService.UpdateListAsync(allFeedbacksCacheKey, entity, null, _cacheExpiration);
            }

            _logger.LogInformation("Feedback updated in DB and cached.");
        }

        public async Task<ICollection<Feedback>> GetNumberOfFeedbacks(int amount)
        {
            var feedbacks = await _context.Feedbacks
                .OrderByDescending(f => f.Date)
                .Take(amount)
                .ToListAsync();
            return feedbacks;
        }
    }
}