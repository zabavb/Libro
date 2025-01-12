using BookApi.Models;
using Library.Interfaces;

namespace BookAPI.Repositories
{
    public interface IPublisherRepository : IManagable<Publisher>
    {
        Task<List<Publisher>> GetAllAsync();

    }
}
