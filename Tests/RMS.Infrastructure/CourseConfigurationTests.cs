using Xunit;
using Microsoft.EntityFrameworkCore;
using RMS.Infrastructure.Data;
using RMS.Infrastructure.Data.Configurations;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Tests;

public class CourseConfigurationTests
{
    private DbContextOptions<RmsDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<RmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void CourseConfiguration_Configure_ShouldSetTableName()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Course));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("Courses", entityType.GetTableName());
    }

    [Fact]
    public void CourseConfiguration_Configure_ShouldHaveIdAsPrimaryKey()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Course));
        var primaryKey = entityType?.FindPrimaryKey();

        // Assert
        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal("Id", primaryKey.Properties.First().Name);
    }

    [Fact]
    public void CourseConfiguration_Configure_ShouldSetCourseTitleAsRequired()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Course));
        var courseTitleProperty = entityType?.FindProperty("CourseTitle");

        // Assert
        Assert.NotNull(courseTitleProperty);
        Assert.False(courseTitleProperty.IsNullable);
    }

    [Fact]
    public void CourseConfiguration_Configure_ShouldSetCourseTitleMaxLength()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Course));
        var courseTitleProperty = entityType?.FindProperty("CourseTitle");

        // Assert
        Assert.NotNull(courseTitleProperty);
        Assert.Equal(100, courseTitleProperty.GetMaxLength());
    }

    [Fact]
    public void CourseConfiguration_Configure_ShouldSetCourseLevelAsRequired()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Course));
        var courseLevelProperty = entityType?.FindProperty("CourseLevel");

        // Assert
        Assert.NotNull(courseLevelProperty);
        Assert.False(courseLevelProperty.IsNullable);
    }

    [Fact]
    public void CourseConfiguration_Instance_ShouldBeOfCorrectType()
    {
        // Arrange & Act
        var configuration = new CourseConfiguration();

        // Assert
        Assert.NotNull(configuration);
        Assert.IsAssignableFrom<IEntityTypeConfiguration<Course>>(configuration);
    }
}
