using BookApi.Models;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories
{
    public interface IPublisherRepository : IManagable<Publisher>
    {
        Task<PaginatedResult<Publisher>> GetAllAsync(int pageNumber, int pageSize);
        Task DeleteAsync(Guid id);
    }
}
