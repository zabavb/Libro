using Library.Common;
using Library.Common.Middleware;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Library.DTOs.UserRelated.User;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class UserRepository(UserDbContext context, IConnectionMultiplexer redis, ILogger<IUserRepository> logger)
        : IUserRepository
    {
        private readonly UserDbContext _context = context;
        private readonly IDatabase _redisDatabase = redis.GetDatabase();
        private const string CacheKeyPrefix = "User_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalDefaults.cacheExpirationTime);
        private readonly ILogger<IUserRepository> _logger = logger;

        public async Task<PaginatedResult<User>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Filter? filter,
            Sort? sort
        )
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                string cacheKey = $"{CacheKeyPrefix}All_Page:{pageNumber}_Size:{pageSize}_Search:{searchTerm}";
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
                    users = _context.Users.AsNoTracking();
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
                    users = users.SearchBy(searchTerm,
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
            catch (Exception ex)
            {
                throw new RepositoryException("Error while fetching users.", ex);
            }
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            try
            {
                string cacheKey = $"{CacheKeyPrefix}{id}";
                string fieldKey = id.ToString();

                var cachedUser = await _redisDatabase.HashGetAsync(cacheKey, fieldKey);

                if (!cachedUser.IsNullOrEmpty)
                {
                    _logger.LogInformation("Fetched from CACHE.");
                    return JsonSerializer.Deserialize<User>(cachedUser!);
                }

                var user = await _context.Users
                    .AsNoTracking()
                    .Include(u => u.UserSubscriptions)!
                    .ThenInclude(us => us.Subscription)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                    return null;

                _logger.LogInformation("Fetched from DB.");

                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    fieldKey,
                    JsonSerializer.Serialize(user)
                );
                _logger.LogInformation("Set to CACHE.");

                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);

                return user;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Database error while fetching user by ID [{id}].", ex);
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                string cacheKey = $"{CacheKeyPrefix}{email}";
                string fieldKey = email;

                var cachedUser = await _redisDatabase.HashGetAsync(cacheKey, fieldKey);

                if (!cachedUser.IsNullOrEmpty)
                {
                    _logger.LogInformation("Fetched from CACHE.");
                    return JsonSerializer.Deserialize<User>(cachedUser!);
                }

                User? user = null;

                user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email! == email);
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
            catch (Exception ex)
            {
                throw new RepositoryException($"Database error while fetching user by email [{email}].", ex);
            }
        }

        public async Task CreateAsync(User user)
        {
            try
            {
                user.UserId = Guid.NewGuid();
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Database update error while creating user.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error occurred while creating user.", ex);
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.UserId) ??
                                   throw new KeyNotFoundException($"User with ID [{user.UserId}] not found.");

                _context.Entry(existingUser).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException($"Database error while updating user [{user.UserId}].", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {   
            try
            {
                var user = await _context.Users.FindAsync(id) ??
                           throw new KeyNotFoundException($"User with ID [{id}] not found.");

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException($"Database error while deleting user with ID [{id}].", ex);
            }
        }
    }
}