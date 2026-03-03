using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Infrastructure.Data;
using RMS.Infrastructure.Repositories;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Tests;

public class CourseRepositoryTests
{
    private readonly Mock<ILogger<CourseRepository>> _mockLogger;
    private readonly DbContextOptions<RmsDbContext> _options;

    public CourseRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<CourseRepository>>();
        _options = new DbContextOptionsBuilder<RmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void CourseRepository_Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CourseRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void CourseRepository_Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange
        using var context = new RmsDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CourseRepository(context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllActiveCourses()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        var course1 = new Course { CourseTitle = "Course 1", CourseLevel = 100, IsActive = true };
        var course2 = new Course { CourseTitle = "Course 2", CourseLevel = 200, IsActive = true };
        var course3 = new Course { CourseTitle = "Course 3", CourseLevel = 300, IsActive = false };

        context.Courses.AddRange(course1, course2, course3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.CourseTitle == "Course 1");
        Assert.Contains(result, c => c.CourseTitle == "Course 2");
        Assert.DoesNotContain(result, c => c.CourseTitle == "Course 3");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCourse_WhenCourseExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        var course = new Course { CourseTitle = "Test Course", CourseLevel = 100, IsActive = true };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal("Test Course", result.CourseTitle);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCourseDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCourseAndReturnIt()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        var course = new Course
        {
            CourseTitle = "New Course",
            CourseLevel = 300,
            DateCreated = DateTime.Now,
            IsActive = true
        };

        // Act
        var result = await repository.AddAsync(course);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("New Course", result.CourseTitle);
        Assert.Equal(1, await context.Courses.CountAsync());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCourse()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        var course = new Course { CourseTitle = "Original Title", CourseLevel = 100, IsActive = true };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        // Act
        course.CourseTitle = "Updated Title";
        await repository.UpdateAsync(course);

        // Assert
        var updatedCourse = await context.Courses.FindAsync(course.Id);
        Assert.NotNull(updatedCourse);
        Assert.Equal("Updated Title", updatedCourse.CourseTitle);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveFalse()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        var course = new Course { CourseTitle = "Test Course", CourseLevel = 100, IsActive = true };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(course.Id);

        // Assert
        var deletedCourse = await context.Courses.FindAsync(course.Id);
        Assert.NotNull(deletedCourse);
        Assert.False(deletedCourse.IsActive);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenCourseExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        var course = new Course { CourseTitle = "Test Course", CourseLevel = 100, IsActive = true };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(course.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenCourseDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingCourses()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        var course1 = new Course { CourseTitle = "Data Structures", CourseLevel = 200, IsActive = true };
        var course2 = new Course { CourseTitle = "Database Systems", CourseLevel = 300, IsActive = true };
        var course3 = new Course { CourseTitle = "Algorithms", CourseLevel = 300, IsActive = true };

        context.Courses.AddRange(course1, course2, course3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Data");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.CourseTitle == "Data Structures");
        Assert.Contains(result, c => c.CourseTitle == "Database Systems");
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyList_WhenNoMatch()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new CourseRepository(context, _mockLogger.Object);

        var course = new Course { CourseTitle = "Test Course", CourseLevel = 100, IsActive = true };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("NonExistent");

        // Assert
        Assert.Empty(result);
    }
}
