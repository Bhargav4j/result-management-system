using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Services;
using RMS.Web.Pages.Students;

namespace RMS.Web.Tests;

public class StudentsIndexModelTests
{
    private readonly Mock<IStudentService> _mockStudentService;
    private readonly Mock<ILogger<IndexModel>> _mockLogger;
    private readonly IndexModel _model;

    public StudentsIndexModelTests()
    {
        _mockStudentService = new Mock<IStudentService>();
        _mockLogger = new Mock<ILogger<IndexModel>>();
        _model = new IndexModel(_mockStudentService.Object, _mockLogger.Object);
    }

    [Fact]
    public void StudentsIndexModel_Constructor_ShouldInitializeSuccessfully()
    {
        // Arrange & Act
        var model = new IndexModel(_mockStudentService.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(model);
        Assert.NotNull(model.Students);
        Assert.Empty(model.Students);
    }

    [Fact]
    public async Task OnGetAsync_ShouldLoadAllStudents_WhenNoSearchString()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = 1, FirstName = "John", LastName = "Doe" },
            new Student { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };
        _mockStudentService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);

        // Act
        await _model.OnGetAsync();

        // Assert
        Assert.Equal(2, _model.Students.Count());
        _mockStudentService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OnGetAsync_ShouldSearchStudents_WhenSearchStringProvided()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = 1, FirstName = "John", LastName = "Doe" }
        };
        _mockStudentService.Setup(s => s.SearchAsync("John", It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);
        _model.SearchString = "John";

        // Act
        await _model.OnGetAsync();

        // Assert
        Assert.Single(_model.Students);
        _mockStudentService.Verify(s => s.SearchAsync("John", It.IsAny<CancellationToken>()), Times.Once);
        _mockStudentService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task OnGetAsync_ShouldAddModelError_WhenExceptionThrown()
    {
        // Arrange
        _mockStudentService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        await _model.OnGetAsync();

        // Assert
        Assert.False(_model.ModelState.IsValid);
        Assert.True(_model.ModelState.ErrorCount > 0);
    }

    [Fact]
    public async Task OnGetAsync_ShouldNotSearchWithEmptyString()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = 1, FirstName = "John", LastName = "Doe" }
        };
        _mockStudentService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);
        _model.SearchString = "";

        // Act
        await _model.OnGetAsync();

        // Assert
        _mockStudentService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockStudentService.Verify(s => s.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task OnGetAsync_ShouldNotSearchWithWhitespaceString()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = 1, FirstName = "John", LastName = "Doe" }
        };
        _mockStudentService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);
        _model.SearchString = "   ";

        // Act
        await _model.OnGetAsync();

        // Assert
        _mockStudentService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockStudentService.Verify(s => s.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
