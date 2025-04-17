using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services.Interfaces
{
    public interface IBookServiceClient
    {
        Task<ICollection<string>> GetAllBookNamesAsync(ICollection<Guid> ids);
        Task<ICollection<FeedbackForUserDetails>> GetAllFeedbacksAsync(Guid userId);
    }
}