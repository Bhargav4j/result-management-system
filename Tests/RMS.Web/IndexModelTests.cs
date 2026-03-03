using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using RMS.Web.Pages;

namespace RMS.Web.Tests;

public class IndexModelTests
{
    private readonly Mock<ILogger<IndexModel>> _mockLogger;
    private readonly IndexModel _model;

    public IndexModelTests()
    {
        _mockLogger = new Mock<ILogger<IndexModel>>();
        _model = new IndexModel(_mockLogger.Object);
    }

    [Fact]
    public void IndexModel_Constructor_ShouldInitializeSuccessfully()
    {
        // Arrange & Act
        var model = new IndexModel(_mockLogger.Object);

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void IndexModel_OnGet_ShouldExecuteWithoutException()
    {
        // Arrange & Act
        _model.OnGet();

        // Assert - No exception thrown
        Assert.NotNull(_model);
    }

    [Fact]
    public void IndexModel_Constructor_ShouldAcceptLogger()
    {
        // Arrange
        var logger = new Mock<ILogger<IndexModel>>();

        // Act
        var model = new IndexModel(logger.Object);

        // Assert
        Assert.NotNull(model);
    }
}
