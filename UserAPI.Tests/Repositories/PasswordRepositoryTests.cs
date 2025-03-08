using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Repositories;
using Xunit;

namespace UserAPI.Tests.Repositories
{
    public class PasswordRepositoryTests
    {
        private readonly PasswordRepository _passwordRepository;
        private readonly Mock<UserDbContext> _mockContext;

        public PasswordRepositoryTests()
        {
            _mockContext = new Mock<UserDbContext>(new DbContextOptions<UserDbContext>());
            _passwordRepository = new PasswordRepository(_mockContext.Object);
        }

        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var password = "password";
            var salt = "salt";

            // Act
            var result = _passwordRepository.HashPassword(password, salt);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(password, result);
        }

        [Fact]
        public void GenerateSalt_ShouldReturnSaltOfCorrectSize()
        {
            // Arrange
            var size = 8;

            // Act
            var result = _passwordRepository.GenerateSalt(size);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(size, result.Length);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPassword()
        {
            // Arrange
            var passwordId = Guid.NewGuid();
            var password = new Password { PasswordId = passwordId };
            _mockContext.Setup(c => c.Passwords.FindAsync(passwordId)).ReturnsAsync(password);

            // Act
            var result = await _passwordRepository.GetByIdAsync(passwordId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(passwordId, result.PasswordId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenPasswordIsUpdated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var oldPassword = "oldPassword";
            var newPassword = "newPassword";
            var password = new Password
            {
                PasswordId = userId,
                PasswordHash = _passwordRepository.HashPassword(oldPassword, "salt"),
                PasswordSalt = "salt",
                UserId = userId
            };
            _mockContext.Setup(c => c.Passwords.FindAsync(userId)).ReturnsAsync(password);

            // Act
            var result = await _passwordRepository.UpdateAsync(userId, oldPassword, newPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyAsync_ShouldReturnTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var passwordId = Guid.NewGuid();
            var plainPassword = "password";
            var password = new Password
            {
                PasswordId = passwordId,
                PasswordHash = _passwordRepository.HashPassword(plainPassword, "salt"),
                PasswordSalt = "salt"
            };
            _mockContext.Setup(c => c.Passwords.FindAsync(passwordId)).ReturnsAsync(password);

            // Act
            var result = await _passwordRepository.VerifyAsync(passwordId, plainPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnTrue_WhenPasswordIsAdded()
        {
            // Arrange
            var password = "password";
            var user = new User { UserId = Guid.NewGuid() };

            // Act
            var result = await _passwordRepository.AddAsync(password, user);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenPasswordIsDeleted()
        {
            // Arrange
            var passwordId = Guid.NewGuid();
            var password = new Password { PasswordId = passwordId };
            _mockContext.Setup(c => c.Passwords.FindAsync(passwordId)).ReturnsAsync(password);

            // Act
            var result = await _passwordRepository.DeleteAsync(passwordId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetHashByIdAsync_ShouldReturnPasswordHash()
        {
            // Arrange
            var passwordId = Guid.NewGuid();
            var passwordHash = "hashedPassword";
            var password = new Password { PasswordId = passwordId, PasswordHash = passwordHash };
            _mockContext.Setup(c => c.Passwords.FindAsync(passwordId)).ReturnsAsync(password);

            // Act
            var result = await _passwordRepository.GetHashByIdAsync(passwordId);

            // Assert
            Assert.Equal(passwordHash, result);
        }
    }
}
