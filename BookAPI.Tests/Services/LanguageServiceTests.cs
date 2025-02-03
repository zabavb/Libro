using BookApi.Models;
using BookAPI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookAPI.Tests.Services
{
    public class LanguageServiceTests
    {
        private readonly LanguageService _languageService;

        public LanguageServiceTests()
        {
            _languageService = new LanguageService();
        }

        [Fact]
        public async Task GetLanguagesAsync_ReturnsAllLanguages()
        {
            // Arrange
            var expectedLanguages = Enum.GetNames(typeof(Language));

            // Act
            var result = await _languageService.GetLanguagesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLanguages, result);
        }

        [Fact]
        public async Task GetLanguageByIdAsync_ReturnsValidLanguage_WhenIdIsValid()
        {
            // Arrange
            var id = 1; 

            // Act
            var result = await _languageService.GetLanguageByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Enum.GetName(typeof(Language), id), result);
        }

        [Fact]
        public async Task GetLanguageByIdAsync_ReturnsOther_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = 999; 

            // Act
            var result = await _languageService.GetLanguageByIdAsync(invalidId);

            // Assert
            Assert.Equal(Language.OTHER.ToString(), result);
        }

       
        [Fact]
        public async Task GetLanguageByIdAsync_ReturnsExpectedValue_ForDifferentIds()
        {
            // Arrange
            var validId = 0;
            var expectedLanguage = Enum.GetName(typeof(Language), validId);

            // Act
            var result = await _languageService.GetLanguageByIdAsync(validId);

            Assert.Equal(expectedLanguage, result);

            expectedLanguage = Enum.GetName(typeof(Language), validId);
            result = await _languageService.GetLanguageByIdAsync(validId);
            Assert.Equal(expectedLanguage, result);
        }
    }
}
