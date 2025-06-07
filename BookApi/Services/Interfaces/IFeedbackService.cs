using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.DTOs.UserRelated.User;

namespace BookAPI.Services.Interfaces
{
    public interface IFeedbackService
    {
        // Simplified function naming 
        Task<PaginatedResult<FeedbackAdminCard>> GetAllAsync(
            int pageNumber,
            int pageSize,
            FeedbackFilter? filter,
            FeedbackSort? sort
        );

        Task<FeedbackDto> GetByIdAsync(Guid id);
        Task<ICollection<FeedbackForUserDetails>?> GetAllForUserDetailsAsync(Guid userId);

        Task /*<FeedbackDto>*/ CreateAsync(FeedbackDto FeedbackDto);

        // Task<FeedbackDto> UpdateFeedbackAsync(Guid id, FeedbackDto FeedbackDto);     // Should be removed
        Task /*<bool>*/ DeleteAsync(Guid id);
        Task<ICollection<FeedbackDto>> GetNumberOfFeedbacks(int amount, Guid bookId);
    }
}