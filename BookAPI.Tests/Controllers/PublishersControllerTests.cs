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
using Library.Filters;
using Library.Sortings;
using PublisherDto = Library.DTOs.Book.Publisher;
using BookAPI.Services.Interfaces;
using Library.Common;


namespace BookAPI.Tests.Controllers
{
    public class PublishersControllerTests
    {
        private readonly Mock<IPublisherService> _mockPublisherService;
        private readonly Mock<ILogger<PublishersController>> _mockLogger;
        private readonly PublishersController _controller;

        public PublishersControllerTests()
        {
            _mockPublisherService = new Mock<IPublisherService>();
            _mockLogger = new Mock<ILogger<PublishersController>>();
            _controller = new PublishersController(_mockPublisherService.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task GetAll_ReturnsOk_WhenPublishersFetched()
        {
            // Arrange
            _mockPublisherService
                .Setup(s => s.GetPublishersAsync(It.IsAny<int>(), It.IsAny<int>(), null, null))
                .ReturnsAsync(new PaginatedResult<PublisherDto>
                {
                    Items = new List<PublisherDto> { new PublisherDto() }, 
                    TotalCount = 1
                });

            // Act
            var result = await _controller.GetPublishers(1, 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<PublisherDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedPublishers = Assert.IsType<PaginatedResult<PublisherDto>>(okResult.Value);
            Assert.Equal(1, returnedPublishers.TotalCount); 
            _mockPublisherService.Verify(s => s.GetPublishersAsync(It.IsAny<int>(), It.IsAny<int>(), null, null));
        }
        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoPublishersFound()
        {
            // Arrange
            _mockPublisherService
                .Setup(s => s.GetPublishersAsync(It.IsAny<int>(), It.IsAny<int>(), null, null))
                .ReturnsAsync(new PaginatedResult<PublisherDto>
                {
                    Items = new List<PublisherDto>(), 
                    TotalCount = 0
                });

            // Act
            var result = await _controller.GetPublishers(1, 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<PublisherDto>>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("No publishers found.", notFoundResult.Value);
            _mockPublisherService.Verify(s => s.GetPublishersAsync(It.IsAny<int>(), It.IsAny<int>(), null, null));
        }
        [Fact]
        public async Task GetAll_ReturnsBadRequest_WhenInvalidPageNumberOrSize()
        {
            // Arrange
            var invalidPageNumber = -1; 
            var invalidPageSize = -1;  

            // Act
            var result = await _controller.GetPublishers(invalidPageNumber, invalidPageSize);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<PublisherDto>>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Page number and page size must be greater than 0.", badRequestResult.Value);
            _mockPublisherService.Verify(s => s.GetPublishersAsync(It.IsAny<int>(), It.IsAny<int>(), null, null), Times.Never);
        }
        [Fact]
        public async Task GetAll_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockPublisherService
                .Setup(s => s.GetPublishersAsync(It.IsAny<int>(), It.IsAny<int>(), null, null))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetPublishers(1, 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<PublisherDto>>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Something went wrong", statusCodeResult.Value.ToString()); 
        }
        [Fact]
        public async Task GetAll_CallsGetPublishersAsync_WhenCalled()
        {
            // Arrange
            _mockPublisherService
                .Setup(s => s.GetPublishersAsync(It.IsAny<int>(), It.IsAny<int>(), null, null))
                .ReturnsAsync(new PaginatedResult<PublisherDto>
                {
                    Items = new List<PublisherDto> { new PublisherDto() },
                    TotalCount = 1
                });

            // Act
            var result = await _controller.GetPublishers(1, 10);

            // Assert
            _mockPublisherService.Verify(s => s.GetPublishersAsync(It.IsAny<int>(), It.IsAny<int>(), null, null), Times.Once);
        }

        // GetPublisherById Tests
        [Fact]
        public async Task GetPublisherById_ReturnsOk_WhenPublisherFound()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var publisherDto = new PublisherDto { PublisherId = publisherId, Name = "Test Publisher" };
            _mockPublisherService
                .Setup(s => s.GetPublisherByIdAsync(publisherId))
                .ReturnsAsync(publisherDto);

            // Act
            var result = await _controller.GetPublisherById(publisherId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedPublisher = Assert.IsType<PublisherDto>(okResult.Value);
            Assert.Equal(publisherId, returnedPublisher.PublisherId);
            Assert.Equal("Test Publisher", returnedPublisher.Name);
            _mockPublisherService.Verify(s => s.GetPublisherByIdAsync(publisherId), Times.Once);
        }

        [Fact]
        public async Task GetPublisherById_ReturnsNotFound_WhenPublisherNotFound()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            _mockPublisherService
                .Setup(s => s.GetPublisherByIdAsync(publisherId))
                .ReturnsAsync((PublisherDto)null);

            // Act
            var result = await _controller.GetPublisherById(publisherId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal($"Publisher with id {publisherId} not found.", notFoundResult.Value);
            _mockPublisherService.Verify(s => s.GetPublisherByIdAsync(publisherId), Times.Once); 
        }

        [Fact]
        public async Task GetPublisherById_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            _mockPublisherService
                .Setup(s => s.GetPublisherByIdAsync(publisherId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetPublisherById(publisherId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());  
            _mockPublisherService.Verify(s => s.GetPublisherByIdAsync(publisherId), Times.Once); 
        }
        [Fact]
        public async Task CreatePublisher_ReturnsCreatedAtAction_WhenPublisherCreated()
        {
            // Arrange
            var publisherDto = new PublisherDto { Name = "Test Publisher" };
            var createdPublisher = new PublisherDto { PublisherId = Guid.NewGuid(), Name = "Test Publisher" };
            _mockPublisherService
                .Setup(s => s.CreatePublisherAsync(publisherDto))
                .ReturnsAsync(createdPublisher);

            // Act
            var result = await _controller.CreatePublisher(publisherDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnedPublisher = Assert.IsType<PublisherDto>(createdResult.Value);
            Assert.Equal(createdPublisher.PublisherId, returnedPublisher.PublisherId);  
            Assert.Equal(createdPublisher.Name, returnedPublisher.Name);  
            Assert.Equal("GetPublisherById", createdResult.ActionName);
            _mockPublisherService.Verify(s => s.CreatePublisherAsync(publisherDto), Times.Once);
        }
        [Fact]
        public async Task CreatePublisher_ReturnsBadRequest_WhenInvalidDataProvided()
        {
            // Arrange
            PublisherDto publisherDto = null;

            // Act
            var result = await _controller.CreatePublisher(publisherDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }
        [Fact]
        public async Task CreatePublisher_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var publisherDto = new PublisherDto { Name = "Test Publisher" };
            _mockPublisherService
                .Setup(s => s.CreatePublisherAsync(publisherDto))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.CreatePublisher(publisherDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());
            _mockPublisherService.Verify(s => s.CreatePublisherAsync(publisherDto), Times.Once);
        }
        [Fact]
        public async Task UpdatePublisher_ReturnsOk_WhenPublisherUpdated()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var publisherDto = new PublisherDto { Name = "Updated Publisher" };
            var updatedPublisher = new PublisherDto { PublisherId = publisherId, Name = "Updated Publisher" };

            _mockPublisherService
                .Setup(s => s.UpdatePublisherAsync(publisherId, publisherDto))
                .ReturnsAsync(updatedPublisher);

            // Act
            var result = await _controller.UpdatePublisher(publisherId, publisherDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedPublisher = Assert.IsType<PublisherDto>(okResult.Value);
            Assert.Equal(updatedPublisher.PublisherId, returnedPublisher.PublisherId);
            Assert.Equal(updatedPublisher.Name, returnedPublisher.Name);
            _mockPublisherService.Verify(s => s.UpdatePublisherAsync(publisherId, publisherDto), Times.Once);
        }
        [Fact]
        public async Task UpdatePublisher_ReturnsBadRequest_WhenInvalidDataProvided()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            PublisherDto publisherDto = null;

            // Act
            var result = await _controller.UpdatePublisher(publisherId, publisherDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }
        [Fact]
        public async Task UpdatePublisher_ReturnsNotFound_WhenPublisherNotFound()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var publisherDto = new PublisherDto { Name = "Updated Publisher" };
            _mockPublisherService
                .Setup(s => s.UpdatePublisherAsync(publisherId, publisherDto))
                .ReturnsAsync((PublisherDto)null);

            // Act
            var result = await _controller.UpdatePublisher(publisherId, publisherDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Publisher not found.", notFoundResult.Value);
        }
        [Fact]
        public async Task UpdatePublisher_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var publisherDto = new PublisherDto { Name = "Updated Publisher" };
            _mockPublisherService
                .Setup(s => s.UpdatePublisherAsync(publisherId, publisherDto))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.UpdatePublisher(publisherId, publisherDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PublisherDto>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());
            _mockPublisherService.Verify(s => s.UpdatePublisherAsync(publisherId, publisherDto), Times.Once);
        }

        [Fact]
        public async Task DeletePublisher_ReturnsNoContent_WhenPublisherDeleted()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            _mockPublisherService
                .Setup(s => s.DeletePublisherAsync(publisherId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeletePublisher(publisherId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            _mockPublisherService.Verify(s => s.DeletePublisherAsync(publisherId), Times.Once);
        }
        [Fact]
        public async Task DeletePublisher_ReturnsNotFound_WhenPublisherNotFound()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            _mockPublisherService
                .Setup(s => s.DeletePublisherAsync(publisherId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeletePublisher(publisherId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Publisher not found.", notFoundResult.Value);
        }
        [Fact]
        public async Task DeletePublisher_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            _mockPublisherService
                .Setup(s => s.DeletePublisherAsync(publisherId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.DeletePublisher(publisherId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());
            _mockPublisherService.Verify(s => s.DeletePublisherAsync(publisherId), Times.Once);
        }


















    }
}
