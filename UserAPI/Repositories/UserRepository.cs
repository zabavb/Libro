using Library.Common;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Models.Searches;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserRepository(UserDbContext context, IConnectionMultiplexer redis, ILogger<IUserRepository> logger) : IUserRepository
    {
        private readonly UserDbContext _context = context;
        private readonly IDatabase _redisDatabase = redis.GetDatabase();
        public readonly string _cacheKeyPrefix = "User_";
        public readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalDefaults.DefaultCacheExpirationTime);
        private readonly ILogger<IUserRepository> _logger = logger;

        public async Task<PaginatedResult<User>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Filter? filter,
            Sort? sort
        ) {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            string cacheKey = $"{_cacheKeyPrefix}All_Page:{pageNumber}_Size:{pageSize}_Search:{searchTerm}";
            var cachedUsers = await _redisDatabase.HashGetAllAsync(cacheKey);

            IQueryable<User> users;

            if (cachedUsers.Length > 0)
            {
                users = cachedUsers
                    .Select(entry => JsonSerializer.Deserialize<User>(entry.Value!, options)!)
                    .AsQueryable()
                    .AsNoTracking();

                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                users = _context.Users
                    .AsNoTracking()
                    .Include(u => u.Subscription);
                _logger.LogInformation("Fetched from DB.");

                var hashEntries = users.ToDictionary(
                    user => user.UserId.ToString(),
                    user => JsonSerializer.Serialize(user, options)
                );

                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    [.. hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value))]
                );
                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            if (users.Any() && !string.IsNullOrWhiteSpace(searchTerm))
                users = users.Search(searchTerm,
                    u => u.FirstName,
                    u => u.LastName!,
                    u => u.Email!,
                    u => u.PhoneNumber!
                );

            if (users.Any() && filter != null)
                users = filter.Apply(users);

            if (users.Any() && sort != null)
                users = sort.Apply(users);

            var total = await users.CountAsync();
            var paginatedUsers = await users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<User>
            {
                Items = paginatedUsers,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{_cacheKeyPrefix}{id}";
            string fieldKey = id.ToString();

            var cachedUser = await _redisDatabase.HashGetAsync(cacheKey, fieldKey);

            if (!cachedUser.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<User>(cachedUser!);
            }

            _logger.LogInformation("Fetched from DB.");
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.Subscription)
                .FirstOrDefaultAsync(u => u.UserId.Equals(id));

            if (user != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    fieldKey,
                    JsonSerializer.Serialize(user)
                );

                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
            }

            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            string cacheKey = $"{_cacheKeyPrefix}{email}";
            string fieldKey = email;

            var cachedUser = await _redisDatabase.HashGetAsync(cacheKey, fieldKey);

            if (!cachedUser.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<User>(cachedUser!);
            }

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email!.Equals(email));
            _logger.LogInformation("Fetched from DB.");
            
            if (user != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    fieldKey,
                    JsonSerializer.Serialize(user)
                );

                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
            }

            return user;
        }

        public async Task CreateAsync(User user)
        {
            user.UserId = Guid.NewGuid();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId.Equals(user.UserId))
                ?? throw new InvalidOperationException();

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id) ?? throw new KeyNotFoundException();
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
