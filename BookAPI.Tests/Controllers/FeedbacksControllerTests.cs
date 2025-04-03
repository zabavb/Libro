using BookAPI.Services;
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
using FeedbackDto = Library.DTOs.Book.Feedback;
using BookAPI.Services.Interfaces;
using Library.Common;


namespace BookAPI.Tests.Controllers
{
    public class FeedbacksControllerTests
    {
        private readonly Mock<IFeedbackService> _mockFeedbackService;
        private readonly Mock<ILogger<FeedbacksController>> _mockLogger;
        private readonly FeedbacksController _controller;

        public FeedbacksControllerTests()
        {
            _mockFeedbackService = new Mock<IFeedbackService>();
            _mockLogger = new Mock<ILogger<FeedbacksController>>();
            _controller = new FeedbacksController(_mockFeedbackService.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task GetAll_ReturnsOk_WhenFeedbacksFetched()
        {
            // Arrange
            _mockFeedbackService
                .Setup(s => s.GetFeedbacksAsync(It.IsAny<int>(), It.IsAny<int>(), null, null))
                .ReturnsAsync(new PaginatedResult<FeedbackDto>
                {
                    Items = new List<FeedbackDto> { new FeedbackDto() }, 
                    TotalCount = 1
                });

            // Act
            var result = await _controller.GetFeedbacks(1, 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<FeedbackDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedFeedbacks = Assert.IsType<PaginatedResult<FeedbackDto>>(okResult.Value);
            Assert.Equal(1, returnedFeedbacks.TotalCount); 
            _mockFeedbackService.Verify(s => s.GetFeedbacksAsync(It.IsAny<int>(), It.IsAny<int>(), null, null));
        }
        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoFeedbacksFound()
        {
            // Arrange
            _mockFeedbackService
                .Setup(s => s.GetFeedbacksAsync(It.IsAny<int>(), It.IsAny<int>(), null, null))
                .ReturnsAsync(new PaginatedResult<FeedbackDto>
                {
                    Items = new List<FeedbackDto>(), 
                    TotalCount = 0
                });

            // Act
            var result = await _controller.GetFeedbacks(1, 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<FeedbackDto>>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("No feedbacks found.", notFoundResult.Value);
            _mockFeedbackService.Verify(s => s.GetFeedbacksAsync(It.IsAny<int>(), It.IsAny<int>(), null, null));
        }
        [Fact]
        public async Task GetAll_ReturnsBadRequest_WhenInvalidPageNumberOrSize()
        {
            // Arrange
            var invalidPageNumber = -1; 
            var invalidPageSize = -1;  

            // Act
            var result = await _controller.GetFeedbacks(invalidPageNumber, invalidPageSize);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<FeedbackDto>>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Page number and page size must be greater than 0.", badRequestResult.Value);
            _mockFeedbackService.Verify(s => s.GetFeedbacksAsync(It.IsAny<int>(), It.IsAny<int>(), null, null), Times.Never);
        }
        [Fact]
        public async Task GetAll_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockFeedbackService
                .Setup(s => s.GetFeedbacksAsync(It.IsAny<int>(), It.IsAny<int>(), null, null))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetFeedbacks(1, 10);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PaginatedResult<FeedbackDto>>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Something went wrong", statusCodeResult.Value.ToString()); 
        }
        [Fact]
        public async Task GetAll_CallsGetFeedbacksAsync_WhenCalled()
        {
            // Arrange
            _mockFeedbackService
                .Setup(s => s.GetFeedbacksAsync(It.IsAny<int>(), It.IsAny<int>(), null, null))
                .ReturnsAsync(new PaginatedResult<FeedbackDto>
                {
                    Items = new List<FeedbackDto> { new FeedbackDto() },
                    TotalCount = 1
                });

            // Act
            var result = await _controller.GetFeedbacks(1, 10);

            // Assert
            _mockFeedbackService.Verify(s => s.GetFeedbacksAsync(It.IsAny<int>(), It.IsAny<int>(), null, null), Times.Once);
        }

        [Fact]
        public async Task GetFeedbackById_ReturnsOk_WhenFeedbackFound()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedbackDto = new FeedbackDto
            {
                FeedbackId = feedbackId,
                ReviewerName = "John Doe",
                Comment = "Great book!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true
            };

            _mockFeedbackService
                .Setup(s => s.GetFeedbackByIdAsync(feedbackId))
                .ReturnsAsync(feedbackDto);

            // Act
            var result = await _controller.GetFeedbackById(feedbackId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedFeedback = Assert.IsType<FeedbackDto>(okResult.Value);

            Assert.Equal(feedbackDto.FeedbackId, returnedFeedback.FeedbackId);
            Assert.Equal(feedbackDto.ReviewerName, returnedFeedback.ReviewerName);
            Assert.Equal(feedbackDto.Comment, returnedFeedback.Comment);
            Assert.Equal(feedbackDto.Rating, returnedFeedback.Rating);
            Assert.Equal(feedbackDto.Date, returnedFeedback.Date);
            Assert.Equal(feedbackDto.IsPurchased, returnedFeedback.IsPurchased);

            _mockFeedbackService.Verify(s => s.GetFeedbackByIdAsync(feedbackId), Times.Once);
        }


        [Fact]
        public async Task GetFeedbackById_ReturnsNotFound_WhenFeedbackNotFound()
        {
            // Arrange
            var FeedbackId = Guid.NewGuid();
            _mockFeedbackService
                .Setup(s => s.GetFeedbackByIdAsync(FeedbackId))
                .ReturnsAsync((FeedbackDto)null);

            // Act
            var result = await _controller.GetFeedbackById(FeedbackId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal($"Feedback with id {FeedbackId} not found.", notFoundResult.Value);
            _mockFeedbackService.Verify(s => s.GetFeedbackByIdAsync(FeedbackId), Times.Once); 
        }

        [Fact]
        public async Task GetFeedbackById_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var FeedbackId = Guid.NewGuid();
            _mockFeedbackService
                .Setup(s => s.GetFeedbackByIdAsync(FeedbackId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetFeedbackById(FeedbackId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());  
            _mockFeedbackService.Verify(s => s.GetFeedbackByIdAsync(FeedbackId), Times.Once); 
        }
        
        [Fact]
        public async Task UpdateFeedback_ReturnsBadRequest_WhenInvalidDataProvided()
        {
            // Arrange
            var FeedbackId = Guid.NewGuid();
            FeedbackDto FeedbackDto = null;

            // Act
            var result = await _controller.UpdateFeedback(FeedbackId, FeedbackDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }
        [Fact]
        public async Task CreateFeedback_ReturnsBadRequest_WhenInvalidDataProvided()
        {
            // Arrange
            FeedbackDto FeedbackDto = null;

            // Act
            var result = await _controller.CreateFeedback(FeedbackDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateFeedback_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var feedbackDto = new FeedbackDto
            {
                ReviewerName = "Test Reviewer",
                Comment = "Great book!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true
            };

            _mockFeedbackService
                .Setup(s => s.CreateFeedbackAsync(feedbackDto))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.CreateFeedback(feedbackDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Something went wrong", statusCodeResult.Value.ToString());

            _mockFeedbackService.Verify(s => s.CreateFeedbackAsync(feedbackDto), Times.Once);
        }

        [Fact]
        public async Task UpdateFeedback_ReturnsOk_WhenFeedbackUpdated()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedbackDto = new FeedbackDto
            {
                ReviewerName = "Updated Reviewer",
                Comment = "Updated comment",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = false
            };

            var updatedFeedback = new FeedbackDto
            {
                FeedbackId = feedbackId,
                ReviewerName = "Updated Reviewer",
                Comment = "Updated comment",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = false
            };

            _mockFeedbackService
                .Setup(s => s.UpdateFeedbackAsync(feedbackId, feedbackDto))
                .ReturnsAsync(updatedFeedback);

            // Act
            var result = await _controller.UpdateFeedback(feedbackId, feedbackDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedFeedback = Assert.IsType<FeedbackDto>(okResult.Value);

            Assert.Equal(updatedFeedback.FeedbackId, returnedFeedback.FeedbackId);
            Assert.Equal(updatedFeedback.ReviewerName, returnedFeedback.ReviewerName);
            Assert.Equal(updatedFeedback.Comment, returnedFeedback.Comment);
            Assert.Equal(updatedFeedback.Rating, returnedFeedback.Rating);
            Assert.Equal(updatedFeedback.IsPurchased, returnedFeedback.IsPurchased);

            _mockFeedbackService.Verify(s => s.UpdateFeedbackAsync(feedbackId, feedbackDto), Times.Once);
        }

        [Fact]
        public async Task UpdateFeedback_ReturnsNotFound_WhenFeedbackNotFound()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedbackDto = new FeedbackDto
            {
                ReviewerName = "Updated Reviewer",
                Comment = "Updated comment",
                Rating = 3,
                Date = DateTime.UtcNow,
                IsPurchased = true
            };

            _mockFeedbackService
                .Setup(s => s.UpdateFeedbackAsync(feedbackId, feedbackDto))
                .ReturnsAsync((FeedbackDto)null);

            // Act
            var result = await _controller.UpdateFeedback(feedbackId, feedbackDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal($"Feedback with id {feedbackId} not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateFeedback_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedbackDto = new FeedbackDto
            {
                ReviewerName = "Updated Reviewer",
                Comment = "Updated comment",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = false
            };

            _mockFeedbackService
                .Setup(s => s.UpdateFeedbackAsync(feedbackId, feedbackDto))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.UpdateFeedback(feedbackId, feedbackDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Something went wrong", statusCodeResult.Value.ToString());

            _mockFeedbackService.Verify(s => s.UpdateFeedbackAsync(feedbackId, feedbackDto), Times.Once);
        }

        [Fact]
        public async Task CreateFeedback_ReturnsCreatedAtAction_WhenFeedbackCreated()
        {
            // Arrange
            var feedbackDto = new FeedbackDto
            {
                ReviewerName = "Test Reviewer",
                Comment = "Great book!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true
            };

            var createdFeedback = new FeedbackDto
            {
                FeedbackId = Guid.NewGuid(),
                ReviewerName = "Test Reviewer",
                Comment = "Great book!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true
            };

            _mockFeedbackService
                .Setup(s => s.CreateFeedbackAsync(feedbackDto))
                .ReturnsAsync(createdFeedback);

            // Act
            var result = await _controller.CreateFeedback(feedbackDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnedFeedback = Assert.IsType<FeedbackDto>(createdResult.Value);

            Assert.Equal(createdFeedback.FeedbackId, returnedFeedback.FeedbackId);
            Assert.Equal(createdFeedback.ReviewerName, returnedFeedback.ReviewerName);
            Assert.Equal(createdFeedback.Comment, returnedFeedback.Comment);
            Assert.Equal(createdFeedback.Rating, returnedFeedback.Rating);
            Assert.Equal(createdFeedback.IsPurchased, returnedFeedback.IsPurchased);
            Assert.Equal("GetFeedbackById", createdResult.ActionName);

            _mockFeedbackService.Verify(s => s.CreateFeedbackAsync(feedbackDto), Times.Once);
        }


        //[Fact]
        //public async Task CreateFeedback_ReturnsInternalServerError_WhenExceptionOccurs()
        //{
        //    // Arrange
        //    var FeedbackDto = new FeedbackDto { Name = "Test Feedback" };
        //    _mockFeedbackService
        //        .Setup(s => s.CreateFeedbackAsync(FeedbackDto))
        //        .ThrowsAsync(new Exception("Something went wrong"));

        //    // Act
        //    var result = await _controller.CreateFeedback(FeedbackDto);

        //    // Assert
        //    var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
        //    var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
        //    Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        //    Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());
        //    _mockFeedbackService.Verify(s => s.CreateFeedbackAsync(FeedbackDto), Times.Once);
        //}
        //[Fact]
        //public async Task UpdateFeedback_ReturnsOk_WhenFeedbackUpdated()
        //{
        //    // Arrange
        //    var FeedbackId = Guid.NewGuid();
        //    var FeedbackDto = new FeedbackDto { Name = "Updated Feedback" };
        //    var updatedFeedback = new FeedbackDto { FeedbackId = FeedbackId, Name = "Updated Feedback" };

        //    _mockFeedbackService
        //        .Setup(s => s.UpdateFeedbackAsync(FeedbackId, FeedbackDto))
        //        .ReturnsAsync(updatedFeedback);

        //    // Act
        //    var result = await _controller.UpdateFeedback(FeedbackId, FeedbackDto);

        //    // Assert
        //    var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
        //    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        //    var returnedFeedback = Assert.IsType<FeedbackDto>(okResult.Value);
        //    Assert.Equal(updatedFeedback.FeedbackId, returnedFeedback.FeedbackId);
        //    Assert.Equal(updatedFeedback.Name, returnedFeedback.Name);
        //    _mockFeedbackService.Verify(s => s.UpdateFeedbackAsync(FeedbackId, FeedbackDto), Times.Once);
        //}

        //[Fact]
        //public async Task UpdateFeedback_ReturnsNotFound_WhenFeedbackNotFound()
        //{
        //    // Arrange
        //    var FeedbackId = Guid.NewGuid();
        //    var FeedbackDto = new FeedbackDto { Name = "Updated Feedback" };
        //    _mockFeedbackService
        //        .Setup(s => s.UpdateFeedbackAsync(FeedbackId, FeedbackDto))
        //        .ReturnsAsync((FeedbackDto)null);

        //    // Act
        //    var result = await _controller.UpdateFeedback(FeedbackId, FeedbackDto);

        //    // Assert
        //    var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
        //    var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        //    Assert.Equal("Feedback not found.", notFoundResult.Value);
        //}
        //[Fact]
        //public async Task UpdateFeedback_ReturnsInternalServerError_WhenExceptionOccurs()
        //{
        //    // Arrange
        //    var FeedbackId = Guid.NewGuid();
        //    var FeedbackDto = new FeedbackDto { Name = "Updated Feedback" };
        //    _mockFeedbackService
        //        .Setup(s => s.UpdateFeedbackAsync(FeedbackId, FeedbackDto))
        //        .ThrowsAsync(new Exception("Something went wrong"));

        //    // Act
        //    var result = await _controller.UpdateFeedback(FeedbackId, FeedbackDto);

        //    // Assert
        //    var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
        //    var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
        //    Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        //    Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());
        //    _mockFeedbackService.Verify(s => s.UpdateFeedbackAsync(FeedbackId, FeedbackDto), Times.Once);
        //}
        //[Fact]
        //public async Task CreateFeedback_ReturnsCreatedAtAction_WhenFeedbackCreated()
        //{
        //    // Arrange
        //    var FeedbackDto = new FeedbackDto { Name = "Test Feedback" };
        //    var createdFeedback = new FeedbackDto { FeedbackId = Guid.NewGuid(), Name = "Test Feedback" };
        //    _mockFeedbackService
        //        .Setup(s => s.CreateFeedbackAsync(FeedbackDto))
        //        .ReturnsAsync(createdFeedback);

        //    // Act
        //    var result = await _controller.CreateFeedback(FeedbackDto);

        //    // Assert
        //    var actionResult = Assert.IsType<ActionResult<FeedbackDto>>(result);
        //    var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        //    var returnedFeedback = Assert.IsType<FeedbackDto>(createdResult.Value);
        //    Assert.Equal(createdFeedback.FeedbackId, returnedFeedback.FeedbackId);
        //    Assert.Equal(createdFeedback.Name, returnedFeedback.Name);
        //    Assert.Equal("GetFeedbackById", createdResult.ActionName);
        //    _mockFeedbackService.Verify(s => s.CreateFeedbackAsync(FeedbackDto), Times.Once);
        //}

        [Fact]
        public async Task DeleteFeedback_ReturnsNoContent_WhenFeedbackDeleted()
        {
            // Arrange
            var FeedbackId = Guid.NewGuid();
            _mockFeedbackService
                .Setup(s => s.DeleteFeedbackAsync(FeedbackId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteFeedback(FeedbackId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            _mockFeedbackService.Verify(s => s.DeleteFeedbackAsync(FeedbackId), Times.Once);
        }
        [Fact]
        public async Task DeleteFeedback_ReturnsNotFound_WhenFeedbackNotFound()
        {
            // Arrange
            var FeedbackId = Guid.NewGuid();
            _mockFeedbackService
                .Setup(s => s.DeleteFeedbackAsync(FeedbackId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteFeedback(FeedbackId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Feedback with id {FeedbackId} not found.", notFoundResult.Value);
        }
        [Fact]
        public async Task DeleteFeedback_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var FeedbackId = Guid.NewGuid();
            _mockFeedbackService
                .Setup(s => s.DeleteFeedbackAsync(FeedbackId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.DeleteFeedback(FeedbackId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains("Internal server error: Something went wrong", statusCodeResult.Value.ToString());
            _mockFeedbackService.Verify(s => s.DeleteFeedbackAsync(FeedbackId), Times.Once);
        }


















    }
}
