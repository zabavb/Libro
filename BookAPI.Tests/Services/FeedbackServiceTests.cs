using AutoMapper;
using BookApi.Models;
using BookAPI.Repositories;
using FeedbackApi.Services;
using Library.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedbackDto = Library.DTOs.Book.Feedback;


namespace BookAPI.Tests.Services
{
    public class FeedbackServiceTests
    {
        private readonly Mock<IFeedbackRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FeedbackService _feedbackService;

        public FeedbackServiceTests()
        {
            _repositoryMock = new Mock<IFeedbackRepository>();
            _mapperMock = new Mock<IMapper>();
            _feedbackService = new FeedbackService(_mapperMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task CreateFeedbackAsync_ShouldReturnFeedbackDto_WhenFeedbackIsCreated()
        {
            // Arrange
            var feedbackDto = new FeedbackDto { Comment = "Great service!", Rating = 5 };
            var feedback = new Feedback { Id = Guid.NewGuid(), Comment = "Great service!", Rating = 5 };

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Feedback>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<Feedback>(It.IsAny<FeedbackDto>())).Returns(feedback);
            _mapperMock.Setup(m => m.Map<FeedbackDto>(It.IsAny<Feedback>())).Returns(feedbackDto);

            // Act
            var result = await _feedbackService.CreateFeedbackAsync(feedbackDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(feedbackDto.Comment, result.Comment);
            Assert.Equal(feedbackDto.Rating, result.Rating);

            _repositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Feedback>()), Times.Once);
        }

        [Fact]
        public async Task CreateFeedbackAsync_ShouldReturnNull_WhenFeedbackDtoIsInvalid()
        {
            // Arrange
            var invalidFeedbackDto = new FeedbackDto { FeedbackId = Guid.Empty, Comment = "", Rating = 0 };

            // Act
            var result = await _feedbackService.CreateFeedbackAsync(invalidFeedbackDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteFeedbackAsync_ShouldReturnTrue_WhenFeedbackIsDeleted()
        {
            // Arrange
            var feedback = new Feedback { Id = Guid.NewGuid(), Comment = "Great service!" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(feedback);
            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            var result = await _feedbackService.DeleteFeedbackAsync(feedback.Id);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteFeedbackAsync_ShouldReturnFalse_WhenFeedbackNotFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Feedback)null);

            // Act
            var result = await _feedbackService.DeleteFeedbackAsync(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetFeedbacksAsync_ShouldReturnPaginatedFeedbacks()
        {
            // Arrange
            var feedbacks = new List<Feedback>
            {
                new Feedback { Id = Guid.NewGuid(), Comment = "Great service!" },
                new Feedback { Id = Guid.NewGuid(), Comment = "Very helpful." }
            };
            var paginatedResult = new PaginatedResult<Feedback>
            {
                Items = feedbacks,
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 2
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedResult);
            _mapperMock.Setup(m => m.Map<ICollection<FeedbackDto>>(It.IsAny<ICollection<Feedback>>()))
                .Returns(new List<FeedbackDto> { new FeedbackDto { Comment = "Great service!" }, new FeedbackDto { Comment = "Very helpful." } });

            // Act
            var result = await _feedbackService.GetFeedbacksAsync(1, 2);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.PageSize);
            _repositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetFeedbackByIdAsync_ShouldReturnFeedback_WhenExists()
        {
            // Arrange
            var feedback = new Feedback { Id = Guid.NewGuid(), Comment = "Great service!" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(feedback);
            _mapperMock.Setup(m => m.Map<FeedbackDto>(It.IsAny<Feedback>())).Returns(new FeedbackDto { Comment = "Great service!" });

            // Act
            var result = await _feedbackService.GetFeedbackByIdAsync(feedback.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Great service!", result.Comment);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetFeedbackByIdAsync_ShouldReturnNull_WhenFeedbackDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Feedback)null);

            // Act
            var result = await _feedbackService.GetFeedbackByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateFeedbackAsync_ShouldReturnUpdatedFeedback_WhenFeedbackIsUpdated()
        {
            // Arrange
            var feedback = new Feedback { Id = Guid.NewGuid(), Comment = "Great service!", Rating = 5 };
            var feedbackDto = new FeedbackDto { FeedbackId = feedback.Id, Comment = "Updated feedback", Rating = 4 };

            _mapperMock.Setup(m => m.Map<Feedback>(It.Is<FeedbackDto>(dto => dto.FeedbackId == feedbackDto.FeedbackId)))
                       .Returns(feedback);

            _mapperMock.Setup(m => m.Map<FeedbackDto>(It.Is<Feedback>(f => f.Id == feedback.Id)))
                       .Returns(feedbackDto);

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Feedback>())).Returns(Task.CompletedTask);

            var createdFeedback = await _feedbackService.CreateFeedbackAsync(feedbackDto);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.Is<Guid>(id => id == createdFeedback.FeedbackId)))
                           .ReturnsAsync(feedback);

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.Is<Feedback>(f => f.Id == feedback.Id)))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _feedbackService.UpdateFeedbackAsync(createdFeedback.FeedbackId, feedbackDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated feedback", result.Comment);
            Assert.Equal(4, result.Rating);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Feedback>(f => f.Id == feedback.Id)), Times.Once);
        }

        [Fact]
        public async Task UpdateFeedbackAsync_ShouldReturnNull_WhenFeedbackNotFound()
        {
            // Arrange
            var feedbackDto = new FeedbackDto { Comment = "Updated feedback" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Feedback)null);

            // Act
            var result = await _feedbackService.UpdateFeedbackAsync(Guid.NewGuid(), feedbackDto);

            // Assert
            Assert.Null(result);
        }
    }
}
