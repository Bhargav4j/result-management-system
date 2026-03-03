using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Infrastructure.Data;
using RMS.Infrastructure.Repositories;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Tests;

public class StudentRepositoryTests
{
    private readonly Mock<ILogger<StudentRepository>> _mockLogger;
    private readonly DbContextOptions<RmsDbContext> _options;

    public StudentRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<StudentRepository>>();
        _options = new DbContextOptionsBuilder<RmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void StudentRepository_Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new StudentRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void StudentRepository_Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange
        using var context = new RmsDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new StudentRepository(context, null!));
    }

    [Fact]
    public void StudentRepository_Constructor_ShouldInitializeSuccessfully()
    {
        // Arrange
        using var context = new RmsDbContext(_options);

        // Act
        var repository = new StudentRepository(context, _mockLogger.Object);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllActiveStudents()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        var student1 = new Student { FirstName = "John", LastName = "Doe", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };
        var student2 = new Student { FirstName = "Jane", LastName = "Smith", Gender = "Female", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };
        var student3 = new Student { FirstName = "Bob", LastName = "Johnson", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = false };

        context.Students.AddRange(student1, student2, student3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, s => s.FirstName == "John");
        Assert.Contains(result, s => s.FirstName == "Jane");
        Assert.DoesNotContain(result, s => s.FirstName == "Bob");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnStudent_WhenStudentExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        var student = new Student { FirstName = "John", LastName = "Doe", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(student.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(student.Id, result.Id);
        Assert.Equal("John", result.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenStudentDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenStudentIsInactive()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        var student = new Student { FirstName = "John", LastName = "Doe", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = false };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(student.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddStudentAndReturnIt()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        var student = new Student
        {
            FirstName = "Alice",
            LastName = "Williams",
            Gender = "Female",
            Program = "Engineering",
            Level = "200",
            Session = "2023/2024",
            UserCreated = "admin",
            DateCreated = DateTime.Now,
            IsActive = true
        };

        // Act
        var result = await repository.AddAsync(student);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Alice", result.FirstName);
        Assert.Equal(1, await context.Students.CountAsync());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStudent()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        var student = new Student { FirstName = "Original", LastName = "Name", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Act
        student.FirstName = "Updated";
        await repository.UpdateAsync(student);

        // Assert
        var updatedStudent = await context.Students.FindAsync(student.Id);
        Assert.NotNull(updatedStudent);
        Assert.Equal("Updated", updatedStudent.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveFalse()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        var student = new Student { FirstName = "Test", LastName = "User", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(student.Id);

        // Assert
        var deletedStudent = await context.Students.FindAsync(student.Id);
        Assert.NotNull(deletedStudent);
        Assert.False(deletedStudent.IsActive);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDoNothing_WhenStudentDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        // Act
        await repository.DeleteAsync(999);

        // Assert - No exception thrown
        Assert.Equal(0, await context.Students.CountAsync());
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenStudentExists()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        var student = new Student { FirstName = "Test", LastName = "User", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(student.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenStudentDoesNotExist()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingStudents()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        var student1 = new Student { FirstName = "John", LastName = "Doe", Email = "john@example.com", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };
        var student2 = new Student { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Gender = "Female", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };
        var student3 = new Student { FirstName = "Bob", LastName = "Johnson", Email = "bob@example.com", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };

        context.Students.AddRange(student1, student2, student3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("John");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, s => s.FirstName == "John");
        Assert.Contains(result, s => s.LastName == "Johnson");
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyList_WhenNoMatch()
    {
        // Arrange
        using var context = new RmsDbContext(_options);
        var repository = new StudentRepository(context, _mockLogger.Object);

        var student = new Student { FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Male", Program = "CS", Level = "100", Session = "2023", UserCreated = "admin", IsActive = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("NonExistent");

        // Assert
        Assert.Empty(result);
    }
}
