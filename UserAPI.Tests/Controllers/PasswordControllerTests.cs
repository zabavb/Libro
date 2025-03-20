using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using UserAPI.Controllers;
using UserAPI.Services.Interfaces;
using Xunit;

namespace UserAPI.Tests.Controllers
{
    public class PasswordControllerTests
    {
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly PasswordController _controller;

        public PasswordControllerTests()
        {
            _mockPasswordService = new Mock<IPasswordService>();
            _controller = new PasswordController(_mockPasswordService.Object);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var user = new Library.DTOs.User.User { Id = new Guid() };
            var oldPassword = "oldPassword";
            var newPassword = "newPassword";
            _mockPasswordService.Setup(service => service.UpdateAsync(user, oldPassword, newPassword)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(user, oldPassword, newPassword);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.Update(null, null, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("some error", ((SerializableError)badRequestResult.Value)["error"]);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var user = new Library.DTOs.User.User { Id = new Guid() };
            var oldPassword = "oldPassword";
            var newPassword = "newPassword";
            _mockPasswordService.Setup(service => service.UpdateAsync(user, oldPassword, newPassword)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(user, oldPassword, newPassword);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password update failed.", ((dynamic)badRequestResult.Value).message);
        }

        [Fact]
        public async Task Update_ReturnsStatusCode500_WhenExceptionIsThrown()
        {
            // Arrange
            var user = new Library.DTOs.User.User {Id = new Guid() };
            var oldPassword = "oldPassword";
            var newPassword = "newPassword";
            _mockPasswordService.Setup(service => service.UpdateAsync(user, oldPassword, newPassword)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Update(user, oldPassword, newPassword);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Test exception", ((dynamic)statusCodeResult.Value).message);
        }
    }
}