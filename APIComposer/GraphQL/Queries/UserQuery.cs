using APIComposer.GraphQL.Services.Interfaces;
using AutoMapper;
using Library.Common;
using Library.DTOs.UserRelated.User;
using UserAPI.Models.Filters;
using UserAPI.Models.Sorts;

namespace APIComposer.GraphQL.Queries
{
    public class UserQuery
    {
        [GraphQLName("allUsers")]
        public async Task<PaginatedResult<UserCard>> GetAllUsersAsync(
            [Service] IUserServiceClient userClient,
            [Service] IOrderServiceClient orderClient,
            [Service] IMapper mapper,
            int pageNumber,
            int pageSize,
            string? searchTerm,
            UserFilter filter,
            UserSort sort)
        {
            var users = await userClient.GetAllUsersAsync(pageNumber, pageSize, searchTerm, filter, sort);
            var userCards = new List<UserCard>();

            foreach (var user in users.Items)
            {
                var order = await orderClient.GetOrderAsync(user.Id);
                var userCard = mapper.Map<UserCard>((user, order));
                userCards.Add(userCard);
            }

            return new PaginatedResult<UserCard>()
            {
                Items = userCards,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = users.TotalCount,
            };
        }

        [GraphQLName("user")]
        public async Task<UserDetails> GetUserAsync(
            [Service] IUserServiceClient userClient,
            [Service] IOrderServiceClient orderClient,
            [Service] IBookServiceClient bookClient,
            [Service] IMapper mapper,
            Guid id)
        {
            var user = await userClient.GetUserAsync(id);
            var orders = await orderClient.GetAllOrdersAsync(id);
            var feedbacks = await bookClient.GetAllFeedbacksAsync(id);

            return mapper.Map<UserDetails>((user, orders, feedbacks));
        }
    }
}