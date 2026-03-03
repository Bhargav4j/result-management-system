using Xunit;
using Microsoft.EntityFrameworkCore;
using RMS.Infrastructure.Data;
using RMS.Infrastructure.Data.Configurations;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Tests;

public class StudentConfigurationTests
{
    private DbContextOptions<RmsDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<RmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void StudentConfiguration_Configure_ShouldSetTableName()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Student));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("Students", entityType.GetTableName());
    }

    [Fact]
    public void StudentConfiguration_Configure_ShouldHaveIdAsPrimaryKey()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Student));
        var primaryKey = entityType?.FindPrimaryKey();

        // Assert
        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal("Id", primaryKey.Properties.First().Name);
    }

    [Fact]
    public void StudentConfiguration_Configure_ShouldSetRequiredFieldsCorrectly()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Student));

        var lastNameProperty = entityType?.FindProperty("LastName");
        var firstNameProperty = entityType?.FindProperty("FirstName");
        var genderProperty = entityType?.FindProperty("Gender");
        var programProperty = entityType?.FindProperty("Program");
        var levelProperty = entityType?.FindProperty("Level");
        var sessionProperty = entityType?.FindProperty("Session");
        var userCreatedProperty = entityType?.FindProperty("UserCreated");

        // Assert
        Assert.NotNull(lastNameProperty);
        Assert.False(lastNameProperty.IsNullable);
        Assert.NotNull(firstNameProperty);
        Assert.False(firstNameProperty.IsNullable);
        Assert.NotNull(genderProperty);
        Assert.False(genderProperty.IsNullable);
        Assert.NotNull(programProperty);
        Assert.False(programProperty.IsNullable);
        Assert.NotNull(levelProperty);
        Assert.False(levelProperty.IsNullable);
        Assert.NotNull(sessionProperty);
        Assert.False(sessionProperty.IsNullable);
        Assert.NotNull(userCreatedProperty);
        Assert.False(userCreatedProperty.IsNullable);
    }

    [Fact]
    public void StudentConfiguration_Configure_ShouldSetMaxLengthForStringFields()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Student));

        var lastNameProperty = entityType?.FindProperty("LastName");
        var firstNameProperty = entityType?.FindProperty("FirstName");

        // Assert
        Assert.NotNull(lastNameProperty);
        Assert.Equal(50, lastNameProperty.GetMaxLength());
        Assert.NotNull(firstNameProperty);
        Assert.Equal(50, firstNameProperty.GetMaxLength());
    }

    [Fact]
    public void StudentConfiguration_Configure_ShouldAllowNullableFields()
    {
        // Arrange
        var options = CreateNewContextOptions();

        // Act
        using var context = new RmsDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Student));

        var middleNameProperty = entityType?.FindProperty("MiddleName");
        var phoneProperty = entityType?.FindProperty("Phone");
        var emailProperty = entityType?.FindProperty("Email");
        var birthDateProperty = entityType?.FindProperty("BirthDate");

        // Assert
        Assert.NotNull(middleNameProperty);
        Assert.True(middleNameProperty.IsNullable);
        Assert.NotNull(phoneProperty);
        Assert.True(phoneProperty.IsNullable);
        Assert.NotNull(emailProperty);
        Assert.True(emailProperty.IsNullable);
        Assert.NotNull(birthDateProperty);
        Assert.True(birthDateProperty.IsNullable);
    }

    [Fact]
    public void StudentConfiguration_Instance_ShouldBeOfCorrectType()
    {
        // Arrange & Act
        var configuration = new StudentConfiguration();

        // Assert
        Assert.NotNull(configuration);
        Assert.IsAssignableFrom<IEntityTypeConfiguration<Student>>(configuration);
    }
}
