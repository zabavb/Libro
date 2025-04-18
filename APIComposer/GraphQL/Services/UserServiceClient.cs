using System.Text.Json;
using APIComposer.GraphQL.Services.Interfaces;
using Library.Common;
using Library.DTOs.UserRelated.User;
using UserAPI.Models.Filters;
using UserAPI.Models.Sorts;

namespace APIComposer.GraphQL.Services
{
    public class UserServiceClient(HttpClient http) : IUserServiceClient
    {
        private readonly HttpClient _http = http;

        public async Task<PaginatedResult<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize, string? searchTerm,
            UserFilter? filter, UserSort? sort)
        {
            var response = await _http.GetAsync($"users?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
                await ErrorHandler.HandleErrorResponseAsync(response);

            var data = await response.Content.ReadFromJsonAsync<PaginatedResult<UserDto>>();
            return data!;
        }

        public async Task<UserWithSubscriptionsDto> GetUserAsync(Guid id)
        {
            var response = await _http.GetAsync($"users/{id}");

            if (!response.IsSuccessStatusCode)
                await ErrorHandler.HandleErrorResponseAsync(response);

            return (await response.Content.ReadFromJsonAsync<UserWithSubscriptionsDto>())!;
        }
    }
}