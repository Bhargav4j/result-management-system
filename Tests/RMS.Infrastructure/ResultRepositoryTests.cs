using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Infrastructure.Data;
using RMS.Infrastructure.Repositories;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Tests;

public class ResultRepositoryTests
{
    private readonly Mock<ILogger<ResultRepository>> _mockLogger;
    private readonly DbContextOptions<RmsDbContext> _options;

    public ResultRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<ResultRepository>>();
        _options = new DbContextOptionsBuilder<RmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void ResultRepository_Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ResultRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void ResultRepository_Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange
        using var context = new RmsDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ResultRepository(context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllActiveResults()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        var result1 = new Result { StudentId = 1, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        var result2 = new Result { StudentId = 2, Program = "CS", Level = "200", Session = "2023", IsActive = true };
        var result3 = new Result { StudentId = 3, Program = "CS", Level = "300", Session = "2023", IsActive = false };

        context.Results.AddRange(result1, result2, result3);
        await context.SaveChangesAsync();

        // Act
        var results = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, results.Count());
        Assert.Contains(results, r => r.StudentId == 1);
        Assert.Contains(results, r => r.StudentId == 2);
        Assert.DoesNotContain(results, r => r.StudentId == 3);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnResult_WhenExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        var result = new Result { StudentId = 1, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        context.Results.Add(result);
        await context.SaveChangesAsync();

        // Act
        var retrieved = await repository.GetByIdAsync(result.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(result.Id, retrieved.Id);
        Assert.Equal(1, retrieved.StudentId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByStudentIdAsync_ShouldReturnResult_WhenExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        var result = new Result { StudentId = 100, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        context.Results.Add(result);
        await context.SaveChangesAsync();

        // Act
        var retrieved = await repository.GetByStudentIdAsync(100);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(100, retrieved.StudentId);
    }

    [Fact]
    public async Task GetByStudentIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByStudentIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddResultAndReturnIt()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        var result = new Result
        {
            StudentId = 50,
            Program = "Engineering",
            Level = "200",
            Session = "2023/2024",
            CourseId1 = "ENG201",
            Score1 = "85",
            Grade1 = "A",
            DateCreated = DateTime.Now,
            IsActive = true
        };

        // Act
        var added = await repository.AddAsync(result);

        // Assert
        Assert.NotNull(added);
        Assert.True(added.Id > 0);
        Assert.Equal(50, added.StudentId);
        Assert.Equal(1, await context.Results.CountAsync());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateResult()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        var result = new Result { StudentId = 1, Program = "CS", Level = "100", Session = "2023", Score1 = "70", IsActive = true };
        context.Results.Add(result);
        await context.SaveChangesAsync();

        // Act
        result.Score1 = "90";
        await repository.UpdateAsync(result);

        // Assert
        var updated = await context.Results.FindAsync(result.Id);
        Assert.NotNull(updated);
        Assert.Equal("90", updated.Score1);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveFalse()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        var result = new Result { StudentId = 1, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        context.Results.Add(result);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(result.Id);

        // Assert
        var deleted = await context.Results.FindAsync(result.Id);
        Assert.NotNull(deleted);
        Assert.False(deleted.IsActive);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        var result = new Result { StudentId = 1, Program = "CS", Level = "100", Session = "2023", IsActive = true };
        context.Results.Add(result);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.ExistsAsync(result.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new ResultRepository(context, _mockLogger.Object);

        // Act
        var exists = await repository.ExistsAsync(999);

        // Assert
        Assert.False(exists);
    }
}
