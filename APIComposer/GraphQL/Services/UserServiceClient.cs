using System.Net.Http.Headers;
using System.Text.Json;
using APIComposer.GraphQL.Services.Interfaces;
using Library.Common;
using Library.DTOs.UserRelated.Subscription;
using Library.DTOs.UserRelated.User;
using UserAPI.Models.Filters;
using UserAPI.Models.Sorts;

namespace APIComposer.GraphQL.Services
{
    public class UserServiceClient(HttpClient http, IHttpContextAccessor httpContextAccessor) : IUserServiceClient
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

        public async Task<PaginatedResult<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize, string? searchTerm,
            UserFilter? filter, UserSort? sort)
        {
            SetAuthHeader();
            var query = QueryBuilder.BuildQuery(
                new { pageNumber, pageSize, searchTerm },
                filter,
                sort
            );
            var response = await _http.GetAsync($"users?{query}");

            if (!response.IsSuccessStatusCode)
                await ErrorHandler.HandleErrorResponseAsync(response);

            var data = await response.Content.ReadFromJsonAsync<PaginatedResult<UserDto>>();
            return data!;
        }

        public async Task<UserWithSubscriptionsDto> GetUserAsync(Guid id)
        {
            SetAuthHeader();
            var response = await _http.GetAsync($"users/{id}");

            if (!response.IsSuccessStatusCode)
                await ErrorHandler.HandleErrorResponseAsync(response);

            return (await response.Content.ReadFromJsonAsync<UserWithSubscriptionsDto>())!;
        }

        public async Task<SubscriptionDto?> GetUserSubscriptionAsync(Guid userId)
        {
            SetAuthHeader();
            var response = await _http.GetAsync($"subscriptions/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                await ErrorHandler.HandleErrorResponseAsync(response);
                return null;
            }


            return (await response.Content.ReadFromJsonAsync<SubscriptionDto>())!;
        }
    }
}