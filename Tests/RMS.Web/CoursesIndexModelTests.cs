using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Services;
using RMS.Web.Pages.Courses;

namespace RMS.Web.Tests;

public class CoursesIndexModelTests
{
    private readonly Mock<ICourseService> _mockCourseService;
    private readonly Mock<ILogger<IndexModel>> _mockLogger;
    private readonly IndexModel _model;

    public CoursesIndexModelTests()
    {
        _mockCourseService = new Mock<ICourseService>();
        _mockLogger = new Mock<ILogger<IndexModel>>();
        _model = new IndexModel(_mockCourseService.Object, _mockLogger.Object);
    }

    [Fact]
    public void CoursesIndexModel_Constructor_ShouldInitializeSuccessfully()
    {
        // Arrange & Act
        var model = new IndexModel(_mockCourseService.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(model);
        Assert.NotNull(model.Courses);
        Assert.Empty(model.Courses);
    }

    [Fact]
    public async Task OnGetAsync_ShouldLoadAllCourses()
    {
        // Arrange
        var courses = new List<Course>
        {
            new Course { Id = 1, CourseTitle = "Course 1", CourseLevel = 100 },
            new Course { Id = 2, CourseTitle = "Course 2", CourseLevel = 200 }
        };
        _mockCourseService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(courses);

        // Act
        await _model.OnGetAsync();

        // Assert
        Assert.Equal(2, _model.Courses.Count());
        _mockCourseService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OnGetAsync_ShouldAddModelError_WhenExceptionThrown()
    {
        // Arrange
        _mockCourseService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        await _model.OnGetAsync();

        // Assert
        Assert.False(_model.ModelState.IsValid);
        Assert.True(_model.ModelState.ErrorCount > 0);
    }

    [Fact]
    public async Task OnGetAsync_ShouldReturnEmptyList_WhenNoCoursesExist()
    {
        // Arrange
        _mockCourseService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Course>());

        // Act
        await _model.OnGetAsync();

        // Assert
        Assert.Empty(_model.Courses);
    }

    [Fact]
    public async Task OnGetAsync_ShouldHandleMultipleCourses()
    {
        // Arrange
        var courses = new List<Course>
        {
            new Course { Id = 1, CourseTitle = "Math", CourseLevel = 100 },
            new Course { Id = 2, CourseTitle = "Physics", CourseLevel = 200 },
            new Course { Id = 3, CourseTitle = "Chemistry", CourseLevel = 300 }
        };
        _mockCourseService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(courses);

        // Act
        await _model.OnGetAsync();

        // Assert
        Assert.Equal(3, _model.Courses.Count());
    }
}
