using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using UserAPI.Repositories;
using UserAPI.Services;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Tests.Services
{
    public class PasswordServiceTests
    {
        private readonly Mock<IPasswordRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PasswordService>> _mockLogger;
        private readonly PasswordService _passwordService;

        public PasswordServiceTests()
        {
            _mockRepository = new Mock<IPasswordRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PasswordService>>();
            _passwordService = new PasswordService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var user = new Library.DTOs.User.User { Id = Guid.NewGuid() };
            var oldPassword = "oldPassword";
            var newPassword = "newPassword";
            _mockRepository.Setup(r => r.UpdateAsync(user.Id, oldPassword, newPassword)).ReturnsAsync(true);

            // Act
            var result = await _passwordService.UpdateAsync(user, oldPassword, newPassword);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.UpdateAsync(user.Id, oldPassword, newPassword), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenUpdateFails()
        {
            // Arrange
            var user = new Library.DTOs.User.User { Id = Guid.NewGuid() };
            var oldPassword = "oldPassword";
            var newPassword = "newPassword";
            _mockRepository.Setup(r => r.UpdateAsync(user.Id, oldPassword, newPassword)).ThrowsAsync(new Exception());

            // Act
            var result = await _passwordService.UpdateAsync(user, oldPassword, newPassword);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.UpdateAsync(user.Id, oldPassword, newPassword), Times.Once);
        }
    }
}
