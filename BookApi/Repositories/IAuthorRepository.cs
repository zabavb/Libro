
using BookApi.Models;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories
{
    public interface IAuthorRepository : IManagable<Author>
    {
        Task<List<Author>> GetAllAsync();

    }
}
