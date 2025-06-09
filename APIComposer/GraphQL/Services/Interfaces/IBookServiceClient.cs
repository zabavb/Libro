using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
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
        Task<ICollection<Feedback>> GetNumberOfFeedbacks(int amount, Guid bookId);
        Task<PaginatedResult<FeedbackAdminCard>?> GetFeedbacksAsync(
          int pageNumber = 1,
          int pageSize = 10,
          string? searchTerm = null,
          FeedbackFilter? filter = null,
          FeedbackSort? sort = null);

        Task<ICollection<BookLibraryItem>> GetAllDigitalBooks(ICollection<Guid> ids);
    }
}