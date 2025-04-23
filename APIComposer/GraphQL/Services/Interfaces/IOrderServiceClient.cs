using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services.Interfaces
{
    public interface IOrderServiceClient
    {
        Task<OrderForUserCard> GetOrderAsync(Guid id);
        Task<ICollection<OrderForUserDetails>> GetAllOrdersAsync(Guid id);
    }
}