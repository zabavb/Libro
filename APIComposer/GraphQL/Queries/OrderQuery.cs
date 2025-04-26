using Amazon.S3.Model;
using APIComposer.GraphQL.Services.Interfaces;
using AutoMapper;
using Library.Common;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;
using OrderAPI;

namespace APIComposer.GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class OrderQuery
    {


        [GraphQLName("order")] 
        [GraphQLDescription("Gets a single order by its unique ID.")]
        public async Task<Order?> GetOrderByIdAsync(
            [Service] IOrderServiceClient orderClient,
            [GraphQLNonNullType] Guid id)
        {
            return await orderClient.GetOrderByIdAsync(id);
        }

        [GraphQLName("allOrders")]
        public async Task<PaginatedResult<Order>?> GetAllOrdersAsync(
            [Service] IOrderServiceClient orderClient,
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            Filter? filter = null,
            OrderSort? sort = null)
        {
            return await orderClient.GetAllOrdersAsync(pageNumber, pageSize, searchTerm, filter, sort);
        }


    }

}  
