using Amazon.S3.Model;
using APIComposer.GraphQL.Services.Interfaces;
using AutoMapper;
using HotChocolate.Validation;
using Library.Common;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;
using OrderAPI;
using UserAPI.Models;


namespace APIComposer.GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class OrderQuery
    {

        

        [GraphQLName("allOrdersWithUserName")]
        public async Task<PaginatedResult<OrderWithUserName>?> GetAllOrdersWithUserNameAsync(
            [Service] IOrderServiceClient orderClient,
            [Service] IUserServiceClient userClient,
            [Service] IMapper mapper,
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            Filter? filter = null,
            OrderSort? sort = null)
        {
            var orders = await orderClient.GetAllOrdersAsync(pageNumber, pageSize, searchTerm, filter, sort);
            var ordersWithUsers = new List<OrderWithUserName>();

            foreach (var order in orders.Items)
            {
                var user = await userClient.GetUserAsync(order.UserId);

                var orderWithUser = new OrderWithUserName
                {
                    OrderUiId = order.Id.ToString(),
                    Price = order.Price,
                    Status = order.Status,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                ordersWithUsers.Add(orderWithUser);
            }

            return new PaginatedResult<OrderWithUserName>()
            {
                Items = ordersWithUsers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = orders.TotalCount,
            };
        }

        // GetAllEditOrderAsync => order, book, user with subscription via user api,
        
        [GraphQLName("allOrderIntAsync")]
        public async Task<PaginatedResult<OrderInput>?> GetAllEditOrderAsync(
           [Service] IOrderServiceClient orderClient,
           [Service] IUserServiceClient userClient,
           [Service] IBookServiceClient bookClient,
           [Service] IMapper mapper,
           int pageNumber = 1,
           int pageSize = 10,
           string? searchTerm = null,
           Filter? filter = null,
           OrderSort? sort = null)
        {
            var orders = await orderClient.GetAllOrdersAsync(pageNumber, pageSize, searchTerm, filter, sort);
            var ordersWithUsers = new List<OrderInput>();

            foreach (var order in orders.Items)
            {
                var user = await userClient.GetUserAsync(order.UserId);
                var subscription = await userClient.GetUserSubscriptionAsync(order.UserId);

                List<BookForOrder> books = new List<BookForOrder>();
                foreach (var orderedBook in order.OrderedBooks)
                {
                    Library.DTOs.Book.Book book = await bookClient.GetBookAsync(orderedBook.BookId);

                    BookForOrder bookForOrder = new BookForOrder
                    {
                        BookId = book.BookId.ToString(),
                        Title = book.Title,
                        Price = book.Price,
                        Quantity = orderedBook.Quantity,
                    };
                    books.Add(bookForOrder);
                }

                var orderInput = new OrderInput
                {
                    OrderUiId = order.Id.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    City = order.City,
                    OrderDate = order.OrderDate,
                    DeliveryDate = order.DeliveryDate,
                    Status = order.Status,
                    ExpirationDays = subscription.ExpirationDays,
                    Books = books,
                };
                ordersWithUsers.Add(orderInput);
            }
            return new PaginatedResult<OrderInput>()
            {
                Items = ordersWithUsers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = orders.TotalCount,
            };
        }
    }
}

