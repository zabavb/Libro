using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using APIComposer.GraphQL.Services.Interfaces;
using Library.DTOs.Book;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;
using Library.Common;
using OrderAPI;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;

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

        public async Task<BookDetails> GetBookAsync(Guid id)
        {
            try
            {
                SetAuthHeader();
                var response = await _http.GetAsync($"books/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var book = await JsonSerializer.DeserializeAsync<BookDetails>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    });
                    return book ?? new BookDetails();
                }
                return null;
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

        public async Task<PaginatedResult<FeedbackAdminCard>?> GetFeedbacksAsync(
           int pageNumber = 1,
           int pageSize = 10,
           string? searchTerm = null,
           FeedbackFilter? filter = null,
           FeedbackSort? sort = null)
        {
            try
            {
                SetAuthHeader();

                var query = QueryBuilder.BuildQuery(
                    new { pageNumber, pageSize, searchTerm },
                    filter,
                    sort
                );

                var response = await _http.GetAsync($"feedbacks?{query}");

                if (!response.IsSuccessStatusCode)
                {
                    await ErrorHandler.HandleErrorResponseAsync(response);
                    return null;
                }

                return (await response.Content.ReadFromJsonAsync<PaginatedResult<FeedbackAdminCard>>())!;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BookOrderDetails> GetBookWithAuthor(Guid bookId)
        {

            try
            {
                SetAuthHeader();
                var response =
                    await _http.GetFromJsonAsync<BookOrderDetails>(
                        $"books/for-order/details/{bookId}");
                return response ?? new BookOrderDetails();
            }
            catch (Exception)
            {
                return new BookOrderDetails();
            }

        }

        public async Task<ICollection<Feedback>> GetNumberOfFeedbacks(int amount, Guid bookId)
        {
            try
            {
                SetAuthHeader();
                var response =
                    await _http.GetFromJsonAsync<ICollection<Feedback>>(
                        $"feedbacks/for-book/{amount}?bookId={bookId}");
                return response ?? new List<Feedback>();
            }
            catch (Exception)
            {
                return new List<Feedback>();
            }
        }
    }
}