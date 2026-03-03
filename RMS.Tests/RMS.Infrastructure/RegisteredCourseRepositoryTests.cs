using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Infrastructure.Data;
using RMS.Infrastructure.Repositories;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Tests;

public class RegisteredCourseRepositoryTests
{
    private readonly Mock<ILogger<RegisteredCourseRepository>> _mockLogger;
    private readonly DbContextOptions<RmsDbContext> _options;

    public RegisteredCourseRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<RegisteredCourseRepository>>();
        _options = new DbContextOptionsBuilder<RmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void RegisteredCourseRepository_Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new RegisteredCourseRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void RegisteredCourseRepository_Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange
        using var context = new RmsDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new RegisteredCourseRepository(context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllActiveRegisteredCourses()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        var rc1 = new RegisteredCourse { StudentId = 1, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        var rc2 = new RegisteredCourse { StudentId = 2, Program = "CS", Level = "200", Session = "2023", IsActive = true };
        var rc3 = new RegisteredCourse { StudentId = 3, Program = "CS", Level = "300", Session = "2023", IsActive = false };

        context.RegisteredCourses.AddRange(rc1, rc2, rc3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, rc => rc.StudentId == 1);
        Assert.Contains(result, rc => rc.StudentId == 2);
        Assert.DoesNotContain(result, rc => rc.StudentId == 3);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRegisteredCourse_WhenExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        var registeredCourse = new RegisteredCourse { StudentId = 1, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        context.RegisteredCourses.Add(registeredCourse);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(registeredCourse.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(registeredCourse.Id, result.Id);
        Assert.Equal(1, result.StudentId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByStudentIdAsync_ShouldReturnRegisteredCourse_WhenExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        var registeredCourse = new RegisteredCourse { StudentId = 100, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        context.RegisteredCourses.Add(registeredCourse);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByStudentIdAsync(100);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.StudentId);
    }

    [Fact]
    public async Task GetByStudentIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByStudentIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddRegisteredCourseAndReturnIt()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        var registeredCourse = new RegisteredCourse
        {
            StudentId = 50,
            Program = "Engineering",
            Level = "200",
            Session = "2023/2024",
            CourseId1 = "ENG201",
            DateCreated = DateTime.Now,
            IsActive = true
        };

        // Act
        var result = await repository.AddAsync(registeredCourse);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(50, result.StudentId);
        Assert.Equal(1, await context.RegisteredCourses.CountAsync());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateRegisteredCourse()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        var registeredCourse = new RegisteredCourse { StudentId = 1, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        context.RegisteredCourses.Add(registeredCourse);
        await context.SaveChangesAsync();

        // Act
        registeredCourse.Program = "Engineering";
        await repository.UpdateAsync(registeredCourse);

        // Assert
        var updated = await context.RegisteredCourses.FindAsync(registeredCourse.Id);
        Assert.NotNull(updated);
        Assert.Equal("Engineering", updated.Program);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveFalse()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        var registeredCourse = new RegisteredCourse { StudentId = 1, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        context.RegisteredCourses.Add(registeredCourse);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(registeredCourse.Id);

        // Assert
        var deleted = await context.RegisteredCourses.FindAsync(registeredCourse.Id);
        Assert.NotNull(deleted);
        Assert.False(deleted.IsActive);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        var registeredCourse = new RegisteredCourse { StudentId = 1, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        context.RegisteredCourses.Add(registeredCourse);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(registeredCourse.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new RegisteredCourseRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }
}
