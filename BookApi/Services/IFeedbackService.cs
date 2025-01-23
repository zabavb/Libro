using Library.Extensions;

namespace BookAPI.Services
{
    public interface IFeedbackService
    {
        Task<PaginatedResult<FeedbackDto>> GetFeedbacksAsync(int pageNumber, int pageSize);
        Task<FeedbackDto> GetFeedbackByIdAsync(Guid id);
        Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto FeedbackDto);
        Task<FeedbackDto> UpdateFeedbackAsync(Guid id, FeedbackDto FeedbackDto);
        Task<bool> DeleteFeedbackAsync(Guid id);


    }
}
