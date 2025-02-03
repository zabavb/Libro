using Microsoft.EntityFrameworkCore;
using BookApi.Data;
using BookApi.Models;
using BookAPI.Repositories;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Tests.Repositories
{
    public class PublisherRepositoryTests
    {
        private readonly BookDbContext _dbContext;
        private readonly PublisherRepository _repository;

        public PublisherRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new BookDbContext(options);
            _repository = new PublisherRepository(_dbContext);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPaginatedPublishers()
        {
            var publishers = new List<Publisher>
            {
                new Publisher { Id = Guid.NewGuid(), Name = "Publisher 1" },
                new Publisher { Id = Guid.NewGuid(), Name = "Publisher 2" },
                new Publisher { Id = Guid.NewGuid(), Name = "Publisher 3" }
            };

            _dbContext.Publishers.AddRange(publishers);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _repository.GetAllAsync(1, 2); 

            // Assert
            Assert.Equal(2, result.Items.Count);  
            Assert.Equal(1, result.PageNumber);  
            Assert.Equal(2, result.PageSize);   
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoPublishersExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookDbContext>()
                          .UseInMemoryDatabase(databaseName: "TestDb")
                          .Options;

            using var context = new BookDbContext(options);
            var publishers = await context.Publishers.ToListAsync();
            context.Publishers.RemoveRange(publishers);
            await context.SaveChangesAsync();

            var repository = new PublisherRepository(context);

            // Act
            var result = await repository.GetAllAsync(1, 10);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
        }


        [Fact]
        public async Task CreateAsync_ShouldAddPublisherToDatabase()
        {
            // Arrange
            var publisher = new Publisher
            {
                Name = "New Publisher",
                Description = "New Description"
            };

            // Act
            await _repository.CreateAsync(publisher);
            var addedPublisher = await _dbContext.Publishers.FirstOrDefaultAsync(p => p.Name == "New Publisher");

            // Assert
            Assert.NotNull(addedPublisher); 
            Assert.Equal("New Publisher", addedPublisher.Name);
            Assert.Equal("New Description", addedPublisher.Description);
        }
        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenPublisherIsNull()
        {
            // Arrange
            Publisher? publisher = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.CreateAsync(publisher));
        }


        [Fact]
        public async Task UpdateAsync_ShouldUpdatePublisherDetails()
        {
            // Arrange
            var publisher = new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Old Publisher"
            };

            _dbContext.Publishers.Add(publisher);
            await _dbContext.SaveChangesAsync();

            publisher.Name = "Updated Publisher";

            // Act
            await _repository.UpdateAsync(publisher);
            var updatedPublisher = await _dbContext.Publishers.FirstOrDefaultAsync(p => p.Id == publisher.Id);

            // Assert
            Assert.NotNull(updatedPublisher);  
            Assert.Equal("Updated Publisher", updatedPublisher.Name);  
        }
        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenPublisherDoesNotExist()
        {
            // Arrange
            var nonExistentPublisher = new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Non-Existent Publisher"
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.UpdateAsync(nonExistentPublisher));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemovePublisherFromDatabase()
        {
            // Arrange
            var publisher = new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Publisher to Delete"
            };

            _dbContext.Publishers.Add(publisher);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(publisher.Id);
            var deletedPublisher = await _dbContext.Publishers.FirstOrDefaultAsync(p => p.Id == publisher.Id);

            // Assert
            Assert.Null(deletedPublisher);  
        }
        [Fact]
        public async Task DeleteAsync_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Arrange
            var emptyId = Guid.Empty;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _repository.DeleteAsync(emptyId));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPublisherWhenExists()
        {
            // Arrange
            var publisher = new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Publisher for GetById"
            };

            _dbContext.Publishers.Add(publisher);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(publisher.Id);

            // Assert
            Assert.NotNull(result);  
            Assert.Equal("Publisher for GetById", result.Name); 
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullWhenPublisherDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(nonExistentId);

            // Assert
            Assert.Null(result); 
        }
        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenPublisherDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.DeleteAsync(nonExistentId));  
        }


    }
}
