using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using UserAPI.Data;
using UserAPI.Repositories;
using System.Text.Json;
using UserAPI.Models;
using FluentAssertions;

namespace UserAPI.Tests.UnitTests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly UserDbContext _dbContext;
        private readonly Mock<UserDbContext> _contextMock;
        private readonly Mock<IConnectionMultiplexer> _redisMock;
        private readonly Mock<IDatabase> _redisDatabaseMock;
        private readonly Mock<ILogger<IUserRepository>> _loggerMock;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new UserDbContext(options);

            _contextMock = new Mock<UserDbContext>();
            _redisMock = new Mock<IConnectionMultiplexer>();
            _redisDatabaseMock = new Mock<IDatabase>();
            _loggerMock = new Mock<ILogger<IUserRepository>>();

            _redisMock.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_redisDatabaseMock.Object);

            _repository = new UserRepository(_dbContext, _redisMock.Object, _loggerMock.Object);
        }

        //========================= Get all =========================

        [Fact]
        public async Task GetAllAsync_ReturnsUsersFromCache_WhenCacheIsAvailable()
        {
            // Arrange
            Guid id1 = Guid.NewGuid(), id2 = Guid.NewGuid();
            var cachedUsers = new[]
            {
                new HashEntry(id1.ToString(), JsonSerializer.Serialize(new User { UserId = id1 })),
                new HashEntry(id2.ToString(), JsonSerializer.Serialize(new User { UserId = id2 }))
            };

            _redisDatabaseMock
                .Setup(db => db.HashGetAllAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(cachedUsers);

            string? capturedLogMessage = string.Empty;
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
                    capturedLogMessage = state.ToString();
                });

            // Act
            var result = await _repository.GetAllAsync(1, 10, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(id1, result.Items.First().UserId);
            Assert.Equal(id2, result.Items.Last().UserId);

            capturedLogMessage.Should().Contain("Fetched from CACHE.");
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsUsersFromDb_WhenCacheIsEmpty()
        {
            // Arrange
            Guid id1 = Guid.NewGuid(), id2 = Guid.NewGuid();

            var usersFromDb = new List<User>
            {
                new() { UserId = id1 },
                new() { UserId = id2 }
            };
            await _dbContext.Users.AddRangeAsync(usersFromDb);
            await _dbContext.SaveChangesAsync();

            _redisDatabaseMock
                .Setup(db => db.HashGetAllAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync([]);

            string? capturedLogMessage = string.Empty;
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
                    capturedLogMessage = state.ToString();
                });

            // Act
            var result = await _repository.GetAllAsync(1, 10, null, null, null);

            // Assert
            Assert.NotNull(result);
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            Assert.Equal(id1, result.Items.First().UserId);
            Assert.Equal(id2, result.Items.Last().UserId);
            capturedLogMessage.Should().Contain("Set to CACHE.");

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Exactly(2));
        }

        //========================= Get By Id =========================

        [Fact]
        public async Task GetByIdAsync_ReturnsUserFromCache_WhenCacheIsAvailable()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            string cacheKey = $"{_repository._cacheKeyPrefix}{userId}";
            string fieldKey = userId.ToString();

            var user = new User { UserId = userId };
            var cachedUser = JsonSerializer.Serialize(user);

            _redisDatabaseMock
                .Setup(db => db.HashGetAsync(cacheKey, fieldKey, It.IsAny<CommandFlags>()))
                .ReturnsAsync(cachedUser);

            _redisDatabaseMock
                .Setup(db => db.KeyExpireAsync(cacheKey, It.IsAny<TimeSpan>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            string? capturedLogMessage = string.Empty;
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
                    capturedLogMessage = state.ToString();
                });

            // Act
            var result = await _repository.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);

            capturedLogMessage.Should().Contain("Fetched from CACHE.");

            _redisDatabaseMock.Verify(db => db.HashSetAsync(
                cacheKey,
                fieldKey,
                It.Is<RedisValue>(value => value.ToString().Contains(userId.ToString())),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()),
                Times.Never);

            _redisDatabaseMock.Verify(db => db.KeyExpireAsync(
                cacheKey,
                It.IsAny<TimeSpan>(),
                It.IsAny<CommandFlags>()),
                Times.Never);

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }


        [Fact]
        public async Task GetByIdAsync_ReturnsUserFromDBAndCachesIt_WhenCacheIsEmpty()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            string cacheKey = $"{_repository._cacheKeyPrefix}{userId}";
            string fieldKey = userId.ToString();

            var user = new User { UserId = userId };
            _redisDatabaseMock
                .Setup(db => db.HashGetAsync(cacheKey, fieldKey, It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Null);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            _redisDatabaseMock
                .Setup(db => db.KeyExpireAsync(cacheKey, It.IsAny<TimeSpan>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            string? capturedLogMessage = string.Empty;
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
                    capturedLogMessage += state.ToString() + " ";
                });

            // Act
            var result = await _repository.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);

            capturedLogMessage.Should().Contain("Fetched from DB.");
            capturedLogMessage.Should().Contain("Set to CACHE.");

            _redisDatabaseMock.Verify(db => db.HashSetAsync(
                cacheKey,
                fieldKey,
                It.Is<RedisValue>(value => value.ToString().Contains(userId.ToString())),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()),
                Times.Once);

            _redisDatabaseMock.Verify(db => db.KeyExpireAsync(
                cacheKey,
                It.Is<TimeSpan>(ts => ts == _repository._cacheExpiration),
                It.IsAny<CommandFlags>()),
                Times.Never);

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Exactly(2));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenUserIsNotFoundInDatabase()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            string cacheKey = $"{_repository._cacheKeyPrefix}{userId}";
            string fieldKey = userId.ToString();

            _redisDatabaseMock
                .Setup(db => db.HashGetAsync(cacheKey, fieldKey, It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Null);

            string? capturedLogMessage = string.Empty;
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
                    capturedLogMessage += state.ToString() + " ";
                });

            // Act
            var result = await _repository.GetByIdAsync(userId);

            // Assert
            Assert.Null(result);

            capturedLogMessage.Should().Contain("Fetched from DB.");

            _redisDatabaseMock.Verify(db => db.HashSetAsync(
                cacheKey,
                fieldKey,
                It.IsAny<RedisValue>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()),
                Times.Never);

            _redisDatabaseMock.Verify(db => db.KeyExpireAsync(
                It.Is<RedisKey>(key => key == cacheKey),
                It.IsAny<TimeSpan>(),
                It.IsAny<CommandFlags>()),
                Times.Never);

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        //========================= Create =========================        

        [Fact]
        public async Task CreateAsync_AddsUserToDatabase()
        {
            // Arrange
            var newUser = new User { UserId = Guid.NewGuid() };

            // Act
            await _repository.CreateAsync(newUser);

            // Assert
            var result = await _dbContext.Users.FindAsync(newUser.UserId);
            Assert.NotNull(result);
            Assert.Equal(newUser.UserId, result.UserId);
        }


        //========================= Update =========================

        [Fact]
        public async Task UpdateAsync_UpdatesUserInDatabase_WhenUserExists()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "Jane Doe"
            };

            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(existingUser).State = EntityState.Detached;

            var updatedUser = new User
            {
                UserId = existingUser.UserId,
                FirstName = "Jane Smith"
            };

            // Act
            await _repository.UpdateAsync(updatedUser);

            // Assert
            var userInDb = await _dbContext.Users.FindAsync(existingUser.UserId);
            Assert.NotNull(userInDb);
            Assert.Equal(updatedUser.FirstName, userInDb.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsInvalidOperationException_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentUser = new User { UserId = Guid.NewGuid() };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(nonExistentUser));
        }

        //========================= Delete =========================

        [Fact]
        public async Task DeleteAsync_RemovesUserFromDatabase_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(userId);

            // Assert
            var userInDb = await _dbContext.Users.FindAsync(userId);
            Assert.Null(userInDb);
        }
    }
}
