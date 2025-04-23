using APIComposer.GraphQL.Services.Interfaces;
using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services
{
    public class BookServiceClient(HttpClient http) : IBookServiceClient
    {
        private readonly HttpClient _http = http;

        public async Task<ICollection<string>> GetAllBookNamesAsync(ICollection<Guid> ids)
        {
            try
            {
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
                var response =
                    await _http.GetFromJsonAsync<ICollection<FeedbackForUserDetails>>(
                        "feedbacks/for-user/details/{id}");
                return response ?? new List<FeedbackForUserDetails>();
            }
            catch (Exception)
            {
                return new List<FeedbackForUserDetails>();
            }
        }
    }
}