using Amazon.S3.Model;
using Library.Common;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;
using OrderAPI;

namespace APIComposer.GraphQL.Services.Interfaces
{
    public interface IOrderServiceClient
    {
        Task<Order> GetOrderAsync(Guid id);
        Task<ICollection<OrderForUserDetails>> GetAllOrdersAsync(Guid id);
        Task<Order> GetOrderByIdAsync(Guid id);

        Task<OrderForUserCard> GetOrderForUserAsync(Guid id);

        Task<PaginatedResult<Order>> GetAllOrdersAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            OrderFilter? filter,
            OrderAPI.OrderSort? sort);

        Task<PaginatedResult<OrderWithUserName>?> GetAllOrdersWithUserNameAsync(
           int pageNumber,
           int pageSize,
           string? searchTerm,
           OrderFilter? filter,
           OrderSort? sort);


        Task<List<Guid>> GetMostOrderedBooksAsync(int days);

        Task<List<Guid>> GetPurchasedBookIds(Guid id);

    }
}