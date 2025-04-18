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
        Task<PaginatedResult<Feedback>> GetAllAsync(
            int pageNumber,
            int pageSize,
            FeedbackFilter? filter,
            FeedbackSort? sort
        );

        Task<ICollection<FeedbackForUserDetails>> GetAllForUserDetailsAsync(Guid userId);
    }
}