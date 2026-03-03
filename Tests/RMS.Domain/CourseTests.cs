using Xunit;
using RMS.Domain.Entities;

namespace RMS.Domain.Tests;

public class CourseTests
{
    [Fact]
    public void Course_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var course = new Course();

        // Assert
        Assert.Equal(0, course.Id);
        Assert.Equal(string.Empty, course.CourseTitle);
        Assert.Equal(0, course.CourseLevel);
        Assert.Equal(default(DateTime), course.DateCreated);
        Assert.True(course.IsActive);
    }

    [Fact]
    public void Course_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var course = new Course();
        var expectedDate = DateTime.Now;

        // Act
        course.Id = 1;
        course.CourseTitle = "Introduction to Computer Science";
        course.CourseLevel = 100;
        course.DateCreated = expectedDate;
        course.IsActive = true;

        // Assert
        Assert.Equal(1, course.Id);
        Assert.Equal("Introduction to Computer Science", course.CourseTitle);
        Assert.Equal(100, course.CourseLevel);
        Assert.Equal(expectedDate, course.DateCreated);
        Assert.True(course.IsActive);
    }

    [Fact]
    public void Course_SetIsActiveFalse_ShouldReturnFalse()
    {
        // Arrange
        var course = new Course { IsActive = false };

        // Act & Assert
        Assert.False(course.IsActive);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(200)]
    [InlineData(300)]
    [InlineData(400)]
    public void Course_SetCourseLevel_ShouldAcceptDifferentValues(int level)
    {
        // Arrange & Act
        var course = new Course { CourseLevel = level };

        // Assert
        Assert.Equal(level, course.CourseLevel);
    }

    [Fact]
    public void Course_SetCourseTitleEmpty_ShouldStoreEmptyString()
    {
        // Arrange & Act
        var course = new Course { CourseTitle = "" };

        // Assert
        Assert.Equal(string.Empty, course.CourseTitle);
    }

    [Fact]
    public void Course_SetNegativeId_ShouldStoreValue()
    {
        // Arrange & Act
        var course = new Course { Id = -1 };

        // Assert
        Assert.Equal(-1, course.Id);
    }

    [Fact]
    public void Course_SetNegativeCourseLevel_ShouldStoreValue()
    {
        // Arrange & Act
        var course = new Course { CourseLevel = -100 };

        // Assert
        Assert.Equal(-100, course.CourseLevel);
    }

    [Fact]
    public void Course_SetLongCourseTitle_ShouldStoreValue()
    {
        // Arrange
        var longTitle = new string('A', 200);

        // Act
        var course = new Course { CourseTitle = longTitle };

        // Assert
        Assert.Equal(longTitle, course.CourseTitle);
    }

    [Theory]
    [InlineData("Mathematics")]
    [InlineData("Physics")]
    [InlineData("Chemistry")]
    [InlineData("Computer Science")]
    public void Course_SetCourseTitle_ShouldAcceptDifferentValues(string title)
    {
        // Arrange & Act
        var course = new Course { CourseTitle = title };

        // Assert
        Assert.Equal(title, course.CourseTitle);
    }

    [Fact]
    public void Course_SetZeroCourseLevel_ShouldStoreValue()
    {
        // Arrange & Act
        var course = new Course { CourseLevel = 0 };

        // Assert
        Assert.Equal(0, course.CourseLevel);
    }
}
