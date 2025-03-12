using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OrderApi.Services;
using OrderAPI.Repository;
using DeliveryTypeDto = Library.DTOs.Order.DeliveryType;
using DeliveryType = OrderApi.Models.DeliveryType;
using Library.Extensions;
using FluentAssertions;
namespace OrderAPI.Tests.Services
{
    public class DeliveryTypeServiceTests
    {
        private readonly Mock<IDeliveryTypeRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<DeliveryTypeService>> _loggerMock;
        private readonly DeliveryTypeService _deliveryTypeService;

        public DeliveryTypeServiceTests()
        {
            _repositoryMock = new Mock<IDeliveryTypeRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<DeliveryTypeService>>();
            _deliveryTypeService = new DeliveryTypeService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        //================== Get all ==============

        [Fact]
        public async Task GetAllAsync_ReturnsPaginatedResult_WhenDeliveryTypeExist()
        {
            // Arrange
            var deliveryType = new DeliveryType { DeliveryId = Guid.NewGuid() };
            var deliveryTypeDto = new DeliveryTypeDto { Id = Guid.NewGuid() };
            var paginatedDeliveryTypes = new PaginatedResult<DeliveryType>
            {
                Items = [deliveryType],
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10,
            };

            _repositoryMock
                .Setup(r => r.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(paginatedDeliveryTypes);

            _mapperMock
                .Setup(m => m.Map<ICollection<DeliveryTypeDto>>(It.IsAny<ICollection<DeliveryType>>()))
                .Returns([deliveryTypeDto]);

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
            var result = await _deliveryTypeService.GetAllAsync(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
            capturedLogMessage.Should().Contain("Successfully fetched [1] delivery types.");

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
                .Setup(r => r.GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((PaginatedResult<DeliveryType>)null!);

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
                await _deliveryTypeService.GetAllAsync(It.IsAny<int>(), It.IsAny<int>());
            };

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Failed to fetch paginated delivery types.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("Failed to fetch paginated delivery types.");

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
        public async Task GetByIdAsync_ReturnsDeliveryType_WhenDeliveryTypeExists()
        {
            // Arrange
            var deliveryId = Guid.NewGuid();
            var deliveryTypeId = Guid.NewGuid();
            var deliveryType = new DeliveryType
            {
                DeliveryId = deliveryId,
                ServiceName = "Some mail"
            };

            var deliveryTypeDto = new DeliveryTypeDto
            {
                Id = deliveryId,
                ServiceName = deliveryType.ServiceName
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(deliveryId))
                .ReturnsAsync(deliveryType);

            _mapperMock
                .Setup(m => m.Map<DeliveryTypeDto>(It.IsAny<DeliveryType>()))
                .Returns(deliveryTypeDto);

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
            var result = await _deliveryTypeService.GetByIdAsync(deliveryId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(deliveryId);
            capturedLogMessage.Should().Contain($"Delivery type with ID [{deliveryId}] successfully fetched.");

            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsKeyNotFoundException_WhenDeliveryTypeDoesNotExist()
        {
            // Arrange
            var deliveryId = Guid.NewGuid();
            _repositoryMock
                .Setup(r => r.GetByIdAsync(deliveryId))
                .ReturnsAsync((DeliveryType?)null);

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
            Func<Task> act = async () => await _deliveryTypeService.GetByIdAsync(deliveryId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Delivery type with ID [{deliveryId}] not found.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Delivery type with ID [{deliveryId}] not found.");

            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        //========================= Create =========================

        [Fact]
        public async Task CreateAsync_LogInformation_WhenDeliveryTypeIsCreated()
        {
            // Arrange
            var deliveryTypeDto = new DeliveryTypeDto
            {
                Id = Guid.NewGuid(),
                ServiceName = "Some mail"
            };

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<DeliveryType>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<DeliveryType>(It.IsAny<DeliveryTypeDto>()))
                .Returns((DeliveryTypeDto dto) => new DeliveryType());

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
            await _deliveryTypeService.CreateAsync(deliveryTypeDto);

            // Assert
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("Delivery type successfully created.");

            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<DeliveryType>()), Times.Once);
            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsArgumentNullException_WhenDeliveryTypeIsNull()
        {
            // Arrange
            DeliveryTypeDto deliveryTypeDto = null;

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<DeliveryType>()))
                .ThrowsAsync(new ArgumentNullException("Delivery type was not provided for creation."));

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
            Func<Task> act = async () => await _deliveryTypeService.CreateAsync(deliveryTypeDto!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Delivery type was not provided for creation.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("Delivery type was not provided for creation.");

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
            var deliveryTypeId = Guid.NewGuid();
            var deliveryTypeDto = new DeliveryTypeDto { Id = deliveryTypeId };
            var deliveryType = new DeliveryType { DeliveryId = deliveryTypeId };

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<DeliveryType>()))
                .ThrowsAsync(new Exception($"Error occurred while adding the delivery type with ID [{deliveryTypeId}]."));

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
            Func<Task> act = async () => await _deliveryTypeService.CreateAsync(deliveryTypeDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Error occurred while adding the delivery type with ID [{deliveryTypeId}].");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Error occurred while adding the delivery type with ID [{deliveryTypeId}].");

            _repositoryMock.Verify(r => r.CreateAsync(deliveryType), Times.Never);
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
        public async Task UpdateAsync_LogInformation_WhenDeliveryTypeUpdated()
        {
            // Arrange
            var deliveryDto = new DeliveryTypeDto
            {
                Id = Guid.NewGuid(),
                ServiceName = "Some mail"
            };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<DeliveryType>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<DeliveryTypeDto>(It.IsAny<DeliveryTypeDto>()))
                .Returns(new DeliveryTypeDto());

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
            await _deliveryTypeService.UpdateAsync(deliveryDto);

            // Assert
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Delivery type with ID [{deliveryDto.Id}] successfully updated.");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<DeliveryType>()), Times.Once);
            _loggerMock.Verify(l => l.Log(It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsArgumentNullException_WhenDeliveryTypeIsNull()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.UpdateAsync(null!))
                .ThrowsAsync(new ArgumentNullException("Delivery type was not provided for the update."));
            
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
            Func<Task> act = async () => await _deliveryTypeService.UpdateAsync(null!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Delivery type was not provided for the update.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain("Delivery type was not provided for the update.");

            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsKeyNotFoundException_WhenDeliveryTypeDoesNotExist()
        {
            // Arrange
            var deliveryTypeId = Guid.NewGuid();
            var deliveryDto = new DeliveryTypeDto { Id = deliveryTypeId };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<DeliveryType>()))
                .ThrowsAsync(new InvalidOperationException($"Delivery type with ID [{deliveryDto.Id}] not found for update."));

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
            Func<Task> act = async () => await _deliveryTypeService.UpdateAsync(deliveryDto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Delivery type with ID [{deliveryDto.Id}] not found for update.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Delivery type with ID [{deliveryDto.Id}] not found for update.");

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
            var deliveryId = Guid.NewGuid();
            var deliveryTypeDto = new DeliveryTypeDto { Id = deliveryId };
            var deliveryType = new DeliveryType { DeliveryId = deliveryId };

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<DeliveryType>()))
                .ThrowsAsync(new Exception($"Error occured while updating delivery type with ID [{deliveryTypeDto.Id}]."));

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
            Func<Task> act = async () => await _deliveryTypeService.UpdateAsync(deliveryTypeDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Error occured while updating the delivery type with ID [{deliveryId}].");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Error occured while updating the delivery type with ID [{deliveryTypeDto.Id}].");

            _repositoryMock.Verify(r => r.UpdateAsync(deliveryType), Times.Never);
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
        public async Task DeleteAsync_LogicInformation_WhenDeliveryTypeIsDeleted()
        {
            // Arrange
            var deliveryId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(deliveryId))
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
            await _deliveryTypeService.DeleteAsync(deliveryId);

            // Assert
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Delivery type with ID [{deliveryId}] successfully deleted.");

            _repositoryMock.Verify(r => r.DeleteAsync(deliveryId), Times.Once);
            _loggerMock.Verify(l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsKeyNotFoundException_WhenDeliveryTypeDoesNotExist()
        {
            // Arrange
            var deliveryId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(deliveryId))
                .ThrowsAsync(new KeyNotFoundException($"Delivery type with ID [{deliveryId}] not found for deletion."));

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
            Func<Task> act = async () => await _deliveryTypeService.DeleteAsync(deliveryId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Delivery type with ID [{deliveryId}] not found for deletion.");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Delivery type with ID [{deliveryId}] not found for deletion.");

            _repositoryMock.Verify(r => r.DeleteAsync(deliveryId), Times.Once);
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
            var deliveryId = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.DeleteAsync(deliveryId))
                .ThrowsAsync(new InvalidOperationException($"Error occurred while deleting the delivery type with ID [{deliveryId}]."));

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
            Func<Task> act = async () => await _deliveryTypeService.DeleteAsync(deliveryId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Error occurred while deleting the delivery type with ID [{deliveryId}].");
            capturedLogMessage.Should().NotBeNull();
            capturedLogMessage.Should().Contain($"Error occurred while deleting the delivery type with ID [{deliveryId}].");

            _repositoryMock.Verify(r => r.DeleteAsync(deliveryId), Times.Once);
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
