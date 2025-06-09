using APIComposer.GraphQL.Services.Interfaces;
using AutoMapper;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.DTOs.Book;
using Library.DTOs.UserRelated.User;
using OrderAPI;
using System.Reflection;
using UserAPI.Models;

namespace APIComposer.GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class BookQuery
    {

        // method that returns url to image of 9 most ordered book for last n days
        [GraphQLName("mostOrderedBooks")]
        public async Task<List<string?>> GetMostOrderedBooks(
            [Service] IOrderServiceClient orderClient,
            [Service] IBookServiceClient bookClient,
            int days
            )
        {
            var booksIds = orderClient.GetMostOrderedBooksAsync(30).Result;
            var booksImages = new List<string?>();
            foreach (var bookId in booksIds)
            {
                var book = await bookClient.GetBookAsync(bookId);
                booksImages.Add(book.ImageUrl);
            }
            return booksImages;
        }

        [GraphQLName("getBookDetails")]
        public async Task<BookDetails> GetBookDetails(
                [Service] IBookServiceClient bookClient,
                [Service] IUserServiceClient userClient,
                [Service] IMapper mapper,
                Guid bookId
                )
        {
            BookDetails book = await bookClient.GetBookAsync(bookId);
            List<FeedbackCard> cards = new List<FeedbackCard>();
            ICollection<Feedback> feedbacks = await bookClient.GetNumberOfFeedbacks(2, book.BookId);
            foreach (var feedback in feedbacks)
            {
                var reviewer = await userClient.GetUserDisplayData(feedback.UserId);
                cards.Add(mapper.Map<FeedbackCard>((feedback, reviewer)));
            }

            book.LatestFeedback = cards;

            return book;
        }

        [GraphQLName("getFeedbacksForAdmin")]
        public async Task<PaginatedResult<FeedbackAdminCard>> GetFeedbacksForAdmin(
            [Service] IBookServiceClient bookClient,
            [Service] IUserServiceClient userClient,
            [Service] IMapper mapper,
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            FeedbackFilter? filter = null,
            FeedbackSort? sort = null)
        {
            PaginatedResult<FeedbackAdminCard>? feedbacks = await bookClient.GetFeedbacksAsync(pageNumber, pageSize, searchTerm, filter, sort);
            if (feedbacks == null)
                return null;

            foreach (var feedback in feedbacks.Items)
            {
                var reviewer = await userClient.GetUserDisplayData(feedback.UserId ?? Guid.Empty);
                feedback.UserId = null;
                feedback.UserName = reviewer!.UserName;
            }


            return feedbacks;
        }

        [GraphQLName("getUserOwnedBooks")]
        public async Task<PaginatedResult<BookLibraryItem>> GetUserOwnedBooks(
            [Service] IOrderServiceClient orderClient,
            [Service] IBookServiceClient bookClient,
            Guid userId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var userOrders = await orderClient.GetAllOrdersAsync(1, int.MaxValue, string.Empty, new OrderFilter() { UserId = userId }, null);

            List<Guid> books = new List<Guid>();
            foreach (var order in userOrders.Items)
            {
                foreach (KeyValuePair<Guid, int> book in order.Books)
                {
                    if (!books.Contains(book.Key))
                    {
                        books.Add(book.Key);
                    }
                }
            }

            ICollection<BookLibraryItem> bookLibraryItems = await bookClient.GetAllDigitalBooks(books);


            return new PaginatedResult<BookLibraryItem>()
            {
                Items = bookLibraryItems.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList(),
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalCount = bookLibraryItems.Count
            };
        }
    }
}
