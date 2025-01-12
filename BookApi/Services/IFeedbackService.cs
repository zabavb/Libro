namespace BookAPI.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackDto>> GetFeedbacksAsync();
        Task<FeedbackDto> GetFeedbackByIdAsync(Guid id);
        Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto FeedbackDto);
        Task<FeedbackDto> UpdateFeedbackAsync(Guid id, FeedbackDto FeedbackDto);
        Task<bool> DeleteFeedbackAsync(Guid id);


    }
}
