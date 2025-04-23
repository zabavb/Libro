using System.Text.Json;
using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL.Services
{
    public static class ErrorHandler
    {
        public static async Task HandleErrorResponseAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var errorJson = JsonDocument.Parse(content);
                var message = errorJson.RootElement.GetProperty("Message").GetString() ?? "Unknown error";

                throw new GraphQLException(ErrorBuilder.New()
                    .SetMessage(message)
                    .SetCode("USER_SERVICE_ERROR")
                    .SetExtension("status", (int)response.StatusCode)
                    .Build());
            }
            catch (JsonException)
            {
                throw new GraphQLException(ErrorBuilder.New()
                    .SetMessage($"Error: {(int)response.StatusCode} {response.ReasonPhrase}")
                    .SetCode("USER_SERVICE_ERROR")
                    .SetExtension("status", (int)response.StatusCode)
                    .SetExtension("details", content)
                    .Build());
            }
        }
    }
}