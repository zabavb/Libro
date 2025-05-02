using Moq;
using Microsoft.Extensions.Logging;
using UserAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.Sorts;
using UserAPI.Models.Filters;
using UserAPI.Services.Interfaces;
using Library.Common;

namespace UserAPI.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<UsersController>> _loggerMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_userServiceMock.Object, _loggerMock.Object);
        }

        //========================= Get all =========================

        [Fact]
        public async Task GetAll_ReturnsOk_WhenUsersFetched()
        {
            // Arrange
            _userServiceMock
                .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>()))
                .ReturnsAsync(new PaginatedResult<User>());

            // Act
            var result = await _controller.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>());

            // Assert
            var statusResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<PaginatedResult<User>>(statusResult.Value);
            _userServiceMock.Verify(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _userServiceMock
                .Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>()))
                .ThrowsAsync(new Exception("Failed to fetch paginated users."));

            // Act
            var result = await _controller.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>());

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("Failed to fetch paginated users.", statusResult.Value);
            _userServiceMock.Verify(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>()), Times.Once);
        }

        //========================= Get by ID =========================

        [Fact]
        public async Task GetById_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };

            _userServiceMock
                .Setup(s => s.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            var statusResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<User>(statusResult.Value);
            Assert.Equal(userId, returnedUser.Id);
            _userServiceMock.Verify(s => s.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userServiceMock
                .Setup(s => s.GetByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException($"User with ID [{id}] not found."));

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var statusResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID [{id}] not found.", statusResult.Value);
            _userServiceMock.Verify(s => s.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenUserIdWasNotProvided()
        {
            // Arrange
            var id = Guid.NewGuid();

            _userServiceMock
                .Setup(s => s.GetByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException($"User ID [{id}] was not provided."));

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var statusResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User ID [{id}] was not provided.", statusResult.Value);
            _userServiceMock.Verify(s => s.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userServiceMock
                .Setup(s => s.GetByIdAsync(userId))
                .ThrowsAsync(new Exception("Something went wrong."));

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("Something went wrong.", statusResult.Value);
            _userServiceMock.Verify(s => s.GetByIdAsync(userId), Times.Once);
        }

        //========================= Create =========================

        [Fact]
        public async Task Create_ReturnsOk_WhenUserCreated()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid() };
            _userServiceMock
                .Setup(s => s.CreateAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(user);

            // Assert
            var statusResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(user.Id, ((User)statusResult.Value!).Id);
            Assert.Equal(nameof(_controller.GetById), statusResult.ActionName);
            _userServiceMock.Verify(s => s.CreateAsync(user), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenUserWasNotProvided()
        {
            // Arrange
            _userServiceMock
                .Setup(s => s.CreateAsync(null!))
                .ThrowsAsync(new ArgumentNullException("User was not provided for creation."));

            // Act
            var result = await _controller.Create(null!);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            _userServiceMock.Verify(s => s.CreateAsync(null!), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid() };
            _userServiceMock
                .Setup(s => s.CreateAsync(user))
                .ThrowsAsync(new Exception($"Error occurred while adding the user with ID [{user.Id}]."));

            // Act
            var result = await _controller.Create(user);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal($"Error occurred while adding the user with ID [{user.Id}].", statusResult.Value);
            _userServiceMock.Verify(s => s.CreateAsync(user), Times.Once);
        }

        //========================= Update =========================

        [Fact]
        public async Task Update_ReturnsNoContent_WhenUserUpdated()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid() };
            var changedUser = new User { Id = user.Id };
            _userServiceMock
                .Setup(s => s.UpdateAsync(changedUser))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(user.Id, changedUser);

            // Assert
            var statusResult = Assert.IsType<NoContentResult>(result);
            _userServiceMock.Verify(s => s.UpdateAsync(changedUser), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdsDoesNotMuch()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid() };
            await _controller.Create(user);
            var changedUser = new User { Id = Guid.NewGuid() };
            _userServiceMock.Setup(s => s.UpdateAsync(changedUser));

            // Act
            var result = await _controller.Update(user.Id, changedUser);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User ID in the URL does not match the ID in the body.", statusResult.Value);
            _userServiceMock.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenUserNotProvided()
        {
            // Arrange
            _userServiceMock
                .Setup(s => s.UpdateAsync(null!))
                .ThrowsAsync(new ArgumentNullException(null, "User was not provided for update."));

            // Act
            var result = await _controller.Update(Guid.Empty, null!);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User was not provided for update.", statusResult.Value);
            _userServiceMock.Verify(s => s.UpdateAsync(null!), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var user = new User() { Id = Guid.NewGuid() };
            _userServiceMock
                .Setup(s => s.UpdateAsync(user))
                .ThrowsAsync(new KeyNotFoundException($"User with ID [{user.Id}] not found for update."));

            // Act
            var result = await _controller.Update(user.Id, user);

            // Assert
            var statusResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID [{user.Id}] not found for update.", statusResult.Value);
            _userServiceMock.Verify(s => s.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var user = new User();
            _userServiceMock
                .Setup(s => s.UpdateAsync(user))
                .ThrowsAsync(new Exception($"Error occurred while updating the user with ID [{user.Id}]."));

            // Act
            var result = await _controller.Update(user.Id, user);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal($"Error occurred while updating the user with ID [{user.Id}].", statusResult.Value);
            _userServiceMock.Verify(s => s.UpdateAsync(user), Times.Once);
        }

        //========================= Delete =========================

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenUserDeleted()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), ImageUrl = ""};
            await _controller.Create(user);
            _userServiceMock
                .Setup(s => s.DeleteAsync(user.Id, user.ImageUrl))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(user.Id, user.ImageUrl);

            // Assert
            var statusResult = Assert.IsType<NoContentResult>(result);
            _userServiceMock.Verify(s => s.DeleteAsync(user.Id, user.ImageUrl), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), ImageUrl = "" };
            _userServiceMock
                .Setup(s => s.DeleteAsync(user.Id, user.ImageUrl))
                .ThrowsAsync(new KeyNotFoundException($"User with ID [{user.Id}] not found for deletion."));

            // Act
            var result = await _controller.Delete(user.Id, user.ImageUrl);

            // Assert
            var statusResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with ID [{user.Id}] not found for deletion.", statusResult.Value);
            _userServiceMock.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Delete_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), ImageUrl = "" };
            _userServiceMock
                .Setup(s => s.DeleteAsync(user.Id, user.ImageUrl))
                .ThrowsAsync(new Exception($"Error occurred while deleting the user with ID [{user.Id}]."));

            // Act
            var result = await _controller.Delete(user.Id, user.ImageUrl);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal($"Error occurred while deleting the user with ID [{user.Id}].", statusResult.Value);
            _userServiceMock.Verify(s => s.UpdateAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
