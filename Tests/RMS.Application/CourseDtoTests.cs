using Xunit;
using RMS.Application.DTOs;

namespace RMS.Application.Tests;

public class CourseDtoTests
{
    [Fact]
    public void CourseDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new CourseDto();

        // Assert
        Assert.Equal(0, dto.Id);
        Assert.Equal(string.Empty, dto.CourseTitle);
        Assert.Equal(0, dto.CourseLevel);
        Assert.Equal(default(DateTime), dto.DateCreated);
    }

    [Fact]
    public void CourseDto_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var dto = new CourseDto();
        var expectedDate = DateTime.Now;

        // Act
        dto.Id = 1;
        dto.CourseTitle = "Data Structures";
        dto.CourseLevel = 200;
        dto.DateCreated = expectedDate;

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("Data Structures", dto.CourseTitle);
        Assert.Equal(200, dto.CourseLevel);
        Assert.Equal(expectedDate, dto.DateCreated);
    }

    [Theory]
    [InlineData("Mathematics")]
    [InlineData("Physics")]
    [InlineData("Computer Science")]
    public void CourseDto_SetCourseTitle_ShouldAcceptDifferentValues(string title)
    {
        // Arrange & Act
        var dto = new CourseDto { CourseTitle = title };

        // Assert
        Assert.Equal(title, dto.CourseTitle);
    }
}

public class CourseCreateDtoTests
{
    [Fact]
    public void CourseCreateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new CourseCreateDto();

        // Assert
        Assert.Equal(string.Empty, dto.CourseTitle);
        Assert.Equal(0, dto.CourseLevel);
    }

    [Fact]
    public void CourseCreateDto_SetProperties_ShouldStoreValues()
    {
        // Arrange & Act
        var dto = new CourseCreateDto
        {
            CourseTitle = "Algorithms",
            CourseLevel = 300
        };

        // Assert
        Assert.Equal("Algorithms", dto.CourseTitle);
        Assert.Equal(300, dto.CourseLevel);
    }

    [Fact]
    public void CourseCreateDto_SetEmptyTitle_ShouldStoreEmptyString()
    {
        // Arrange & Act
        var dto = new CourseCreateDto { CourseTitle = "" };

        // Assert
        Assert.Equal(string.Empty, dto.CourseTitle);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(200)]
    [InlineData(300)]
    [InlineData(400)]
    public void CourseCreateDto_SetCourseLevel_ShouldAcceptDifferentValues(int level)
    {
        // Arrange & Act
        var dto = new CourseCreateDto { CourseLevel = level };

        // Assert
        Assert.Equal(level, dto.CourseLevel);
    }
}

public class CourseUpdateDtoTests
{
    [Fact]
    public void CourseUpdateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new CourseUpdateDto();

        // Assert
        Assert.Equal(string.Empty, dto.CourseTitle);
        Assert.Equal(0, dto.CourseLevel);
    }

    [Fact]
    public void CourseUpdateDto_SetProperties_ShouldStoreValues()
    {
        // Arrange & Act
        var dto = new CourseUpdateDto
        {
            CourseTitle = "Machine Learning",
            CourseLevel = 400
        };

        // Assert
        Assert.Equal("Machine Learning", dto.CourseTitle);
        Assert.Equal(400, dto.CourseLevel);
    }

    [Fact]
    public void CourseUpdateDto_SetNegativeLevel_ShouldStoreValue()
    {
        // Arrange & Act
        var dto = new CourseUpdateDto { CourseLevel = -100 };

        // Assert
        Assert.Equal(-100, dto.CourseLevel);
    }
}
