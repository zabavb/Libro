using APIComposer.GraphQL.Services.Interfaces;
using AutoMapper;

namespace APIComposer.GraphQL.Queries
{
    public class BookQuery
    {
        
        public class OrderQuery
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
        }
    }
}
