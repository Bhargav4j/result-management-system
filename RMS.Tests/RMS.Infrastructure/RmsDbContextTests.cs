using Xunit;
using Microsoft.EntityFrameworkCore;
using RMS.Infrastructure.Data;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Tests;

public class RmsDbContextTests
{
    private DbContextOptions<RmsDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<RmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void RmsDbContext_Constructor_ShouldInitializeSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);

        // Assert
        Assert.NotNull(context);
        Assert.NotNull(context.Students);
        Assert.NotNull(context.Courses);
        Assert.NotNull(context.Results);
        Assert.NotNull(context.RegisteredCourses);
    }

    [Fact]
    public void RmsDbContext_Students_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var students = context.Students;

        // Assert
        Assert.NotNull(students);
    }

    [Fact]
    public void RmsDbContext_Courses_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var courses = context.Courses;

        // Assert
        Assert.NotNull(courses);
    }

    [Fact]
    public void RmsDbContext_Results_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var results = context.Results;

        // Assert
        Assert.NotNull(results);
    }

    [Fact]
    public void RmsDbContext_RegisteredCourses_ShouldBeAccessible()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var registeredCourses = context.RegisteredCourses;

        // Assert
        Assert.NotNull(registeredCourses);
    }

    [Fact]
    public async Task RmsDbContext_AddStudent_ShouldSaveSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new RmsDbContext(options);

        var student = new Student
        {
            FirstName = "Test",
            LastName = "User",
            Gender = "Male",
            Program = "CS",
            Level = "100",
            Session = "2023/2024",
            UserCreated = "admin",
            DateCreated = DateTime.Now,
            IsActive = true
        };

        // Act
        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(1, await context.Students.CountAsync());
    }

    [Fact]
    public async Task RmsDbContext_AddCourse_ShouldSaveSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new RmsDbContext(options);

        var course = new Course
        {
            CourseTitle = "Test Course",
            CourseLevel = 100,
            DateCreated = DateTime.Now,
            IsActive = true
        };

        // Act
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(1, await context.Courses.CountAsync());
    }

    [Fact]
    public async Task RmsDbContext_AddResult_ShouldSaveSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new RmsDbContext(options);

        var result = new Result
        {
            StudentId = 1,
            Program = "CS",
            Level = "100",
            Session = "2023/2024",
            CourseId1 = "CS101",
            Score1 = "85",
            Grade1 = "A",
            DateCreated = DateTime.Now,
            IsActive = true
        };

        // Act
        context.Results.Add(result);
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(1, await context.Results.CountAsync());
    }

    [Fact]
    public async Task RmsDbContext_AddRegisteredCourse_ShouldSaveSuccessfully()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new RmsDbContext(options);

        var registeredCourse = new RegisteredCourse
        {
            StudentId = 1,
            Program = "CS",
            Level = "100",
            Session = "2023/2024",
            CourseId1 = "CS101",
            DateCreated = DateTime.Now,
            IsActive = true
        };

        // Act
        context.RegisteredCourses.Add(registeredCourse);
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(1, await context.RegisteredCourses.CountAsync());
    }

    [Fact]
    public async Task RmsDbContext_UpdateStudent_ShouldSaveChanges()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new RmsDbContext(options);

        var student = new Student
        {
            FirstName = "Original",
            LastName = "Name",
            Gender = "Male",
            Program = "CS",
            Level = "100",
            Session = "2023/2024",
            UserCreated = "admin",
            DateCreated = DateTime.Now,
            IsActive = true
        };

        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Act
        student.FirstName = "Updated";
        await context.SaveChangesAsync();

        // Assert
        var updatedStudent = await context.Students.FirstAsync();
        Assert.Equal("Updated", updatedStudent.FirstName);
    }

    [Fact]
    public async Task RmsDbContext_DeleteStudent_ShouldRemoveFromDatabase()
    {
        // Arrange
        var options = CreateNewContextOptions();
        using var context = new RmsDbContext(options);

        var student = new Student
        {
            FirstName = "Test",
            LastName = "User",
            Gender = "Male",
            Program = "CS",
            Level = "100",
            Session = "2023/2024",
            UserCreated = "admin",
            DateCreated = DateTime.Now,
            IsActive = true
        };

        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Act
        context.Students.Remove(student);
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(0, await context.Students.CountAsync());
    }
}
