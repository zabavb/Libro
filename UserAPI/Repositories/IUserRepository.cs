using Library.Extensions;
using Library.Interfaces;
using UserAPI.Models;

namespace UserAPI.Repositories
{
    public interface IUserRepository : IManagable<User>
    {
        Task<PaginatedResult<User>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter);
        Task<IEnumerable<User>> FilterAsync(IEnumerable<User> entities, Filter filter);
        Task<IEnumerable<User>> SearchAsync(string searchTerm, IEnumerable<User> data);
    }
}