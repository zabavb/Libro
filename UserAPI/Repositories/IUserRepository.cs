using Library.Extensions;
using Library.Interfaces;
using UserAPI.Models;

namespace UserAPI.Repositories
{
    public interface IUserRepository : IManagable<User>
    {
        Task<PaginatedResult<User>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, Filter? filter, Sort? sort);
        Task<IEnumerable<User>> FilterAsync(IEnumerable<User> users, Filter filter);
        Task<IEnumerable<User>> SearchAsync(IEnumerable<User> users, string searchTerm);
        Task DeleteAsync(Guid id);
    }
}