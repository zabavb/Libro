using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using OrderApi.Data;
using OrderApi.Models;
using OrderApi.Repository;
using OrderAPI.Repository.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderAPI.Tests.Repositories
{
    public class DeliveryTypeRepositoryTests
    {
        private readonly OrderDbContext _dbContext;
        private readonly Mock<OrderDbContext> _contextMock;
        private readonly Mock<IConnectionMultiplexer> _redisMock;
        private readonly Mock<IDatabase> _redisDatabaseMock;
        private readonly Mock<ILogger<IDeliveryTypeRepository>> _loggerMock;
        private readonly DeliveryTypeRepository _repository;

        public DeliveryTypeRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new OrderDbContext(options);

            _contextMock = new Mock<OrderDbContext>();

            _redisMock = new Mock<IConnectionMultiplexer>();
            _redisDatabaseMock = new Mock<IDatabase>();
            _loggerMock = new Mock<ILogger<IDeliveryTypeRepository>>();

            _redisMock
                .Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_redisDatabaseMock.Object);

            _repository = new DeliveryTypeRepository(_dbContext, _redisMock.Object, _loggerMock.Object);
        }

        //================== Get all ==============

        [Fact]
        public async Task GetAllAsync_ReturnsDeliveryTypesFromCache_WhenCacheIsAvailable()
        {
            // Arrange
            Guid id1 = Guid.NewGuid(), id2 = Guid.NewGuid();

            var cachedDeliveryTypes = new[]
            {
                new HashEntry(id1.ToString(),JsonSerializer.Serialize(new DeliveryType { DeliveryId = id1})),
                new HashEntry(id1.ToString(),JsonSerializer.Serialize(new DeliveryType { DeliveryId = id2}))
            };

            _redisDatabaseMock
                .Setup(db => db.HashGetAllAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(cachedDeliveryTypes);

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
            var result = await _repository.GetAllPaginatedAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(id1, result.Items.First().DeliveryId);
            Assert.Equal(id2, result.Items.Last().DeliveryId);

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
        public async Task GetAllAsync_ReturnsDeliveryTypesFromDb_WhenCacheIsEmpty()
        {
            // Arrange
            Guid id1 = Guid.NewGuid(), id2 = Guid.NewGuid();

            var deliveryTypesFromDb = new List<DeliveryType> 
            {
                new() {DeliveryId = id2},
                new() {DeliveryId = id1}
            };

            // Removing the seeded data for ease of testing 
            foreach (var entity in _dbContext.DeliveryTypes)
                _dbContext.DeliveryTypes.Remove(entity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.DeliveryTypes.AddRangeAsync(deliveryTypesFromDb);
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
            var result = await _repository.GetAllPaginatedAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            Assert.Equal(id1, result.Items.First().DeliveryId);
            Assert.Equal(id2, result.Items.Last().DeliveryId);
            capturedLogMessage.Should().Contain("Set to CACHE.");

            _loggerMock.Verify(l => l.Log(
            It.Is<LogLevel>(level => level == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Exactly(2));
        }

        //================== Get By Id ==============
        [Fact]
        public async Task GetByIdAsync_ReturnsDeliveryTypeFromCache_WhenCacheIsAvailable()
        {
            // Arrange
            Guid deliveryTypeId = Guid.NewGuid();
            string cacheKey = $"{_repository._cacheKeyPrefix}{deliveryTypeId.ToString()}";
            string fieldKey = deliveryTypeId.ToString();

            var deliveryType = new DeliveryType { DeliveryId = deliveryTypeId };
            var cachedDeliveryType = JsonSerializer.Serialize(deliveryType);

            _redisDatabaseMock
                .Setup(db => db.HashGetAsync(cacheKey, fieldKey, It.IsAny<CommandFlags>()))
                .ReturnsAsync(cachedDeliveryType);

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
            var result = await _repository.GetByIdAsync(deliveryTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(deliveryTypeId, result.DeliveryId);

            capturedLogMessage.Should().Contain("Fetched from CACHE.");

            _redisDatabaseMock.Verify(db => db.HashSetAsync(
            cacheKey,
            fieldKey,
                It.Is<RedisValue>(value => value.ToString().Contains(deliveryTypeId.ToString())),
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
        public async Task GetByIdAsync_ReturnsDeliveryTypeFromDbAndCachesIt_WhenCacheIsEmpty()
        {
            // Arrange
            Guid deliveryTypeId = Guid.NewGuid();
            string cacheKey = $"{_repository._cacheKeyPrefix}{deliveryTypeId.ToString()}";
            string fieldKey = deliveryTypeId.ToString();
            
            var deliveryType = new DeliveryType { DeliveryId = deliveryTypeId };
            _redisDatabaseMock
                .Setup(db => db.HashGetAsync(cacheKey, fieldKey, It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Null);

            _dbContext.DeliveryTypes.Add(deliveryType);
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
            var result = await _repository.GetByIdAsync(deliveryTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(deliveryTypeId, result.DeliveryId);

            capturedLogMessage.Should().Contain("Fetched from DB.");
            capturedLogMessage.Should().Contain("Set to CACHE.");

            _redisDatabaseMock.Verify(db => db.HashSetAsync(
                cacheKey,
                fieldKey,
                It.Is<RedisValue>(value => value.ToString().Contains(deliveryTypeId.ToString())),
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

        //================== Create ==============

        [Fact]
        public async Task CreateAsync_AddsDeliveryTypeToDatabase()
        {
            // Arrange
            var newDeliveryType = new DeliveryType { DeliveryId = Guid.NewGuid() };

            // Act
            await _repository.CreateAsync(newDeliveryType);

            // Assert
            var result = await _dbContext.DeliveryTypes.FindAsync(newDeliveryType.DeliveryId);
            Assert.NotNull(result);
            Assert.Equal(newDeliveryType.DeliveryId, result.DeliveryId);
        }

        //================== Update ==============

        [Fact]
        public async Task UpdateAsync_UpdatesDeliveryTypeInDatabase_WhenDeliveryTypeExists()
        {
            // Arrange
            var existingDeliveryType = new DeliveryType
            {
                DeliveryId = Guid.NewGuid(),
                ServiceName = "Some mail"
            };

            await _dbContext.DeliveryTypes.AddAsync(existingDeliveryType);
            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(existingDeliveryType).State = EntityState.Detached;

            var updatedDeliveryType = new DeliveryType
            {
                DeliveryId = existingDeliveryType.DeliveryId,
                ServiceName = "Some post"
            };
            // Act
            await _repository.UpdateAsync(updatedDeliveryType);

            // Assert
            var deliveryTypeInDb = await _dbContext.DeliveryTypes.FindAsync(existingDeliveryType.DeliveryId);
            Assert.NotNull(deliveryTypeInDb);
            Assert.Equal(updatedDeliveryType.ServiceName, deliveryTypeInDb.ServiceName);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsInvalidOperationException_WhenDeliveryTypeDoesNotExist()
        {
            // Arrange
            var deliveryType = new DeliveryType { DeliveryId = Guid.NewGuid() };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(deliveryType));
        }

        //================== Delete ==============

        [Fact]
        public async Task DeleteAsyncRemoveDeliveryTypeFromDatabase_WhenDeliveryTypeExists()
        {
            // Arrange
            var deliveryTypeId = Guid.NewGuid();
            var deliveryType = new DeliveryType { DeliveryId = deliveryTypeId };

            _dbContext.DeliveryTypes.Add(deliveryType);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(deliveryTypeId);

            // Assert
            var deliveryTypeInDb = await _dbContext.DeliveryTypes.FindAsync(deliveryTypeId);
            Assert.Null(deliveryTypeInDb);
        }
    }
}
