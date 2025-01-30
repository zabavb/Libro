using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OrderApi.Services;
using OrderDto = Library.DTOs.Order.Order;
using Order = OrderApi.Models.Order;
using DeliveryType = OrderApi.Models.DeliveryType;
using Library.Extensions;
using Library.Filters;
using FluentAssertions;
using Library.DTOs.Order;
using OrderApi.Models;
using StackExchange.Redis;

namespace OrderAPI.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<OrderService>> _loggerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        //================== Get all ==============

        [Fact]
        public async Task GetAllAsync_ReturnsPaginatedResult_WhenOrdersExist()
        {
            // Arrange
            var order = new Order { OrderId = Guid.NewGuid() };
            var orderDto = new OrderDto { Id = Guid.NewGuid() };
            var paginatedOrders = new PaginatedResult<Order>
            {
                Items = [order],
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            _repositoryMock
                .Setup(r => r.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<OrderFilter>()))
                .ReturnsAsync(paginatedOrders);

            _mapperMock
                .Setup(m => m.Map<ICollection<OrderDto>>(It.IsAny<ICollection<Order>>()))
                .Returns([orderDto]);

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
            var result = await _orderService.GetAllAsync(It.IsAny<int>(),It.IsAny<int>(), It.IsAny<string>(), It.IsAny<OrderFilter>());

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
            capturedLogMessage.Should().Contain("Successfully fetched [1] orders.");

            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsInvalidOperationExpection_WhenErrorOccurs()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<OrderFilter>()))
                .ReturnsAsync((PaginatedResult<Order>)null!);


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
            Func<Task> act = async () =>
            {
                await _orderService.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<OrderFilter>());
            };

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Failed to fetch paginated orders.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("Failed to fetch paginated orders.");

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
        public async Task GetByIdAsync_ReturnsOrder_whenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var deliveryTypeId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                UserId = Guid.NewGuid(),
                BookIds = new List<Guid>() { Guid.NewGuid() },
                DeliveryTypeId = deliveryTypeId,
                DeliveryDate = DateTime.UtcNow,
                DeliveryPrice = 159,
                OrderDate = DateTime.UtcNow,
                Region = "Region",
                City = "City",
                Address = "Address",
                Price = 162,
                Status = OrderStatus.PROCESSING
            };

            var orderDto = new OrderDto
            {
                Id = orderId,
                Address = order.Address,
                Region = order.Region,
                City = order.City,
                UserId = order.UserId,
                DeliveryTypeId = deliveryTypeId,
                DeliveryDate = order.DeliveryDate,
                DeliveryPrice = order.DeliveryPrice,
                OrderDate = order.DeliveryDate,
                Price = order.Price,
                Status = order.Status,
                BookIds = order.BookIds
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(order);

            _mapperMock
                .Setup(m => m.Map<OrderDto>(It.IsAny<Order>()))
                .Returns(orderDto);

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
            var result = await _orderService.GetByIdAsync(orderId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(orderId);
            capturedLogMessage.Should().Contain($"Order with ID [{orderId}] successfully fetched.");

            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsKeyNotFoundExceptiop_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _repositoryMock
                .Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync((Order?)null);

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
            Func<Task> act = async () => await _orderService.GetByIdAsync(orderId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Order with ID [{orderId}] not found.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Order with ID [{orderId}] not found.");

            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        //========================= Create =========================

        [Fact]
        public async Task CreateAsync_LogInformation_WhenOrderIsCreated()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                BookIds = new List<Guid>() { Guid.NewGuid() },
                DeliveryTypeId = Guid.Parse("23b0a723-0653-4371-a7b8-78b75beec78d"),
                DeliveryDate = DateTime.UtcNow,
                DeliveryPrice = 0,
                OrderDate = DateTime.UtcNow,
                Region = "string",
                City = "string",
                Address = "string",
                Price = 0,
                Status = OrderStatus.PENDING
            };

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<Order>(It.IsAny<OrderDto>()))
                .Returns((OrderDto dto) => new Order());

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
            await _orderService.CreateAsync(orderDto);

            // Assert
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("Order successfully created.");

            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Once);
            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsArgumentNullException_WhenOrderIsNull()
        {
            // Arrange
            OrderDto? orderDto = null;

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Order>()))
                .ThrowsAsync(new ArgumentNullException("Order was not provided for creation"));

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
            Func<Task> act = async () => await _orderService.CreateAsync(orderDto!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Order was not provided for creation.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("Order was not provided for creation.");

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
            var orderId = Guid.NewGuid();
            var orderDto = new OrderDto { Id = orderId };
            var order = new Order { OrderId = orderId };

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Order>()))
                .ThrowsAsync(new Exception($"Error occurred while adding the order with ID [{orderId}]."));

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
            Func<Task> act = async () => await _orderService.CreateAsync(orderDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Error occurred while adding the order with ID [{orderId}].");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Error occurred while adding the order with ID [{orderId}].");

            _repositoryMock.Verify(r => r.CreateAsync(order), Times.Never);
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
        public async Task UpdateAsync_LogInformation_WhenOrderUpdated()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                BookIds = new List<Guid>() { Guid.NewGuid() },
                DeliveryTypeId = Guid.NewGuid(),
                DeliveryDate = DateTime.UtcNow,
                DeliveryPrice = 159,
                OrderDate = DateTime.UtcNow,
                Region = "Region",
                City = "City",
                Address = "Address",
                Price = 162,
                Status = OrderStatus.PROCESSING
            };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<OrderDto>(It.IsAny<OrderDto>()))
                .Returns(new OrderDto());

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
            await _orderService.UpdateAsync(orderDto);

            // Assert
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Order with ID [{orderDto.Id}] successfully updated.");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once);
            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsArgumentNullException_WhenOrderIsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.UpdateAsync(null!))
                .ThrowsAsync(new ArgumentNullException("Order was not provided for update."));

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
            Func<Task> act = async () => await _orderService.UpdateAsync(null!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Order was not provided for update.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("Order was not provided for update.");

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsKeyNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderDto = new OrderDto { Id = orderId };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Order>()))
                .ThrowsAsync(new InvalidOperationException($"Order with ID [{orderDto.Id}] not found for update."));

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
            Func<Task> act = async () => await _orderService.UpdateAsync(orderDto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Order with ID [{orderDto.Id}] not found for update.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Order with ID [{orderDto.Id}] not found for update.");

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
            var orderId = Guid.NewGuid();
            var orderDto = new OrderDto { Id = orderId };
            var order = new Order { OrderId = orderId };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Order>()))
                .ThrowsAsync(new Exception($"Error occurred while updating order with ID [{orderDto.Id}]."));

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
            Func<Task> act = async () => await _orderService.UpdateAsync(orderDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Error occurred while updating the order with ID [{orderId}].");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Error occurred while updating the order with ID [{orderDto.Id}].");

            _repositoryMock.Verify(r => r.UpdateAsync(order), Times.Never);
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
        public async Task DeleteAsync_LogicInformation_WhenOrderIsDeleted()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(orderId))
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
            await _orderService.DeleteAsync(orderId);

            // Assert
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Order with ID [{orderId}] successfully deleted.");

            _repositoryMock.Verify(r => r.DeleteAsync(orderId), Times.Once);
            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsKeyNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(orderId))
                .ThrowsAsync(new KeyNotFoundException($"Order with ID [{orderId}] not found for deletion."));

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
            Func<Task> act = async () => await _orderService.DeleteAsync(orderId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Order with ID [{orderId}] not found for deletion.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Order with ID [{orderId}] not found for deletion.");

            _repositoryMock.Verify(r => r.DeleteAsync(orderId), Times.Once);
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
            var orderId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(orderId))
                .ThrowsAsync(new InvalidOperationException($"Error occurred while deleting the order with ID [{orderId}]."));

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
            Func<Task> act = async () => await _orderService.DeleteAsync(orderId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Error occurred while deleting the order with ID [{orderId}].");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Error occurred while deleting the order with ID [{orderId}].");

            _repositoryMock.Verify(r => r.DeleteAsync(orderId), Times.Once);
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
