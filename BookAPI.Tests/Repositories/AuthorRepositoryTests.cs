using Microsoft.EntityFrameworkCore;
using BookApi.Data;
using BookApi.Models;
using BookAPI.Repositories;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Tests.Repositories
{
    public class AuthorRepositoryTests
    {
        private readonly BookDbContext _dbContext;
        private readonly AuthorRepository _repository;

        public AuthorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new BookDbContext(options);
            _repository = new AuthorRepository(_dbContext);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPaginatedAuthors()
        {
            var authors = new List<Author>
            {
                new Author { Id = Guid.NewGuid(), Name = "Author 1" },
                new Author { Id = Guid.NewGuid(), Name = "Author 2" },
                new Author { Id = Guid.NewGuid(), Name = "Author 3" }
            };

            _dbContext.Authors.AddRange(authors);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync(1, 2);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.PageSize);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoAuthorsExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookDbContext>()
                          .UseInMemoryDatabase(databaseName: "TestDb")
                          .Options;

            using var context = new BookDbContext(options);
            var authors = await context.Authors.ToListAsync();
            context.Authors.RemoveRange(authors);
            await context.SaveChangesAsync();

            var repository = new AuthorRepository(context);

            // Act
            var result = await repository.GetAllAsync(1, 10);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAuthorToDatabase()
        {
            // Arrange
            var author = new Author
            {
                Name = "New Author",
                Biography = "New Biography"
            };

            // Act
            await _repository.CreateAsync(author);
            var addedAuthor = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == "New Author");

            // Assert
            Assert.NotNull(addedAuthor);
            Assert.Equal("New Author", addedAuthor.Name);
            Assert.Equal("New Biography", addedAuthor.Biography);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenAuthorIsNull()
        {
            // Arrange
            Author? author = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.CreateAsync(author));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAuthorDetails()
        {
            // Arrange
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Old Author"
            };

            _dbContext.Authors.Add(author);
            await _dbContext.SaveChangesAsync();

            author.Name = "Updated Author";

            // Act
            await _repository.UpdateAsync(author);
            var updatedAuthor = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == author.Id);

            // Assert
            Assert.NotNull(updatedAuthor);
            Assert.Equal("Updated Author", updatedAuthor.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var nonExistentAuthor = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Non-Existent Author"
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.UpdateAsync(nonExistentAuthor));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAuthorFromDatabase()
        {
            // Arrange
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Author to Delete"
            };

            _dbContext.Authors.Add(author);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(author.Id);
            var deletedAuthor = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == author.Id);

            // Assert
            Assert.Null(deletedAuthor);
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
        public async Task GetByIdAsync_ShouldReturnAuthorWhenExists()
        {
            // Arrange
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "Author for GetById"
            };

            _dbContext.Authors.Add(author);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(author.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Author for GetById", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullWhenAuthorDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(nonExistentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.DeleteAsync(nonExistentId));
        }
    }
}
