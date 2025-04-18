using APIComposer.GraphQL.Services.Interfaces;
using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services
{
    public class OrderServiceClient(HttpClient http) : IOrderServiceClient
    {
        private readonly HttpClient _http = http;

        public async Task<OrderForUserCard> GetOrderAsync(Guid id)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<OrderForUserCard>("orders/for-user/card/{id}");
                return response ?? new();
            }
            catch (Exception)
            {
                return new();
            }
        }

        public async Task<ICollection<OrderForUserDetails>> GetAllOrdersAsync(Guid id)
        {
            try
            {
                var response =
                    await _http.GetFromJsonAsync<ICollection<OrderForUserDetails>>("orders/for-user/details/{id}");
                return response ?? new List<OrderForUserDetails>();
            }
            catch (Exception)
            {
                return new List<OrderForUserDetails>();
            }
        }
    }
}