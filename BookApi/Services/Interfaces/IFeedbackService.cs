using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Extensions;

namespace BookAPI.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<PaginatedResult<FeedbackDto>> GetFeedbacksAsync(int pageNumber, int pageSize, FeedbackFilter? filter, FeedbackSort? sort);
        Task<FeedbackDto> GetFeedbackByIdAsync(Guid id);
        Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto FeedbackDto);
        Task<FeedbackDto> UpdateFeedbackAsync(Guid id, FeedbackDto FeedbackDto);
        Task<bool> DeleteFeedbackAsync(Guid id);


    }
}
