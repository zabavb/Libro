using AutoMapper;
using FluentAssertions;
using Library.DTOs.User;
using Library.Extensions;
using Library.Filters;
using Library.Sortings;
using Microsoft.Extensions.Logging;
using Moq;
using UserAPI.Repositories;
using UserAPI.Services;

namespace UserAPI.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        //========================= Get all =========================

        [Fact]
        public async Task GetAllAsync_ReturnsPaginatedResult_WhenUsersExist()
        {
            // Arrange
            var user = new Models.User { UserId = Guid.NewGuid() };
            var userDto = new User { Id = user.UserId };
            var paginatedUsers = new PaginatedResult<Models.User>
            {
                Items = [user],
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>()))
                .ReturnsAsync(paginatedUsers);

            _mapperMock
                .Setup(m => m.Map<ICollection<User>>(It.IsAny<ICollection<Models.User>>()))
                .Returns([userDto]);

            string capturedLogMessage = null!;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            var result = await _userService.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>());

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
            capturedLogMessage.Should().Contain("Successfully fetched [1] users.");


            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsInvalidOperationException_WhenErrorOccurs()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>()))
                .ReturnsAsync((PaginatedResult<Models.User>)null!);

            string capturedLogMessage = null!;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            Func<Task> act = async () => await _userService.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserFilter>(), It.IsAny<UserSort>());

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Failed to fetch paginated users.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("Failed to fetch paginated users.");

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        //========================= Get By Id =========================

        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new Models.User
            {
                UserId = userId,
                FirstName = "Test User Name",
                LastName = "Test User Surname",
                Email = "testuseremail@gmail.com",
                PhoneNumber = "000000000",
                DateOfBirth = DateTime.Now,
                Role = RoleType.USER
            };

            var userDto = new User
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Role = user.Role
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(m => m.Map<User>(It.IsAny<Models.User>()))
                .Returns(userDto);

            string capturedLogMessage = null!;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(userId);
            capturedLogMessage.Should().Contain($"User with ID [{userId}] successfully fetched.");

            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsKeyNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _repositoryMock
                .Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((Models.User?)null);

            string capturedLogMessage = null!;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            Func<Task> act = async () => await _userService.GetByIdAsync(userId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"User with ID [{userId}] not found.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"User with ID [{userId}] not found.");

            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        //========================= Create =========================

        [Fact]
        public async Task CreateAsync_LogInformation_WhenUserIsCreated()
        {
            // Arrange
            var userDto = new User
            {
                Id = Guid.Empty,
                FirstName = "Test User Name",
                LastName = "Test User Surname",
                Email = "testuseremail@gmail.com",
                PhoneNumber = "000000000",
                DateOfBirth = DateTime.Now,
                Role = RoleType.USER
            };

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Models.User>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                    .Setup(m => m.Map<Models.User>(It.IsAny<User>()))
                    .Returns(new Models.User());

            string capturedLogMessage = null!;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            await _userService.CreateAsync(userDto);

            // Assert
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("User successfully created.");

            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Models.User>()), Times.Once);
            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsArgumentNullException_WhenUserIsNull()
        {
            // Arrange
            User? userDto = null;

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Models.User>()))
                .ThrowsAsync(new ArgumentNullException("User was not provided for creation."));

            string capturedLogMessage = null!;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            Func<Task> act = async () => await _userService.CreateAsync(userDto!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("User was not provided for creation.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("User was not provided for creation.");

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsInvalidOperationException_WhenErrorOccurs()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new User { Id = userId };
            var user = new Models.User { UserId = userId };

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Models.User>()))
                .ThrowsAsync(new Exception($"Error occurred while adding the user with ID [{userId}]."));

            string? capturedLogMessage = null;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            Func<Task> act = async () => await _userService.CreateAsync(userDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Error occurred while adding the user with ID [{userId}].");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Error occurred while adding the user with ID [{userId}].");

            _repositoryMock.Verify(r => r.CreateAsync(user), Times.Never);
            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        //========================= Update =========================

        [Fact]
        public async Task UpdateAsync_LogInformation_WhenUserUpdated()
        {
            // Arrange
            var userDto = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test User Name",
                LastName = "Test User Surname",
                Email = "testuseremail@gmail.com",
                PhoneNumber = "000000000",
                DateOfBirth = DateTime.Now,
                Role = RoleType.USER
            };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Models.User>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<User>(It.IsAny<User>()))
                .Returns(new User());

            string capturedLogMessage = null!;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            await _userService.UpdateAsync(userDto);

            // Assert
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"User with ID [{userDto.Id}] successfully updated.");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Models.User>()), Times.Once);
            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsArgumentNullException_WhenUserIsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.UpdateAsync(null!))
                .ThrowsAsync(new ArgumentNullException("User was not provided for update."));

            string capturedLogMessage = null!;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            Func<Task> act = async () => await _userService.UpdateAsync(null!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("User was not provided for update.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("User was not provided for update.");

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsKeyNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new User { Id = userId };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Models.User>()))
                           .ThrowsAsync(new InvalidOperationException($"User with ID [{userDto.Id}] not found for update."));

            string capturedLogMessage = null!;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            Func<Task> act = async () => await _userService.UpdateAsync(userDto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"User with ID [{userDto.Id}] not found for update.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"User with ID [{userDto.Id}] not found for update.");

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsInvalidOperationException_WhenErrorOccurs()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new User { Id = userId };
            var user = new Models.User { UserId = userId };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Models.User>()))
                .ThrowsAsync(new Exception($"Error occurred while updating the user with ID [{userDto.Id}]."));

            string? capturedLogMessage = null;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            Func<Task> act = async () => await _userService.UpdateAsync(userDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Error occurred while updating the user with ID [{userId}].");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Error occurred while updating the user with ID [{userDto.Id}].");

            _repositoryMock.Verify(r => r.UpdateAsync(user), Times.Never);
            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        //========================= Delete =========================

        [Fact]
        public async Task DeleteAsync_LogInformation_WhenUserIsDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(userId))
                .Returns(Task.CompletedTask);

            string? capturedLogMessage = null;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            await _userService.DeleteAsync(userId);

            // Assert
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"User with ID [{userId}] successfully deleted.");

            _repositoryMock.Verify(r => r.DeleteAsync(userId), Times.Once);
            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsKeyNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(userId))
                .ThrowsAsync(new KeyNotFoundException($"User with ID [{userId}] not found for deletion."));

            string? capturedLogMessage = null;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            Func<Task> act = async () => await _userService.DeleteAsync(userId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"User with ID [{userId}] not found for deletion.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"User with ID [{userId}] not found for deletion.");

            _repositoryMock.Verify(r => r.DeleteAsync(userId), Times.Once);
            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsInvalidOperationException_WhenErrorOccurs()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(userId))
                .ThrowsAsync(new InvalidOperationException($"Error occurred while deleting the user with ID [{userId}]."));

            string? capturedLogMessage = null;
            _loggerMock
                .Setup(l => l.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ))
                .Callback<LogLevel, EventId, object, Exception, Delegate>((level, eventId, state, exception, formatter) =>
                {
                    capturedLogMessage = state.ToString()!;
                });

            // Act
            Func<Task> act = async () => await _userService.DeleteAsync(userId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Error occurred while deleting the user with ID [{userId}].");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Error occurred while deleting the user with ID [{userId}].");

            _repositoryMock.Verify(r => r.DeleteAsync(userId), Times.Once);
            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }
    }
}
