using AutoMapper;
using BookAPI.Models;
using BookAPI.Services;
using BookAPI.Repositories.Interfaces;
using Library.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AuthorDto = Library.DTOs.Book.Author;

namespace BookAPI.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _repositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _authorService = new AuthorService(_mapperMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task CreateAuthorAsync_ShouldReturnAuthorDto_WhenAuthorIsCreated()
        {
            // Arrange
            var authorDto = new AuthorDto { Name = "Author 1", Biography = "Biographygraphy 1" };
            var author = new Author { Id = Guid.NewGuid(), Name = "Author 1", Biography = "Biographygraphy 1" };

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Author>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<Author>(It.IsAny<AuthorDto>())).Returns(author);
            _mapperMock.Setup(m => m.Map<AuthorDto>(It.IsAny<Author>())).Returns(authorDto);

            // Act
            var result = await _authorService.CreateAuthorAsync(authorDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authorDto.AuthorId, result.AuthorId);
            Assert.Equal(authorDto.Name, result.Name);
            Assert.Equal(authorDto.Biography, result.Biography);

            _repositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public async Task CreateAuthorAsync_ShouldReturnNull_WhenAuthorDtoIsInvalid()
        {
            // Arrange
            var invalidAuthorDto = new AuthorDto { AuthorId = Guid.Empty, Name = "", Biography = "" };

            // Act
            var result = await _authorService.CreateAuthorAsync(invalidAuthorDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAuthorAsync_ShouldReturnTrue_WhenAuthorIsDeleted()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "Author 1" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(author);
            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            var result = await _authorService.DeleteAuthorAsync(author.Id);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthorAsync_ShouldReturnFalse_WhenAuthorNotFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Author)null);

            // Act
            var result = await _authorService.DeleteAuthorAsync(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAuthorsAsync_ShouldReturnPaginatedAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = Guid.NewGuid(), Name = "Author 1" },
                new Author { Id = Guid.NewGuid(), Name = "Author 2" }
            };
            var paginatedResult = new PaginatedResult<Author>
            {
                Items = authors,
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 2
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedResult);
            _mapperMock.Setup(m => m.Map<ICollection<AuthorDto>>(It.IsAny<ICollection<Author>>()))
                .Returns(new List<AuthorDto> { new AuthorDto { Name = "Author 1" }, new AuthorDto { Name = "Author 2" } });

            // Act
            var result = await _authorService.GetAuthorsAsync(1, 2);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.PageSize);
            _repositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ShouldReturnAuthor_WhenExists()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "Author 1" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(author);
            _mapperMock.Setup(m => m.Map<AuthorDto>(It.IsAny<Author>())).Returns(new AuthorDto { Name = "Author 1" });

            // Act
            var result = await _authorService.GetAuthorByIdAsync(author.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Author 1", result.Name);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ShouldReturnNull_WhenAuthorDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Author)null);

            // Act
            var result = await _authorService.GetAuthorByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAuthorAsync_ShouldReturnUpdatedAuthor_WhenAuthorIsUpdated()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "Author 1", Biography = "Biographygraphy 1" };
            var authorDto = new AuthorDto { AuthorId = author.Id, Name = "Updated Author", Biography = "Updated Biographygraphy" };

            _mapperMock.Setup(m => m.Map<Author>(It.Is<AuthorDto>(dto => dto.AuthorId == authorDto.AuthorId)))
                       .Returns(author);

            _mapperMock.Setup(m => m.Map<AuthorDto>(It.Is<Author>(a => a.Id == author.Id)))
                       .Returns(authorDto);

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Author>())).Returns(Task.CompletedTask);

            var createdAuthor = await _authorService.CreateAuthorAsync(authorDto);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.Is<Guid>(id => id == createdAuthor.AuthorId)))
                           .ReturnsAsync(author);

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.Is<Author>(a => a.Id == author.Id)))
                   .Returns(Task.CompletedTask);

            // Act
            var result = await _authorService.UpdateAuthorAsync(createdAuthor.AuthorId, authorDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Author", result.Name);
            Assert.Equal("Updated Biographygraphy", result.Biography);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Author>(a => a.Id == author.Id)), Times.Once);
        }

        [Fact]
        public async Task UpdateAuthorAsync_ShouldReturnNull_WhenAuthorNotFound()
        {
            // Arrange
            var authorDto = new AuthorDto { Name = "Updated Author" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Author)null);

            // Act
            var result = await _authorService.UpdateAuthorAsync(Guid.NewGuid(), authorDto);

            // Assert
            Assert.Null(result);
        }
    }
}
