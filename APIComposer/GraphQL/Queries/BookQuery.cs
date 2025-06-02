using APIComposer.GraphQL.Services.Interfaces;
using AutoMapper;
using Library.DTOs.Book;

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


        // method that returns purchased eBooks and audiobooks for user
        [GraphQLName("libraryBooks")]
        public async Task<List<LibraryBook>> LibraryBooks(
            [Service] IBookServiceClient bookClient,
            [Service] IOrderServiceClient orderClient,
            [Service] IMapper mapper,
            Guid userId)
        {
            List<LibraryBook> libraryBooks = new List<LibraryBook>();
            var bookIds = await orderClient.GetPurchasedBookIds(userId);
            foreach (var bookId in bookIds) { 
                var book = bookClient.GetBookAsync(bookId);
                var author = bookClient.GetAuthorAsync(book.Result.AuthorId);
                LibraryBook libraryBook = 
                    new LibraryBook
                    {
                        Id = bookId,
                        ImageUrl = book.Result.ImageUrl,
                        Title = book.Result.Title,
                        Type = book.Result.Format,
                        AuthorFullName = author.Result.Name,
                    };
                libraryBooks.Add(libraryBook);

            }
            return libraryBooks;
        }
    }
}
