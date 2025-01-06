using Library.Extensions;
using Library.Sortings;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using UserAPI.Data;
using UserAPI.Models;

namespace UserAPI.Repositories
{
    public class UserRepository(UserDbContext context, IConnectionMultiplexer redis, ILogger<IUserRepository> logger) : IUserRepository
    {
        private readonly UserDbContext _context = context;
        private readonly IDatabase _redisDatabase = redis.GetDatabase();
        private readonly string _cacheKeyPrefix = "User_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);
        private readonly ILogger<IUserRepository> _logger = logger;

        public async Task<PaginatedResult<User>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, Filter? filter, Sort? sort)
        {
            IEnumerable<User> users;

            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedUsers = await _redisDatabase.HashGetAllAsync(cacheKey);
            if (cachedUsers.Length > 0)
            {
                users = cachedUsers.Select(entry => JsonSerializer.Deserialize<User>(entry.Value!)!);
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                users = _context.Users.AsNoTracking();
                _logger.LogInformation("Fetched from DB.");

                var hashEntries = users.ToDictionary(
                    user => user.UserId.ToString(),
                    user => JsonSerializer.Serialize(user)
                );
                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value)).ToArray()
                );
                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            if (users.Any() && !string.IsNullOrWhiteSpace(searchTerm))
                users = await SearchAsync(users, searchTerm);
            if (users.Any() && filter != null)
                users = await FilterAsync(users, filter);
            if (users.Any() && sort != null)
                users = await SortAsync(users, sort);

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

        public async Task<IEnumerable<User>> SearchAsync(IEnumerable<User> users, string searchTerm)
        {
            searchTerm.ToLower();

            if (users == null)
                return await _context.Users
                    .AsNoTracking()
                    .Where(u => u.FirstName.ToLower().Contains(searchTerm)
                                || u.LastName!.ToLower().Contains(searchTerm)
                                || u.Email.Contains(searchTerm))
                    .ToListAsync();

            return await Task.FromResult(
                users.Where(u => u.FirstName.ToLower().Contains(searchTerm)
                            || u.LastName!.ToLower().Contains(searchTerm)
                            || u.Email.Contains(searchTerm))
                );
        }

        public async Task<IEnumerable<User>> FilterAsync(IEnumerable<User> users, Filter filter)
        {
            var query = users.AsQueryable();

            if (filter.Role.HasValue)
                query = query.Where(u => u.Role.Equals(filter.Role));


            if (filter.DateOfBirthStart.HasValue)
                query = query.Where(u => u.DateOfBirth >= filter.DateOfBirthStart.Value);

            if (filter.DateOfBirthEnd.HasValue)
                query = query.Where(u => u.DateOfBirth <= filter.DateOfBirthEnd.Value);

            if (filter.HasSubscription)
                query = query.Where(u => u.SubscriptionId.Equals(filter.HasSubscription));

            return await Task.FromResult(query.ToList());
        }

        public async Task<IEnumerable<User>> SortAsync(IEnumerable<User> users, UserSort sort)
        {
            var query = users.AsQueryable();

            if (sort.FirstName != Bool.NULL)
                query = sort.FirstName == Bool.ASCENDING
                    ? query.OrderBy(u => u.FirstName)
                    : query.OrderByDescending(u => u.FirstName);

            if (sort.LastName != Bool.NULL)
                query = sort.LastName == Bool.ASCENDING
                    ? query.OrderBy(u => u.LastName)
                    : query.OrderByDescending(u => u.LastName);

            if (sort.DateOfBirth != Bool.NULL)
                query = sort.DateOfBirth == Bool.ASCENDING
                    ? query.OrderBy(u => u.DateOfBirth)
                    : query.OrderByDescending(u => u.DateOfBirth);

            if (sort.Email != Bool.NULL)
                query = sort.Email == Bool.ASCENDING
                    ? query.OrderBy(u => u.Email)
                    : query.OrderByDescending(u => u.Email);

            if (sort.PhoneNumber != Bool.NULL)
                query = sort.PhoneNumber == Bool.ASCENDING
                    ? query.OrderBy(u => u.PhoneNumber)
                    : query.OrderByDescending(u => u.PhoneNumber);

            return await Task.FromResult(query.ToList());
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
            var user = await _context.Users.FindAsync(id);
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

        public async Task CreateAsync(User entity)
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
