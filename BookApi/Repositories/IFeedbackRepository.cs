using BookApi.Models;
using Library.Interfaces;

namespace BookAPI.Repositories
{
    public interface IFeedbackRepository : IManagable<Feedback>
    {
        Task<List<Feedback>> GetAllAsync();

    }
}
