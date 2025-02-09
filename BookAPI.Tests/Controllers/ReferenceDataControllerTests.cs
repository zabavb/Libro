using BookAPI.Controllers;
using BookAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
namespace BookAPI.Tests.Controllers
{
    public class ReferenceDataControllerTests
    {
        private readonly Mock<ICoverTypeService> _coverTypeServiceMock;
        private readonly Mock<ILanguageService> _languageServiceMock;
        private readonly Mock<ILogger<ReferenceDataController>> _loggerMock;
        private readonly ReferenceDataController _controller;

        public ReferenceDataControllerTests()
        {
            _coverTypeServiceMock = new Mock<ICoverTypeService>();
            _languageServiceMock = new Mock<ILanguageService>();
            _loggerMock = new Mock<ILogger<ReferenceDataController>>();
            _controller = new ReferenceDataController(
                _coverTypeServiceMock.Object,
                _languageServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetCoverTypes_ReturnsOk_WhenCoverTypesExist()
        {
            // Arrange
            var coverTypes = new List<string> { "Hardcover", "Paperback" };
            _coverTypeServiceMock.Setup(service => service.GetCoverTypesAsync())
                .ReturnsAsync(coverTypes);

            // Act
            var result = await _controller.GetCoverTypes();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(coverTypes, okResult.Value);
        }

        [Fact]
        public async Task GetCoverTypes_ReturnsNotFound_WhenNoCoverTypes()
        {
            // Arrange
            _coverTypeServiceMock.Setup(service => service.GetCoverTypesAsync())
                .ReturnsAsync((List<string>)null);

            // Act
            var result = await _controller.GetCoverTypes();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetCoverType_ReturnsOk_WhenCoverTypeExists()
        {
            // Arrange
            var coverType = "Hardcover";
            _coverTypeServiceMock.Setup(service => service.GetCoverTypeByIdAsync(1))
                .ReturnsAsync(coverType);

            // Act
            var result = await _controller.GetCoverType(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(coverType, okResult.Value);
        }

        [Fact]
        public async Task GetCoverType_ReturnsNotFound_WhenCoverTypeNotFound()
        {
            // Arrange
            _coverTypeServiceMock.Setup(service => service.GetCoverTypeByIdAsync(1))
                .ReturnsAsync((string)null);

            // Act
            var result = await _controller.GetCoverType(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetLanguages_ReturnsOk_WhenLanguagesExist()
        {
            // Arrange
            var languages = new List<string> { "English", "Spanish" };
            _languageServiceMock.Setup(service => service.GetLanguagesAsync())
                .ReturnsAsync(languages);

            // Act
            var result = await _controller.GetLanguages();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(languages, okResult.Value);
        }

        [Fact]
        public async Task GetLanguages_ReturnsNotFound_WhenNoLanguages()
        {
            // Arrange
            _languageServiceMock.Setup(service => service.GetLanguagesAsync())
                .ReturnsAsync((List<string>)null);

            // Act
            var result = await _controller.GetLanguages();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetLanguage_ReturnsOk_WhenLanguageExists()
        {
            // Arrange
            var language = "English";
            _languageServiceMock.Setup(service => service.GetLanguageByIdAsync(1))
                .ReturnsAsync(language);

            // Act
            var result = await _controller.GetLanguage(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(language, okResult.Value);
        }

        [Fact]
        public async Task GetLanguage_ReturnsNotFound_WhenLanguageNotFound()
        {
            // Arrange
            _languageServiceMock.Setup(service => service.GetLanguageByIdAsync(1))
                .ReturnsAsync((string)null);

            // Act
            var result = await _controller.GetLanguage(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetCoverTypes_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _coverTypeServiceMock.Setup(service => service.GetCoverTypesAsync())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetCoverTypes();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            Assert.IsType<ObjectResult>(actionResult.Result);
            var objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetLanguage_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _languageServiceMock.Setup(service => service.GetLanguageByIdAsync(1))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetLanguage(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            Assert.IsType<ObjectResult>(actionResult.Result);
            var objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(500, objectResult.StatusCode);
        }
    }

}
