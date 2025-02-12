using BookAPI.Models;
using BookAPI.Models.Sortings;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface IPublisherRepository : IManagable<Publisher>
    {
        Task<PaginatedResult<Publisher>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, PublisherSort? sort);
        Task DeleteAsync(Guid id);
    }
}
