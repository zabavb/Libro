using APIComposer.GraphQL.Services.Interfaces;
using AutoMapper;
using Library.DTOs.Book;
using Library.DTOs.UserRelated.User;
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
    }
}
