using BookApi.Models;
using BookAPI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookAPI.Tests.Services
{
    public class CoverTypeServiceTests
    {
        private readonly CoverTypeService _coverTypeService;

        public CoverTypeServiceTests()
        {
            _coverTypeService = new CoverTypeService();
        }

        [Fact]
        public async Task GetCoverTypesAsync_ReturnsAllCoverTypes()
        {
            // Arrange
            var expectedCoverTypes = Enum.GetNames(typeof(CoverType));

            // Act
            var result = await _coverTypeService.GetCoverTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCoverTypes, result);
        }

        [Fact]
        public async Task GetCoverTypeByIdAsync_ReturnsValidCoverType_WhenIdIsValid()
        {
            // Arrange
            var id = 1; 

            // Act
            var result = await _coverTypeService.GetCoverTypeByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Enum.GetName(typeof(CoverType), id), result);
        }

        [Fact]
        public async Task GetCoverTypeByIdAsync_ReturnsOther_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var result = await _coverTypeService.GetCoverTypeByIdAsync(invalidId);

            // Assert
            Assert.Equal(CoverType.OTHER.ToString(), result);
        }

        

        [Fact]
        public async Task GetCoverTypeByIdAsync_ReturnsOther_ForInvalidId()
        {
            // Arrange
            var invalidId = -1;

            // Act
            var result = await _coverTypeService.GetCoverTypeByIdAsync(invalidId);

            // Assert
            Assert.Equal(CoverType.OTHER.ToString(), result);
        }

       
    }
}
