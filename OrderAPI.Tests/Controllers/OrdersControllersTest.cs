using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderApi.Controllers;
using Library.DTOs.Order;
using FluentAssertions;
using Library.Extensions;
using OrderAPI.Services.Interfaces;

namespace OrderAPI.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<ILogger<OrdersController>> _loggerMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _loggerMock = new Mock<ILogger<OrdersController>>();
            _controller = new OrdersController(_orderServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithOrders()
        {
            // Arrange
            var orders = new List<Order> { new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Region = "Region1" } };
            var paginatedResult = new PaginatedResult<Order>
            {
                Items = orders,
                TotalCount = orders.Count,
                PageNumber = 1,
                PageSize = 10
            };

            _orderServiceMock.Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, null))
                             .ReturnsAsync(paginatedResult);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(paginatedResult);
        }


        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            _orderServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new KeyNotFoundException("Order not found"));

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<ObjectResult>()
                  .Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenValidOrder()
        {
            // Arrange
            var orderDto = new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Region = "Region1" };
            _orderServiceMock.Setup(s => s.CreateAsync(orderDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(orderDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>()
                  .Which.ActionName.Should().Be(nameof(_controller.GetById));
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var orderDto = new Order { Id = id, UserId = Guid.NewGuid(), Region = "Region1" };
            _orderServiceMock.Setup(s => s.UpdateAsync(orderDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(id, orderDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _orderServiceMock.Setup(s => s.DeleteAsync(orderId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(orderId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Region", "Region is required");
            var orderDto = new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };

            // Act
            var result = await _controller.Create(orderDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Region = "Region1" };
            _orderServiceMock.Setup(s => s.GetByIdAsync(order.Id)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetById(order.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(order);
        }
    }
}