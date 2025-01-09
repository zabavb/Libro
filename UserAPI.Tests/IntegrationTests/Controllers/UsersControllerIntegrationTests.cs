using System.Net;
using System.Text;
using System.Text.Json;
using Library.DTOs.User;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UserAPI.Tests.IntegrationTests.Controllers
{
    public class UsersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        // private readonly string _baseAddress = "https://localhost:7007/gateway/users";

        public UsersControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:7007/gateway/users");
        }

        [Fact]
        public async Task GetAll_ReturnsUsers_WithStatus200()
        {
            // Act
            var response = await _client.GetAsync("?pageNumber=1&pageSize=10");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseData = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(users);
        }

        [Fact]
        public async Task GetById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = "some-existing-user-id";

            // Act
            var response = await _client.GetAsync($"api/Users/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id.ToString());
        }

        [Fact]
        public async Task GetById_Returns404_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"api/Users/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_Returns201_WhenUserIsCreated()
        {
            // Arrange
            var newUser = new User { Id = Guid.NewGuid() };

            var content = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/Users", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<User>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(createdUser);
            Assert.Equal(newUser.Id, createdUser.Id);
        }

        [Fact]
        public async Task Update_Returns204_WhenUserIsUpdated()
        {
            // Arrange
            var existingUserId = "some-existing-user-id";
            var updatedUser = new User { Id = Guid.Parse(existingUserId) };

            var content = new StringContent(JsonSerializer.Serialize(updatedUser), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"api/Users/{existingUserId}", content);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Returns204_WhenUserIsDeleted()
        {
            // Arrange
            var existingUserId = "some-existing-user-id";

            // Act
            var response = await _client.DeleteAsync($"api/Users/{existingUserId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Returns404_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentUserId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"api/Users/{nonExistentUserId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

}
