using Library.Common;
using Library.DTOs.Book;
using Library.Interfaces;
using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IUserRepository : IManageable<User>
    {
        Task<PaginatedResult<User>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Filter? filter,
            Sort? sort
        );

        Task<User?> GetByEmailAsync(string email);

        Task<UserDisplayData> GetUserDisplayDataAsync(Guid id);
    }
}