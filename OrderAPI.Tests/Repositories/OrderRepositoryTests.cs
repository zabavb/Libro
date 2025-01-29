using FluentAssertions;
using Library.DTOs.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OrderApi.Data;
using OrderApi.Repository;
using StackExchange.Redis;
using System.Text.Json;
using Order = OrderApi.Models.Order;
using DeliveryType = OrderApi.Models.DeliveryType;
namespace OrderAPI.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly OrderDbContext _dbContext;
        private readonly Mock<OrderDbContext> _contextMock;
        private readonly Mock<IConnectionMultiplexer> _redisMock;
        private readonly Mock<IDatabase> _redisDatabaseMock;
        private readonly Mock<ILogger<IOrderRepository>> _loggerMock;
        private readonly OrderRepository _repository;



        public OrderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new OrderDbContext(options);

            _contextMock = new Mock<OrderDbContext>();

            _redisMock = new Mock<IConnectionMultiplexer>();
            _redisDatabaseMock = new Mock<IDatabase>();
            _loggerMock = new Mock<ILogger<IOrderRepository>>();

            _redisMock.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_redisDatabaseMock.Object);

            _repository = new OrderRepository(_dbContext, _redisMock.Object, _loggerMock.Object);
        }

        //================== Get all ==============

        [Fact]
        public async Task GetAllAsync_ReturnsOrdersFromCache_WhenCacheIsAvailable()
        {
            // Arrange
            Guid id1 = Guid.NewGuid(), id2 = Guid.NewGuid(), deliveryId = Guid.NewGuid();

            await _dbContext.DeliveryTypes.AddAsync(new DeliveryType() { DeliveryId = deliveryId });

            var cachedOrders = new[]
            {
                new HashEntry(id1.ToString(), JsonSerializer.Serialize(new Order { OrderId = id1,DeliveryTypeId = deliveryId})),
                new HashEntry(id2.ToString(), JsonSerializer.Serialize(new Order { OrderId = id2,DeliveryTypeId = deliveryId}))
            };

            _redisDatabaseMock
                .Setup(db => db.HashGetAllAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(cachedOrders);

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
            var result = await _repository.GetAllPaginatedAsync(1, 10, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(id1, result.Items.First().OrderId);
            Assert.Equal(id2, result.Items.Last().OrderId);

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
        public async Task GetAllAsync_ReturnsOrdersFromDb_WhenCacheIsEmpty()
        {
            // Arrange
            Guid id1 = Guid.NewGuid(), id2 = Guid.NewGuid(), id3 = Guid.NewGuid();

            var deliveryTypeFromDb = new DeliveryType(){ DeliveryId = id3};

            var ordersFromDb = new List<Order>
            {
                new() {OrderId = id1, DeliveryTypeId = id3},
                new() {OrderId = id2, DeliveryTypeId = id3},
            };

            await _dbContext.DeliveryTypes.AddAsync(deliveryTypeFromDb);
            await _dbContext.Orders.AddRangeAsync(ordersFromDb);
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
            // Page 4, because our newly added orders are there
            var result = await _repository.GetAllPaginatedAsync(4, 10, null, null);

            // Assert
            Assert.NotNull(result);
            result.Items.Should().HaveCount(2); 
            result.TotalCount.Should().Be(32); // 32 Because of initial seeding
            Assert.Equal(id1, result.Items.First().OrderId);
            Assert.Equal(id2, result.Items.Last().OrderId);
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
        public async Task GetByIdAsync_ReturnsOrderFromCache_WhenCacheIsAvailable()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            string cacheKey = $"{_repository._cacheKeyPrefix}{orderId.ToString()}";
            string fieldKey = orderId.ToString();

            var order = new Order { OrderId = orderId };
            var cachedOrder = JsonSerializer.Serialize(order);

            _redisDatabaseMock
                .Setup(db => db.HashGetAsync(cacheKey, fieldKey, It.IsAny<CommandFlags>()))
                .ReturnsAsync(cachedOrder);

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
            var result = await _repository.GetByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);

            capturedLogMessage.Should().Contain("Fetched from CACHE.");

            _redisDatabaseMock.Verify(db => db.HashSetAsync(
                cacheKey,
                fieldKey,
                It.Is<RedisValue>(value => value.ToString().Contains(orderId.ToString())),
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
        public async Task GetByIdAsync_ReturnsOrderFromDbAndCachesIt_WhenCacheIsEmpty()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            string cacheKey = $"{_repository._cacheKeyPrefix}{orderId}";
            string fieldKey = orderId.ToString();

            var order = new Order { OrderId = orderId };
            _redisDatabaseMock
                .Setup(db => db.HashGetAsync(cacheKey, fieldKey, It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Null);

            _dbContext.Orders.Add(order);
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
            var result = await _repository.GetByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);

            capturedLogMessage.Should().Contain("Fetched from DB.");
            capturedLogMessage.Should().Contain("Set to CACHE.");

            _redisDatabaseMock.Verify(db => db.HashSetAsync(
            cacheKey,
            fieldKey,
            It.Is<RedisValue>(value => value.ToString().Contains(orderId.ToString())),
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
        public async Task CreateAsync_AddsOrderToDatabase()
        {
            // Arrange
            var newOrder = new Order { OrderId = Guid.NewGuid() };

            // Act
            await _repository.CreateAsync(newOrder);

            // Assert
            var result = await _dbContext.Orders.FindAsync(newOrder.OrderId);
            Assert.NotNull(result);
            Assert.Equal(newOrder.OrderId, result.OrderId);
        }

        //================== Update ==============

        [Fact]
        public async Task UpdateAsync_UpdatesOrderInDatabase_WhenOrderExists()
        {
            // Arrange
            var existingOrder = new Order
            {
                OrderId = Guid.NewGuid(),
                Status = OrderStatus.PROCESSING
            };

            await _dbContext.Orders.AddAsync(existingOrder);
            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(existingOrder).State = EntityState.Detached;

            var updatedOrder = new Order()
            {
                OrderId = existingOrder.OrderId,
                Status = OrderStatus.COMPLETED
            };

            // Act
            await _repository.UpdateAsync(updatedOrder);

            // Assert
            var orderInDb = await _dbContext.Orders.FindAsync(existingOrder.OrderId);
            Assert.NotNull(orderInDb);
            Assert.Equal(updatedOrder.Status, orderInDb.Status);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsInvalidOperationException_WhenOrderDoesNotExist()
        {
            // Arrange
            var Order = new Order { OrderId = Guid.NewGuid() };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(Order));
        }

        //================== Delete ==============
        [Fact]
        public async Task DeleteAsync_RemovesOrderFromDatabase_WhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { OrderId = orderId };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(orderId);

            // Assert
            var orderInDb = await _dbContext.Orders.FindAsync(orderId);
            Assert.Null(orderInDb);
        }
    }
}
