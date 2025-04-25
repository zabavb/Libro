using Amazon.S3.Model;
using System.Net.Http.Headers;
using APIComposer.GraphQL.Services.Interfaces;
using Library.Common;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;
using System.Text;

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

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                // Use the correct endpoint from your Order controller
                var response = await _http.GetAsync($"orders/{orderId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null; // Or handle as needed
                }

                if (!response.IsSuccessStatusCode)
                {
                    // Use your existing ErrorHandler or throw an exception
                    await ErrorHandler.HandleErrorResponseAsync(response);
                    return null; // Should not be reached if HandleError throws
                }

                return await response.Content.ReadFromJsonAsync<Order>();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error fetching order by ID {orderId}: {ex.Message}");
                throw;
            }
        }

        public async Task<PaginatedResult<Order>?> GetAllOrdersAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            Filter? filter = null,
            OrderAPI.OrderSort? sort = null)
        {
            try
            {
                var queryString = new StringBuilder($"orders?pageNumber={pageNumber}&pageSize={pageSize}");
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    queryString.Append($"&searchTerm={Uri.EscapeDataString(searchTerm)}");
                }
                if (filter != null && !string.IsNullOrEmpty(filter.S3KeyFilter.ToString()))
                {
                    queryString.Append($"&filterProperty={filter.S3KeyFilter}");
                }
                if (sort != null)
                {
                    // add sorting to query ?
                }

                var response = await _http.GetAsync(queryString.ToString());

                if (!response.IsSuccessStatusCode)
                {
                    await ErrorHandler.HandleErrorResponseAsync(response);
                    return null;
                }

                return (await response.Content.ReadFromJsonAsync<PaginatedResult<Order>>())!;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}