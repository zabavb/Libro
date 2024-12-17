﻿using Microsoft.EntityFrameworkCore;
using System.Text;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Models.Extensions;

namespace UserAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly ILogger<IUserRepository> _logger;
        private  string _message;

        public UserRepository(UserDbContext context, ILogger<IUserRepository> logger, string message)
        {
            _context = context;
            _logger = logger;
            _message = message;
        }

        public async Task<PaginatedResult<User>> GetAllEntitiesPaginatedAsync(int pageNumber, int pageSize, string searchTerm, UserFilter? filter)
        {
            IEnumerable<User> users;
            if (string.IsNullOrWhiteSpace(searchTerm))
                users = await SearchEntitiesAsync(searchTerm);
            else
                users = _context.Users.AsNoTracking();
            if (users.Any() && filter != null)
                users = await FilterEntitiesAsync(users, filter);

            var totalUsers = await Task.FromResult(users.Count());

            users = await Task.FromResult(users.Skip((pageNumber - 1) * pageSize).Take(pageSize));

            if (users == null)
            {
                _message = "Failed to fetch users.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }
            else
                _logger.LogInformation("Successfully fetched {Count} users.", users.Count());

            return new PaginatedResult<User>
            {
                Items = (ICollection<User>)users,
                TotalCount = totalUsers,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<User?> GetEntityByIdAsync(Guid id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                _message = $"User with ID {id} not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            else
                _logger.LogInformation($"User with ID {id} found.");

            return user;
        }

        public async Task<IEnumerable<User>> SearchEntitiesAsync(string searchTerm)
        {
            var users = await _context.Users
                .AsNoTracking()
                .Where(u => u.FirstName.Contains(searchTerm)
                            || u.LastName!.Contains(searchTerm)
                            || u.Email.Contains(searchTerm))
                .ToListAsync();

            if (users == null)
            {
                _message = "Failed to search users.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }
            else
                _logger.LogInformation($"{users.Count} users searched.");


            return users;
        }

        public async Task<IEnumerable<User>> FilterEntitiesAsync(IEnumerable<User> users, UserFilter filter)
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

        public async Task AddEntityAsync(User entity)
        {
            if (entity == null)
            {
                _message = "User was not provided for creation.";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message, nameof(entity));
            }

            try
            {
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User [{entity}] successfully created.");
            }
            catch (ArgumentNullException ex)
            {
                _message = "User entity cannot be null.";
                _logger.LogError(_message);
                throw new ArgumentException(_message, ex);
            }
            catch (Exception ex)
            {
                _message = "Error occurred while adding the user to the database.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task UpdateEntityAsync(User entity)
        {
            if (entity == null)
            {
                _message = "User was not provided for update.";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message, nameof(entity));
            }

            if (!await _context.Users.AnyAsync(u => u.UserId == entity.UserId))
            {
                _message = $"User with ID {entity.UserId} does not exist.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User successfully updated to [{entity}].");
        }

        public async Task DeleteEntityAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                _message = $"User with ID [{id}] not found for deletion.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User with ID [{id}] successfully deleted.");
        }
    }
}