using Amazon.S3.Model;
using System.Net.Http.Headers;
using APIComposer.GraphQL.Services.Interfaces;
using Library.Common;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;
using System.Text;
using System.Text.Json;

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
                SetAuthHeader();
                var response = await _http.GetAsync($"orders/{orderId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    await ErrorHandler.HandleErrorResponseAsync(response);
                    return null;
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
                SetAuthHeader();

                var query = QueryBuilder.BuildQuery(
                    new { pageNumber, pageSize, searchTerm },
                    filter,
                    sort
                );

                var response = await _http.GetAsync(query.ToString());

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

        public async Task<List<Guid>> GetMostOrderedBooksAsync(int days)
        {
            SetAuthHeader();

            var response = await _http.GetAsync($"most-ordered-books/{days}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error fetching most ordered books: {error}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Guid>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Guid>();
        }
    }
}