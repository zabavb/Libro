using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface IFeedbackRepository : IManageable<Feedback>
    {
        Task<PaginatedResult<FeedbackAdminCard>> GetAllAsync(
            int pageNumber,
            int pageSize,
            FeedbackFilter? filter,
            FeedbackSort? sort
        );

        Task<ICollection<FeedbackForUserDetails>> GetAllForUserDetailsAsync(Guid userId);

        Task<ICollection<Feedback>> GetNumberOfFeedbacks(int amount, Guid bookId);
    }
}