using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookAPI.Controllers;
using BookAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AuthorDto = Library.DTOs.Book.Author;

using Library.Extensions;
using BookAPI.Services.Interfaces;

namespace BookAPI.Tests.Controllers
{
    public class AuthorsControllerTests
    {
        private readonly Mock<IAuthorService> _mockAuthorService;
        private readonly Mock<ILogger<AuthorsController>> _mockLogger;
        private readonly AuthorsController _controller;

        public AuthorsControllerTests()
        {
            _mockAuthorService = new Mock<IAuthorService>();
            _mockLogger = new Mock<ILogger<AuthorsController>>();
            _controller = new AuthorsController(_mockAuthorService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenAuthorsFetched()
        {
            // Arrange
            _mockAuthorService
                .Setup(s => s.GetAuthorsAsync(It.IsAny<int>(), It.IsAny<int>(), null, null, null))
                .ReturnsAsync(new PaginatedResult<AuthorDto>
                {
                    Items = new List<AuthorDto> { new AuthorDto() },
                    TotalCount = 1
                });

            // Act
            var result = await _controller.GetAuthors(1, 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<AuthorDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedAuthors = Assert.IsType<PaginatedResult<AuthorDto>>(okResult.Value);
            Assert.Equal(1, returnedAuthors.TotalCount);
            _mockAuthorService.Verify(s => s.GetAuthorsAsync(It.IsAny<int>(), It.IsAny<int>(), null, null, null));
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoAuthorsFound()
        {
            // Arrange
            _mockAuthorService
                .Setup(s => s.GetAuthorsAsync(It.IsAny<int>(), It.IsAny<int>(), null, null, null))
                .ReturnsAsync(new PaginatedResult<AuthorDto>
                {
                    Items = new List<AuthorDto>(),
                    TotalCount = 0
                });

            // Act
            var result = await _controller.GetAuthors(1, 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<AuthorDto>>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("No authors found.", notFoundResult.Value);
            _mockAuthorService.Verify(s => s.GetAuthorsAsync(It.IsAny<int>(), It.IsAny<int>(), null, null, null));
        }

        [Fact]
        public async Task GetAll_ReturnsBadRequest_WhenInvalidPageNumberOrSize()
        {
            // Arrange
            var invalidPageNumber = -1;
            var invalidPageSize = -1;

            // Act
            var result = await _controller.GetAuthors(invalidPageNumber, invalidPageSize);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<AuthorDto>>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Page number and page size must be greater than 0.", badRequestResult.Value);
            _mockAuthorService.Verify(s => s.GetAuthorsAsync(It.IsAny<int>(), It.IsAny<int>(), null, null, null), Times.Never);
        }

        [Fact]
        public async Task GetAll_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockAuthorService
                .Setup(s => s.GetAuthorsAsync(It.IsAny<int>(), It.IsAny<int>(), null, null, null))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetAuthors(1, 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<AuthorDto>>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Something went wrong", statusCodeResult.Value.ToString());
        }

        [Fact]
        public async Task GetAll_CallsGetAuthorsAsync_WhenCalled()
        {
            // Arrange
            _mockAuthorService
                .Setup(s => s.GetAuthorsAsync(It.IsAny<int>(), It.IsAny<int>(), null, null, null))
                .ReturnsAsync(new PaginatedResult<AuthorDto>
                {
                    Items = new List<AuthorDto> { new AuthorDto() },
                    TotalCount = 1
                });

            // Act
            var result = await _controller.GetAuthors(1, 10);

            // Assert
            _mockAuthorService.Verify(s => s.GetAuthorsAsync(It.IsAny<int>(), It.IsAny<int>(), null, null, null), Times.Once);
        }

        // GetAuthorById Tests
        [Fact]
        public async Task GetAuthorById_ReturnsOk_WhenAuthorFound()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorDto = new AuthorDto { AuthorId = authorId, Name = "Test Author" };
            _mockAuthorService
                .Setup(s => s.GetAuthorByIdAsync(authorId))
                .ReturnsAsync(authorDto);

            // Act
            var result = await _controller.GetAuthorById(authorId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedAuthor = Assert.IsType<AuthorDto>(okResult.Value);
            Assert.Equal(authorId, returnedAuthor.AuthorId);
            Assert.Equal("Test Author", returnedAuthor.Name);
            _mockAuthorService.Verify(s => s.GetAuthorByIdAsync(authorId), Times.Once);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsNotFound_WhenAuthorNotFound()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _mockAuthorService
                .Setup(s => s.GetAuthorByIdAsync(authorId))
                .ReturnsAsync((AuthorDto)null);

            // Act
            var result = await _controller.GetAuthorById(authorId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal($"Author with id {authorId} not found.", notFoundResult.Value);
            _mockAuthorService.Verify(s => s.GetAuthorByIdAsync(authorId), Times.Once);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _mockAuthorService
                .Setup(s => s.GetAuthorByIdAsync(authorId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetAuthorById(authorId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Something went wrong", statusCodeResult.Value.ToString());
            _mockAuthorService.Verify(s => s.GetAuthorByIdAsync(authorId), Times.Once);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsCreatedAtAction_WhenAuthorCreated()
        {
            // Arrange
            var authorDto = new AuthorDto { Name = "Test Author" };
            var createdAuthor = new AuthorDto { AuthorId = Guid.NewGuid(), Name = "Test Author" };
            _mockAuthorService
                .Setup(s => s.CreateAuthorAsync(authorDto))
                .ReturnsAsync(createdAuthor);

            // Act
            var result = await _controller.CreateAuthor(authorDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnedAuthor = Assert.IsType<AuthorDto>(createdResult.Value);
            Assert.Equal(createdAuthor.AuthorId, returnedAuthor.AuthorId);
            Assert.Equal(createdAuthor.Name, returnedAuthor.Name);
            Assert.Equal("GetAuthorById", createdResult.ActionName);
            _mockAuthorService.Verify(s => s.CreateAuthorAsync(authorDto), Times.Once);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsBadRequest_WhenInvalidDataProvided()
        {
            // Arrange
            AuthorDto authorDto = null;

            // Act
            var result = await _controller.CreateAuthor(authorDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var authorDto = new AuthorDto { Name = "Test Author" };
            _mockAuthorService
                .Setup(s => s.CreateAuthorAsync(authorDto))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.CreateAuthor(authorDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());
            _mockAuthorService.Verify(s => s.CreateAuthorAsync(authorDto), Times.Once);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsOk_WhenAuthorUpdated()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorDto = new AuthorDto { Name = "Updated Author" };
            var updatedAuthor = new AuthorDto { AuthorId = authorId, Name = "Updated Author" };

            _mockAuthorService
                .Setup(s => s.UpdateAuthorAsync(authorId, authorDto))
                .ReturnsAsync(updatedAuthor);

            // Act
            var result = await _controller.UpdateAuthor(authorId, authorDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedAuthor = Assert.IsType<AuthorDto>(okResult.Value);
            Assert.Equal(updatedAuthor.AuthorId, returnedAuthor.AuthorId);
            Assert.Equal(updatedAuthor.Name, returnedAuthor.Name);
            _mockAuthorService.Verify(s => s.UpdateAuthorAsync(authorId, authorDto), Times.Once);
        }


        [Fact]
        public async Task UpdateAuthor_ReturnsBadRequest_WhenInvalidDataProvided()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            AuthorDto authorDto = null;

            // Act
            var result = await _controller.UpdateAuthor(authorId, authorDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorDto = new AuthorDto { Name = "Updated Author" };
            _mockAuthorService
                .Setup(s => s.UpdateAuthorAsync(authorId, authorDto))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.UpdateAuthor(authorId, authorDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());
            _mockAuthorService.Verify(s => s.UpdateAuthorAsync(authorId, authorDto), Times.Once);
        }


        [Fact]
        public async Task UpdateAuthor_ReturnsNotFound_WhenAuthorNotFound()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorDto = new AuthorDto { Name = "Updated Author" };
            _mockAuthorService
                .Setup(s => s.UpdateAuthorAsync(authorId, authorDto))
                .ReturnsAsync((AuthorDto)null);

            // Act
            var result = await _controller.UpdateAuthor(authorId, authorDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AuthorDto>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Author with id " + authorId + " not found.", notFoundResult.Value);
        }



        [Fact]
        public async Task DeleteAuthor_ReturnsNoContent_WhenAuthorDeleted()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _mockAuthorService
                .Setup(s => s.DeleteAuthorAsync(authorId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAuthor(authorId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockAuthorService.Verify(s => s.DeleteAuthorAsync(authorId), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsNotFound_WhenAuthorNotFound()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _mockAuthorService
                .Setup(s => s.DeleteAuthorAsync(authorId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteAuthor(authorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Author with id {authorId} not found.", notFoundResult.Value);
            _mockAuthorService.Verify(s => s.DeleteAuthorAsync(authorId), Times.Once);
        }
        [Fact]
        public async Task DeleteAuthor_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _mockAuthorService
                .Setup(s => s.DeleteAuthorAsync(authorId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.DeleteAuthor(authorId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Something went wrong", statusCodeResult.Value.ToString());
            _mockAuthorService.Verify(s => s.DeleteAuthorAsync(authorId), Times.Once);
        }


    }
}
