using Amazon.S3.Model;
using Library.Common;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services.Interfaces
{
    public interface IOrderServiceClient
    {
        Task<OrderForUserCard> GetOrderAsync(Guid id);
        Task<ICollection<OrderForUserDetails>> GetAllOrdersAsync(Guid id);
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<PaginatedResult<Order>?> GetAllOrdersAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Filter? filter,
            OrderAPI.OrderSort? sort);

        Task<List<Guid>> GetMostOrderedBooksAsync(int days);

    }
}