using System.Net.Http.Headers;
using APIComposer.GraphQL.Services.Interfaces;
using Library.DTOs.UserRelated.User;
using UserAPI.Models;

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

        public async Task<Library.DTOs.Book.Book> GetBookAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var response = await _http.GetFromJsonAsync<Library.DTOs.Book.Book>($"books/{id}");
                return response ?? new Library.DTOs.Book.Book();
            }
            catch (Exception ex)
            {
                return null;
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

        public async Task<Library.DTOs.Book.Author> GetAuthorAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var response =
                    await _http.GetFromJsonAsync<Library.DTOs.Book.Author>(
                        $"authors/{id}");
                return response ?? new Library.DTOs.Book.Author();
            }
            catch (Exception)
            {
                return new Library.DTOs.Book.Author();
            }
        }
    }
}