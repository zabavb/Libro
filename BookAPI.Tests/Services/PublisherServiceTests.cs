using AutoMapper;
using BookApi.Models;
using BookApi.Services;
using BookAPI.Repositories;
using Library.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PublisherDto = Library.DTOs.Book.Publisher;


namespace BookAPI.Tests.Services
{
    public class PublisherServiceTests
    {
        private readonly Mock<IPublisherRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PublisherService _publisherService;

        public PublisherServiceTests()
        {
            _repositoryMock = new Mock<IPublisherRepository>();
            _mapperMock = new Mock<IMapper>();
            _publisherService = new PublisherService(_mapperMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task CreatePublisherAsync_ShouldReturnPublisherDto_WhenPublisherIsCreated()
        {
            // Arrange
            var publisherDto = new PublisherDto { Name = "Publisher 1", Description = "Description 1" };
            var publisher = new Publisher { Id = Guid.NewGuid(), Name = "Publisher 1", Description = "Description 1" };

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Publisher>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<Publisher>(It.IsAny<PublisherDto>())).Returns(publisher);
            _mapperMock.Setup(m => m.Map<PublisherDto>(It.IsAny<Publisher>())).Returns(publisherDto);

            // Act
            var result = await _publisherService.CreatePublisherAsync(publisherDto);

            // Assert
            Assert.NotNull(result); 
            Assert.Equal(publisherDto.PublisherId, result.PublisherId); 
            Assert.Equal(publisherDto.Name, result.Name);
            Assert.Equal(publisherDto.Description, result.Description);

            _repositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Publisher>()), Times.Once);
        }
        [Fact]
        public async Task CreatePublisherAsync_ShouldReturnNull_WhenPublisherDtoIsInvalid()
        {
            // Arrange
            var invalidPublisherDto = new PublisherDto { PublisherId = Guid.Empty, Name = "", Description = "" };

            // Act
            var result = await _publisherService.CreatePublisherAsync(invalidPublisherDto);

            // Assert
            Assert.Null(result); 
        }


        [Fact]
        public async Task DeletePublisherAsync_ShouldReturnTrue_WhenPublisherIsDeleted()
        {
            // Arrange
            var publisher = new Publisher { Id = Guid.NewGuid(), Name = "Publisher 1" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(publisher);
            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            var result = await _publisherService.DeletePublisherAsync(publisher.Id);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeletePublisherAsync_ShouldReturnFalse_WhenPublisherNotFound()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Publisher)null);

            // Act
            var result = await _publisherService.DeletePublisherAsync(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetPublishersAsync_ShouldReturnPaginatedPublishers()
        {
            // Arrange
            var publishers = new List<Publisher>
            {
                new Publisher { Id = Guid.NewGuid(), Name = "Publisher 1" },
                new Publisher { Id = Guid.NewGuid(), Name = "Publisher 2" }
            };
            var paginatedResult = new PaginatedResult<Publisher>
            {
                Items = publishers,
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 2
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedResult);
            _mapperMock.Setup(m => m.Map<ICollection<PublisherDto>>(It.IsAny<ICollection<Publisher>>()))
                .Returns(new List<PublisherDto> { new PublisherDto { Name = "Publisher 1" }, new PublisherDto { Name = "Publisher 2" } });

            // Act
            var result = await _publisherService.GetPublishersAsync(1, 2);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(2, result.PageSize);
            _repositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_ShouldReturnPublisher_WhenExists()
        {
            // Arrange
            var publisher = new Publisher { Id = Guid.NewGuid(), Name = "Publisher 1" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(publisher);
            _mapperMock.Setup(m => m.Map<PublisherDto>(It.IsAny<Publisher>())).Returns(new PublisherDto { Name = "Publisher 1" });

            // Act
            var result = await _publisherService.GetPublisherByIdAsync(publisher.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Publisher 1", result.Name);
            _repositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_ShouldReturnNull_WhenPublisherDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Publisher)null);

            // Act
            var result = await _publisherService.GetPublisherByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdatePublisherAsync_ShouldReturnUpdatedPublisher_WhenPublisherIsUpdated()
        {
            // Arrange
            var publisher = new Publisher { Id = Guid.NewGuid(), Name = "Publisher 1", Description = "Description 1" };
            var publisherDto = new PublisherDto { PublisherId = publisher.Id, Name = "Updated Publisher", Description = "Updated Description" };

            _mapperMock.Setup(m => m.Map<Publisher>(It.Is<PublisherDto>(dto => dto.PublisherId == publisherDto.PublisherId)))
                       .Returns(publisher);

            _mapperMock.Setup(m => m.Map<PublisherDto>(It.Is<Publisher>(p => p.Id == publisher.Id)))
                       .Returns(publisherDto);

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Publisher>())).Returns(Task.CompletedTask);

            var createdPublisher = await _publisherService.CreatePublisherAsync(publisherDto);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.Is<Guid>(id => id == createdPublisher.PublisherId)))
                           .ReturnsAsync(publisher);

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.Is<Publisher>(p => p.Id == publisher.Id)))
                   .Returns(Task.CompletedTask);
            // Act
            var result = await _publisherService.UpdatePublisherAsync(createdPublisher.PublisherId, publisherDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Publisher", result.Name);
            Assert.Equal("Updated Description", result.Description);
            _repositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Publisher>(p => p.Id == publisher.Id)), Times.Once);

        }

        [Fact]
        public async Task UpdatePublisherAsync_ShouldReturnNull_WhenPublisherNotFound()
        {
            // Arrange
            var publisherDto = new PublisherDto { Name = "Updated Publisher" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Publisher)null);

            // Act
            var result = await _publisherService.UpdatePublisherAsync(Guid.NewGuid(), publisherDto);

            // Assert
            Assert.Null(result);
        }
    }
}
