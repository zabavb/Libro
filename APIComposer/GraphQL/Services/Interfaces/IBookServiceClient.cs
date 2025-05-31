using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services.Interfaces
{
    public interface IBookServiceClient
    {
        Task<ICollection<string>> GetAllBookNamesAsync(ICollection<Guid> ids);
        Task<Library.DTOs.Book.Book> GetBookAsync(Guid id);
        Task<ICollection<FeedbackForUserDetails>> GetAllFeedbacksAsync(Guid userId);
        Task<BookOrderDetails> GetBookWithAuthor(Guid bookId);
    }
}