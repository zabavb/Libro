using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.DTOs.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrderApi.Controllers;
using OrderApi.Services;
using Xunit;
using FluentAssertions;

namespace OrderApi.Tests
{
    public class StatusControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<ILogger<StatusController>> _loggerMock;
        private readonly StatusController _controller;

        public StatusControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _loggerMock = new Mock<ILogger<StatusController>>();
            _controller = new StatusController(_orderServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Update_ValidData_ReturnsNoContent()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderStatus = OrderStatus.PROCESSING;
            var existingOrder = new Order
            {
                Id = orderId,
                Status = OrderStatus.PENDING
            };

            _orderServiceMock.Setup(s => s.GetByIdAsync(orderId))
                             .ReturnsAsync(existingOrder);
            _orderServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(orderId, orderStatus);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _orderServiceMock.Verify(s => s.GetByIdAsync(orderId), Times.Once);
            _orderServiceMock.Verify(s => s.UpdateAsync(It.Is<Order>(o => o.Status == OrderStatus.PROCESSING)), Times.Once);
        }

        [Fact]
        public async Task Update_OrderNotFound_ReturnsNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderStatus = OrderStatus.PROCESSING;

            _orderServiceMock.Setup(s => s.GetByIdAsync(orderId))
                             .ThrowsAsync(new KeyNotFoundException("Order not found"));

            // Act
            var result = await _controller.Update(orderId, orderStatus);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult?.Value.Should().BeEquivalentTo(new { message = "Order not found" });
        }

        [Fact]
        public async Task Update_InvalidOrderStatus_ReturnsBadRequest()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var invalidStatus = (OrderStatus)999;

            // Act
            var result = await _controller.Update(orderId, invalidStatus);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _orderServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task Update_UnexpectedError_ReturnsInternalServerError()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderStatus = OrderStatus.PROCESSING;

            _orderServiceMock.Setup(s => s.GetByIdAsync(orderId))
                             .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Update(orderId, orderStatus);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            objectResult?.Value.Should().BeEquivalentTo(new { message = "Unexpected error" });
        }
    }
}
