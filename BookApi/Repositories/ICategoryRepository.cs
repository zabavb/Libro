using BookApi.Models;
using Library.Interfaces;

namespace BookAPI.Repositories
{
    public interface ICategoryRepository : IManagable<Category>
    {
        Task<List<Category>> GetAllAsync();

    }
}
