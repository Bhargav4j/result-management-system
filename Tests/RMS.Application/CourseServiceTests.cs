using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using RMS.Application.Services;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Repositories;

namespace RMS.Application.Tests;

public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _mockRepository;
    private readonly Mock<ILogger<CourseService>> _mockLogger;
    private readonly CourseService _service;

    public CourseServiceTests()
    {
        _mockRepository = new Mock<ICourseRepository>();
        _mockLogger = new Mock<ILogger<CourseService>>();
        _service = new CourseService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCourses()
    {
        // Arrange
        var courses = new List<Course>
        {
            new Course { Id = 1, CourseTitle = "Course 1", CourseLevel = 100 },
            new Course { Id = 2, CourseTitle = "Course 2", CourseLevel = 200 }
        };
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(courses);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCourse_WhenExists()
    {
        // Arrange
        var course = new Course { Id = 1, CourseTitle = "Test Course", CourseLevel = 100 };
        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Course", result.CourseTitle);
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Course?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCourseWithDefaultValues()
    {
        // Arrange
        var course = new Course { CourseTitle = "New Course", CourseLevel = 300 };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        // Act
        var result = await _service.CreateAsync(course);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsActive);
        _mockRepository.Verify(r => r.AddAsync(It.Is<Course>(c => c.IsActive && c.DateCreated != default), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallRepositoryUpdate()
    {
        // Arrange
        var course = new Course { Id = 1, CourseTitle = "Updated Course", CourseLevel = 200 };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(course);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(course, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryDelete()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingCourses()
    {
        // Arrange
        var courses = new List<Course>
        {
            new Course { Id = 1, CourseTitle = "Data Structures", CourseLevel = 200 }
        };
        _mockRepository.Setup(r => r.SearchAsync("Data", It.IsAny<CancellationToken>()))
            .ReturnsAsync(courses);

        // Act
        var result = await _service.SearchAsync("Data");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _mockRepository.Verify(r => r.SearchAsync("Data", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowException_WhenRepositoryFails()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetAllAsync());
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenRepositoryFails()
    {
        // Arrange
        var course = new Course { CourseTitle = "Test Course", CourseLevel = 100 };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(course));
    }
}
