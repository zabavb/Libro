using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderApi.Controllers;
using Library.DTOs.Order;
using Microsoft.Extensions.Logging;
using Library.Extensions;
using Xunit;
using OrderAPI.Services.Interfaces;

namespace OrderAPI.Tests.Controllers
{
    public class DeliveryTypesControllerTests
    {
        private readonly Mock<IDeliveryTypeService> _deliveryTypeServiceMock;
        private readonly Mock<ILogger<DeliveryTypesController>> _loggerMock;
        private readonly DeliveryTypesController _controller;

        public DeliveryTypesControllerTests()
        {
            _deliveryTypeServiceMock = new Mock<IDeliveryTypeService>();
            _loggerMock = new Mock<ILogger<DeliveryTypesController>>();
            _controller = new DeliveryTypesController(_deliveryTypeServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithDeliveryTypes()
        {
            // Arrange
            var deliveryTypes = new List<DeliveryType>
    {
        new DeliveryType { Id = Guid.NewGuid(), ServiceName = "Standard" },
        new DeliveryType { Id = Guid.NewGuid(), ServiceName = "Express" }
    };

            var paginatedResult = new PaginatedResult<DeliveryType>
            {
                Items = deliveryTypes,
                TotalCount = deliveryTypes.Count,
                PageSize = 10,
            };

            _deliveryTypeServiceMock.Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                                     .ReturnsAsync(paginatedResult);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            Xunit.Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Xunit.Assert.Equal(paginatedResult, okResult.Value);
        }


        [Fact]
        public async Task GetById_ReturnsOk_WithDeliveryType()
        {
            // Arrange
            var deliveryTypeId = Guid.NewGuid();
            var deliveryType = new DeliveryType { Id = deliveryTypeId, ServiceName = "Standard" };

            _deliveryTypeServiceMock.Setup(s => s.GetByIdAsync(deliveryTypeId))
                                     .ReturnsAsync(deliveryType);

            // Act
            var result = await _controller.GetById(deliveryTypeId);

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result.Result);
            Xunit.Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Xunit.Assert.Equal(deliveryType, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenDeliveryTypeDoesNotExist()
        {
            // Arrange
            var deliveryTypeId = Guid.NewGuid();

            _deliveryTypeServiceMock.Setup(s => s.GetByIdAsync(deliveryTypeId))
                                     .Throws<KeyNotFoundException>();

            // Act
            var result = await _controller.GetById(deliveryTypeId);

            // Assert
            var notFoundResult = Xunit.Assert.IsType<ObjectResult>(result.Result);
            Xunit.Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithDeliveryType()
        {
            // Arrange
            var deliveryType = new DeliveryType { Id = Guid.NewGuid(), ServiceName = "Express" };

            _deliveryTypeServiceMock.Setup(s => s.CreateAsync(It.IsAny<DeliveryType>()))
                                     .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(new DeliveryType { Id = deliveryType.Id, ServiceName = deliveryType.ServiceName });

            // Assert
            var createdResult = Xunit.Assert.IsType<CreatedAtActionResult>(result.Result);
            Xunit.Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deliveryType = new DeliveryType { Id = id, ServiceName = "Updated Name" };

            _deliveryTypeServiceMock.Setup(s => s.UpdateAsync(It.IsAny<DeliveryType>()))
                                     .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(id, new DeliveryType { Id = deliveryType.Id, ServiceName = deliveryType.ServiceName });

            // Assert
            var noContentResult = Xunit.Assert.IsType<NoContentResult>(result);
            Xunit.Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var deliveryTypeId = Guid.NewGuid();

            _deliveryTypeServiceMock.Setup(s => s.DeleteAsync(deliveryTypeId))
                                     .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(deliveryTypeId);

            // Assert
            var noContentResult = Xunit.Assert.IsType<NoContentResult>(result);
            Xunit.Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenDeliveryTypeDoesNotExist()
        {
            // Arrange
            var deliveryTypeId = Guid.NewGuid();

            _deliveryTypeServiceMock.Setup(s => s.DeleteAsync(deliveryTypeId))
                                     .Throws<KeyNotFoundException>();

            // Act
            var result = await _controller.Delete(deliveryTypeId);

            // Assert
            var notFoundResult = Xunit.Assert.IsType<ObjectResult>(result);
            Xunit.Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
    }
}