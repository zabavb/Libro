using Library.Common;
using Library.DTOs.Book;
using Library.DTOs.UserRelated.User;

namespace UserAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedResult<Dto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Filter? filter,
            Sort? sort
        );

        Task<UserWithSubscriptionsDto?> GetByIdAsync(Guid id);
        Task CreateAsync(Dto dto);
        Task UpdateAsync(Dto dto);
        Task DeleteAsync(Guid id);

        Task<UserDisplayData> GetUserDisplayDataAsync(Guid id);
    }
}