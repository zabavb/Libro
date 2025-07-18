﻿using APIComposer.GraphQL.Services.Interfaces;
using AutoMapper;
using Library.Common;
using Library.DTOs.Book;
using Library.DTOs.Order;
using OrderAPI;



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
            OrderFilter? filter = null,
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
           OrderFilter? filter = null,
           OrderSort? sort = null)
        {
            var orders = await orderClient.GetAllOrdersAsync(pageNumber, pageSize, searchTerm, filter, sort);
            var ordersWithUsers = new List<OrderInput>();

            foreach (var order in orders.Items)
            {
                var user = await userClient.GetUserAsync(order.UserId);
                var subscription = await userClient.GetUserSubscriptionAsync(order.UserId);

                List<BookForOrder> books = new List<BookForOrder>();
                var bookIds = order.Books.Keys;
                foreach (var bookId in bookIds)
                {
                    BookDetails book = await bookClient.GetBookAsync(bookId);

                    BookForOrder bookForOrder = new BookForOrder
                    {
                        BookId = book.BookId.ToString(),
                        Title = book.Title,
                        Price = book.Price,
                        Quantity = order.Books[bookId]
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

        [GraphQLName("order")]
        public async Task<Order> GetOrderAsync(
           [Service] IOrderServiceClient orderClient,
           Guid id)
        {
            var order = await orderClient.GetOrderAsync(id);

            return order;
        }

        [GraphQLName("allOrderDetails")]
            public async Task<PaginatedResult<OrderDetails>> GetOrderDetailsAsync(
            [Service] IOrderServiceClient orderClient,
            [Service] IBookServiceClient bookClient,
            [Service] IMapper mapper,
            int pageNumber,
            int pageSize,
            string? searchTerm,
            OrderFilter filter,
            OrderSort sort)
        {
            var orders = await orderClient.GetAllOrdersAsync(pageNumber, pageSize, searchTerm, filter, sort);
            var orderDetails = new List<OrderDetails>();
            foreach (var order in orders.Items)
            {
                var bookDetails = new List<BookOrderDetails>();

                foreach (var book in order.Books)
                {
                    var bookDetail = await bookClient.GetBookWithAuthor(book.Key);
                    bookDetail.Amount = book.Value;
                    bookDetails.Add(bookDetail);
                }

                orderDetails.Add(new OrderDetails()
                {
                    OrderId = order.Id,
                    Status = order.Status,
                    Created = order.OrderDate,
                    Price = order.Price,
                    OrderBooks = bookDetails,
                });
            }

            return new PaginatedResult<OrderDetails>()
            {
                Items = orderDetails,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = orders.TotalCount,
            }; ;
        }
    }
}

