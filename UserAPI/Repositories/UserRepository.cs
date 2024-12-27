using Library.Extensions;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Services;

namespace UserAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly IDatabase _redisDatabase;
        private readonly string _cacheKeyPrefix;
        private readonly TimeSpan _cacheExpiration;
        private readonly ILogger<IUserRepository> _logger;

        public UserRepository(UserDbContext context, IConnectionMultiplexer redis, ILogger<IUserRepository> logger)
        {
            _context = context;
            _redisDatabase = redis.GetDatabase();

            _cacheKeyPrefix = "User_";
            _cacheExpiration = TimeSpan.FromMinutes(10);

            _logger = logger;
        }

        public async Task<PaginatedResult<User>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter)
        {
            IEnumerable<User> users;

            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedUsers = await _redisDatabase.StringGetAsync(cacheKey);
            if (!cachedUsers.IsNullOrEmpty)
            {
                users = JsonSerializer.Deserialize<ICollection<User>>(cachedUsers!)!;
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                users = _context.Users.AsNoTracking();
                _logger.LogInformation("Fetched from DB.");

                await _redisDatabase.StringSetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(users),
                    _cacheExpiration
                );
                _logger.LogInformation("Set to CACHE.");
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
                users = await SearchAsync(searchTerm, users);
            if (users.Any() && filter != null)
                users = await FilterAsync(users, filter);

            var totalUsers = await Task.FromResult(users.Count());
            users = await Task.FromResult(users.Skip((pageNumber - 1) * pageSize).Take(pageSize));
            ICollection<User> result = new List<User>(users);

            return new PaginatedResult<User>
            {
                Items = result,
                TotalCount = totalUsers,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<User>> SearchAsync(string searchTerm, IEnumerable<User> data)
        {
            if (data == null)
                return await _context.Users
                    .AsNoTracking()
                    .Where(u => u.FirstName.Contains(searchTerm)
                                || u.LastName!.Contains(searchTerm)
                                || u.Email.Contains(searchTerm))
                    .ToListAsync();
            
            return await Task.FromResult(
                data.Where(u => u.FirstName.Contains(searchTerm)
                            || u.LastName!.Contains(searchTerm)
                            || u.Email.Contains(searchTerm))
                );
        }

        public async Task<IEnumerable<User>> FilterAsync(IEnumerable<User> users, Filter filter)
        {
            if (filter.Role.HasValue)
                users = users.Where(u => u.Role.Equals(filter.Role));

            if (filter.DateOfBirthStart.HasValue)
                users = users.Where(u => u.DateOfBirth >= filter.DateOfBirthStart.Value);

            if (filter.DateOfBirthEnd.HasValue)
                users = users.Where(u => u.DateOfBirth <= filter.DateOfBirthEnd.Value);

            if (filter.HasSubscription)
                users = users.Where(u => u.SubscriptionId.Equals(filter.HasSubscription));

            return await Task.FromResult(users);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedProduct = await _redisDatabase.StringGetAsync(cacheKey);

            if (!cachedProduct.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<User>(cachedProduct!);
            }

            _logger.LogInformation($"Fetched from DB.");
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _redisDatabase.StringSetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(user),
                    _cacheExpiration
                );
            }

            return user;
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User entity)
        {
            if (!await _context.Users.AnyAsync(u => u.UserId == entity.UserId))
                throw new InvalidOperationException();

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new KeyNotFoundException();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
