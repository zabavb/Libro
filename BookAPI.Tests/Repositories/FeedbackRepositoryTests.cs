using Microsoft.EntityFrameworkCore;
using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Repositories;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Tests.Repositories
{
    public class FeedbackRepositoryTests
    {
        private readonly BookDbContext _dbContext;
        private readonly FeedbackRepository _repository;

        public FeedbackRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new BookDbContext(options);
            _repository = new FeedbackRepository(_dbContext);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPaginatedFeedbacks()
        {
            var feedbacks = new List<Feedback>
            {
                new Feedback { Id = Guid.NewGuid(), Comment = "Feedback 1", Rating = 5 },
                new Feedback { Id = Guid.NewGuid(), Comment = "Feedback 2", Rating = 4 },
                new Feedback { Id = Guid.NewGuid(), Comment = "Feedback 3", Rating = 3 }
            };

            _dbContext.Feedbacks.AddRange(feedbacks);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetAllAsync(1, 2, null);

            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.PageSize);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoFeedbacksExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookDbContext>()
                          .UseInMemoryDatabase(databaseName: "TestDb")
                          .Options;

            using var context = new BookDbContext(options);
            var Feedbacks = await context.Feedbacks.ToListAsync();
            context.Feedbacks.RemoveRange(Feedbacks);
            await context.SaveChangesAsync();

            var repository = new FeedbackRepository(context);

            // Act
            var result = await repository.GetAllAsync(1, 10, null);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddFeedbackToDatabase()
        {
            var feedback = new Feedback
            {
                Comment = "New Feedback",
                Rating = 5
            };

            await _repository.CreateAsync(feedback);
            var addedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Comment == "New Feedback");

            Assert.NotNull(addedFeedback);
            Assert.Equal("New Feedback", addedFeedback.Comment);
            Assert.Equal(5, addedFeedback.Rating);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenFeedbackIsNull()
        {
            Feedback? feedback = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.CreateAsync(feedback));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateFeedbackDetails()
        {
            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                Comment = "Old Feedback",
                Rating = 3
            };

            _dbContext.Feedbacks.Add(feedback);
            await _dbContext.SaveChangesAsync();

            feedback.Comment = "Updated Feedback";
            feedback.Rating = 4;

            await _repository.UpdateAsync(feedback);
            var updatedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedback.Id);

            Assert.NotNull(updatedFeedback);
            Assert.Equal("Updated Feedback", updatedFeedback.Comment);
            Assert.Equal(4, updatedFeedback.Rating);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenFeedbackDoesNotExist()
        {
            var nonExistentFeedback = new Feedback
            {
                Id = Guid.NewGuid(),
                Comment = "Non-Existent Feedback"
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.UpdateAsync(nonExistentFeedback));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveFeedbackFromDatabase()
        {
            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                Comment = "Feedback to Delete"
            };

            _dbContext.Feedbacks.Add(feedback);
            await _dbContext.SaveChangesAsync();

            await _repository.DeleteAsync(feedback.Id);
            var deletedFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedback.Id);

            Assert.Null(deletedFeedback);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            var emptyId = Guid.Empty;
            await Assert.ThrowsAsync<ArgumentException>(() => _repository.DeleteAsync(emptyId));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFeedbackWhenExists()
        {
            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                Comment = "Feedback for GetById"
            };

            _dbContext.Feedbacks.Add(feedback);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(feedback.Id);

            Assert.NotNull(result);
            Assert.Equal("Feedback for GetById", result.Comment);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullWhenFeedbackDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            var result = await _repository.GetByIdAsync(nonExistentId);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenFeedbackDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.DeleteAsync(nonExistentId));
        }
    }
}
