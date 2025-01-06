
using BookApi.Models;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories
{
    public interface IBookRepository : IManagable<Book>
    {
        Task<IEnumerable<Book>> GetAsync();
        Task<IEnumerable<Book>> GetAsync(string searchQuery, string sortBy);

    }
}
