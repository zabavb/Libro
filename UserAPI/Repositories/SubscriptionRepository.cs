using System.Text.Json;
using System.Text.Json.Serialization;
using Library.Common;
using Library.Common.Middleware;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UserAPI.Data;
using UserAPI.Models.Filters;
using UserAPI.Models.Subscription;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class SubscriptionRepository(
        UserDbContext context,
        IConnectionMultiplexer redis,
        ILogger<ISubscriptionRepository> logger
    ) : ISubscriptionRepository
    {
        private readonly UserDbContext _context = context;
        private readonly IDatabase _redisDatabase = redis.GetDatabase();
        private const string CacheKeyPrefix = "Subscription_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalDefaults.cacheExpirationTime);
        private readonly ILogger<ISubscriptionRepository> _logger = logger;

        public async Task<PaginatedResult<Subscription>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                string cacheKey = $"{CacheKeyPrefix}All_Page:{pageNumber}_Size:{pageSize}_Search:{searchTerm}";
                var cachedSubscriptions = await _redisDatabase.HashGetAllAsync(cacheKey);

                IQueryable<Subscription> subscriptions;
                if (cachedSubscriptions.Length > 0)
                {
                    subscriptions = cachedSubscriptions
                        .Select(entry => JsonSerializer.Deserialize<Subscription>(entry.Value!, options)!)
                        .ToList()
                        .AsQueryable()
                        .AsNoTracking();

                    _logger.LogInformation("Fetched from CACHE.");
                }
                else
                {
                    subscriptions = _context.Subscriptions.AsNoTracking();
                    _logger.LogInformation("Fetched from DB.");

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                        subscriptions = subscriptions.SearchBy(searchTerm, s => s.Title);

                    var hashEntries = await subscriptions.ToDictionaryAsync(
                        subscription => subscription.SubscriptionId.ToString(),
                        subscription => JsonSerializer.Serialize(subscription, options)
                    );

                    await _redisDatabase.HashSetAsync(
                        cacheKey,
                        hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value)).ToArray()
                    );
                    await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                    _logger.LogInformation("Set to CACHE.");
                }

                var total = subscriptions.Count();
                var paginatedSubscriptions = subscriptions
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PaginatedResult<Subscription>
                {
                    Items = paginatedSubscriptions,
                    TotalCount = total,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error while fetching subscriptions.", ex);
            }
        }

        public async Task<Subscription?> GetByIdAsync(Guid id)
        {
            try
            {
                string cacheKey = $"{CacheKeyPrefix}{id}";
                string fieldKey = id.ToString();

                var cachedUser = await _redisDatabase.HashGetAsync(cacheKey, fieldKey);

                if (!cachedUser.IsNullOrEmpty)
                {
                    _logger.LogInformation("Fetched from CACHE.");
                    return JsonSerializer.Deserialize<Subscription>(cachedUser!);
                }


                var subscription = await _context.Subscriptions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.SubscriptionId.Equals(id));
                _logger.LogInformation("Fetched from DB.");

                if (subscription != null)
                {
                    _logger.LogInformation("Set to CACHE.");
                    await _redisDatabase.HashSetAsync(
                        cacheKey,
                        fieldKey,
                        JsonSerializer.Serialize(subscription)
                    );

                    await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                }

                return subscription;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Database error while fetching subscription by ID [{id}].", ex);
            }
        }

        public async Task CreateAsync(Subscription subscription)
        {
            try
            {
                await _context.Subscriptions.AddAsync(subscription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Database update error while creating subscription.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error occurred while creating subscribtion.", ex);
            }
        }

        public async Task UpdateAsync(Subscription subscription)
        {
            try
            {
                var existingSubscription = await _context.Subscriptions.FindAsync(subscription.SubscriptionId) ??
                                           throw new KeyNotFoundException(
                                               $"User with ID [{subscription.SubscriptionId}] not found.");

                _context.Entry(existingSubscription).CurrentValues.SetValues(subscription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException(
                    $"Database error while updating subscription [{subscription.SubscriptionId}].",
                    ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                // Removing without fetching whole entity 
                Subscription tmp = new() { SubscriptionId = id };
                _context.Subscriptions.Remove(tmp);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException($"Database error while deleting subscription with ID [{id}].", ex);
            }
        }

        public async Task SubscribeAsync(Guid subscriptionId, Guid userId)
        {
            try
            {
                var (existingSubscription, expirationDays) = await CheckForSubscriptionAsync(subscriptionId, userId);
                if (existingSubscription != null)
                    throw new InvalidOperationException(
                        $"User with ID [{userId}] is already subscribed to this subscription with ID [{subscriptionId}].");

                var expirationDate = DateTime.UtcNow.AddDays(expirationDays);
                var userSubscription = new UserSubscription
                {
                    UserId = userId,
                    SubscriptionId = subscriptionId,
                    ExpirationDate = expirationDate
                };

                await _context.UserSubscriptions.AddAsync(userSubscription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException(
                    $"Database error while subscribing user by ID [{userId}] with subscription by ID [{subscriptionId}].");
            }
        }

        public async Task UnsubscribeAsync(Guid subscriptionId, Guid userId)
        {
            try
            {
                var (existingSubscription, expirationDays) = await CheckForSubscriptionAsync(subscriptionId, userId);

                if (existingSubscription == null)
                    throw new InvalidOperationException(
                        $"User with ID [{userId}] does not have this subscription, with ID [{subscriptionId}], to unsubscribe.");

                _context.UserSubscriptions.Remove(existingSubscription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException(
                    $"Database error while UNsubscribing user, by ID [{userId}], with subscription, by ID [{subscriptionId}].");
            }
        }

        public async Task<ICollection<BySubscription>> GetAllForFilterContentAsync()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };
                string cacheKey = $"{CacheKeyPrefix}All";
                var cachedSubscriptions = await _redisDatabase.HashGetAllAsync(cacheKey);

                IQueryable<BySubscription> subscriptions;
                if (cachedSubscriptions.Length > 0)
                {
                    subscriptions = cachedSubscriptions
                        .Select(entry => JsonSerializer.Deserialize<BySubscription>(entry.Value!, options)!)
                        .AsQueryable()
                        .AsNoTracking();

                    _logger.LogInformation("Fetched from CACHE.");
                }
                else
                {
                    subscriptions = _context.Subscriptions
                        .Select(s => new BySubscription()
                        {
                            Id = s.SubscriptionId,
                            Title = s.Title
                        })
                        .AsNoTracking();
                    _logger.LogInformation("Fetched from DB.");

                    var hashEntries = subscriptions.ToDictionary(
                        subscription => subscription.Id.ToString(),
                        subscription => JsonSerializer.Serialize(subscription, options)
                    );

                    await _redisDatabase.HashSetAsync(
                        cacheKey,
                        [.. hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value))]
                    );
                    await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                    _logger.LogInformation("Set to CACHE.");
                }

                return subscriptions.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error while fetching subscriptions.", ex);
            }
        }

        private async Task<(UserSubscription? Existing, int ExpirationDays)> CheckForSubscriptionAsync(
            Guid subscriptionId,
            Guid userId)
        {
            var user = await _context.Users.FindAsync(userId)
                       ?? throw new KeyNotFoundException($"User with ID [{userId}] not found.");

            var subscription = await _context.Subscriptions.FindAsync(subscriptionId)
                               ?? throw new KeyNotFoundException($"Subscription with ID [{subscriptionId}] not found.");

            var existingSubscription = await _context.UserSubscriptions
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SubscriptionId == subscriptionId);

            return (existingSubscription, subscription.ExpirationDays);
        }
    }
}