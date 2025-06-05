using Library.DTOs.Book;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services.Interfaces
{
    public interface IBookServiceClient
    {
        Task<ICollection<string>> GetAllBookNamesAsync(ICollection<Guid> ids);
        Task<BookDetails> GetBookAsync(Guid id);
        Task<ICollection<FeedbackForUserDetails>> GetAllFeedbacksAsync(Guid userId);
        Task<BookOrderDetails> GetBookWithAuthor(Guid bookId);
        Task<ICollection<Feedback>> GetNumberOfFeedbacks(int amount);
    }
}