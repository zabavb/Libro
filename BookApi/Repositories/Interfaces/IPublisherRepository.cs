using BookAPI.Models;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface IPublisherRepository : IManageable<Publisher,Publisher>
    {
        Task<PaginatedResult<Publisher>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, PublisherSort? sort);
        Task DeleteAsync(Guid id);
    }
}
