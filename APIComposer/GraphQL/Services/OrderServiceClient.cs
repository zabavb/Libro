using System.Net.Http.Headers;
using APIComposer.GraphQL.Services.Interfaces;
using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services
{
    public class OrderServiceClient(HttpClient http, IHttpContextAccessor httpContextAccessor) : IOrderServiceClient
    {
        private readonly HttpClient _http = http;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private void SetAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }
        }

        public async Task<OrderForUserCard> GetOrderAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var response = await _http.GetFromJsonAsync<OrderForUserCard>($"orders/for-user/card/{id}");
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
                SetAuthHeader();
                var response =
                    await _http.GetFromJsonAsync<ICollection<OrderForUserDetails>>($"orders/for-user/details/{id}");
                return response ?? new List<OrderForUserDetails>();
            }
            catch (Exception)
            {
                return new List<OrderForUserDetails>();
            }
        }
    }
}