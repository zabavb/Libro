using System.Net.Http.Headers;
using APIComposer.GraphQL.Services.Interfaces;
using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services
{
    public class BookServiceClient(HttpClient http, IHttpContextAccessor httpContextAccessor) : IBookServiceClient
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

        public async Task<ICollection<string>> GetAllBookNamesAsync(ICollection<Guid> ids)
        {
            try
            {
                SetAuthHeader();
                var queryString = string.Join("&ids=", ids);
                var response =
                    await _http.GetFromJsonAsync<ICollection<string>>($"books/for-user/details?ids={queryString}");
                return response ?? new List<string>();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        public async Task<ICollection<FeedbackForUserDetails>> GetAllFeedbacksAsync(Guid userId)
        {
            try
            {
                SetAuthHeader();
                var response =
                    await _http.GetFromJsonAsync<ICollection<FeedbackForUserDetails>>(
                        $"feedbacks/for-user/details/{userId}");
                return response ?? new List<FeedbackForUserDetails>();
            }
            catch (Exception)
            {
                return new List<FeedbackForUserDetails>();
            }
        }
    }
}