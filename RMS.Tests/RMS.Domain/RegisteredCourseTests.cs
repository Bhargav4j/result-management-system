using Xunit;
using RMS.Domain.Entities;

namespace RMS.Domain.Tests;

public class RegisteredCourseTests
{
    [Fact]
    public void RegisteredCourse_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var registeredCourse = new RegisteredCourse();

        // Assert
        Assert.Equal(0, registeredCourse.Id);
        Assert.Equal(0, registeredCourse.StudentId);
        Assert.Equal(string.Empty, registeredCourse.Program);
        Assert.Equal(string.Empty, registeredCourse.Level);
        Assert.Equal(string.Empty, registeredCourse.Session);
        Assert.Null(registeredCourse.CourseId1);
        Assert.Null(registeredCourse.CourseId2);
        Assert.Null(registeredCourse.CourseId3);
        Assert.Null(registeredCourse.CourseId4);
        Assert.Null(registeredCourse.CourseId5);
        Assert.Null(registeredCourse.CourseId6);
        Assert.Null(registeredCourse.CourseId7);
        Assert.Null(registeredCourse.CourseId8);
        Assert.Null(registeredCourse.CourseId9);
        Assert.Null(registeredCourse.CourseId10);
        Assert.Equal(default(DateTime), registeredCourse.DateCreated);
        Assert.True(registeredCourse.IsActive);
    }

    [Fact]
    public void RegisteredCourse_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var registeredCourse = new RegisteredCourse();
        var expectedDate = DateTime.Now;

        // Act
        registeredCourse.Id = 1;
        registeredCourse.StudentId = 100;
        registeredCourse.Program = "Computer Science";
        registeredCourse.Level = "100";
        registeredCourse.Session = "2023/2024";
        registeredCourse.CourseId1 = "CS101";
        registeredCourse.CourseId2 = "CS102";
        registeredCourse.CourseId3 = "CS103";
        registeredCourse.CourseId4 = "CS104";
        registeredCourse.CourseId5 = "CS105";
        registeredCourse.DateCreated = expectedDate;
        registeredCourse.IsActive = true;

        // Assert
        Assert.Equal(1, registeredCourse.Id);
        Assert.Equal(100, registeredCourse.StudentId);
        Assert.Equal("Computer Science", registeredCourse.Program);
        Assert.Equal("100", registeredCourse.Level);
        Assert.Equal("2023/2024", registeredCourse.Session);
        Assert.Equal("CS101", registeredCourse.CourseId1);
        Assert.Equal("CS102", registeredCourse.CourseId2);
        Assert.Equal("CS103", registeredCourse.CourseId3);
        Assert.Equal("CS104", registeredCourse.CourseId4);
        Assert.Equal("CS105", registeredCourse.CourseId5);
        Assert.Equal(expectedDate, registeredCourse.DateCreated);
        Assert.True(registeredCourse.IsActive);
    }

    [Fact]
    public void RegisteredCourse_SetIsActiveFalse_ShouldReturnFalse()
    {
        // Arrange
        var registeredCourse = new RegisteredCourse { IsActive = false };

        // Act & Assert
        Assert.False(registeredCourse.IsActive);
    }

    [Fact]
    public void RegisteredCourse_AllCourseFieldsCanBeNull()
    {
        // Arrange & Act
        var registeredCourse = new RegisteredCourse
        {
            StudentId = 1,
            Program = "Engineering",
            CourseId1 = null,
            CourseId2 = null,
            CourseId3 = null,
            CourseId4 = null,
            CourseId5 = null,
            CourseId6 = null,
            CourseId7 = null,
            CourseId8 = null,
            CourseId9 = null,
            CourseId10 = null
        };

        // Assert
        Assert.Null(registeredCourse.CourseId1);
        Assert.Null(registeredCourse.CourseId2);
        Assert.Null(registeredCourse.CourseId3);
        Assert.Null(registeredCourse.CourseId4);
        Assert.Null(registeredCourse.CourseId5);
        Assert.Null(registeredCourse.CourseId6);
        Assert.Null(registeredCourse.CourseId7);
        Assert.Null(registeredCourse.CourseId8);
        Assert.Null(registeredCourse.CourseId9);
        Assert.Null(registeredCourse.CourseId10);
    }

    [Fact]
    public void RegisteredCourse_SetAllTenCourses_ShouldStoreAllValues()
    {
        // Arrange & Act
        var registeredCourse = new RegisteredCourse
        {
            CourseId1 = "CS101",
            CourseId2 = "CS102",
            CourseId3 = "CS103",
            CourseId4 = "CS104",
            CourseId5 = "CS105",
            CourseId6 = "CS106",
            CourseId7 = "CS107",
            CourseId8 = "CS108",
            CourseId9 = "CS109",
            CourseId10 = "CS110"
        };

        // Assert
        Assert.Equal("CS101", registeredCourse.CourseId1);
        Assert.Equal("CS102", registeredCourse.CourseId2);
        Assert.Equal("CS103", registeredCourse.CourseId3);
        Assert.Equal("CS104", registeredCourse.CourseId4);
        Assert.Equal("CS105", registeredCourse.CourseId5);
        Assert.Equal("CS106", registeredCourse.CourseId6);
        Assert.Equal("CS107", registeredCourse.CourseId7);
        Assert.Equal("CS108", registeredCourse.CourseId8);
        Assert.Equal("CS109", registeredCourse.CourseId9);
        Assert.Equal("CS110", registeredCourse.CourseId10);
    }

    [Fact]
    public void RegisteredCourse_SetEmptyStrings_ShouldStoreEmptyValues()
    {
        // Arrange & Act
        var registeredCourse = new RegisteredCourse
        {
            Program = "",
            Level = "",
            Session = ""
        };

        // Assert
        Assert.Equal(string.Empty, registeredCourse.Program);
        Assert.Equal(string.Empty, registeredCourse.Level);
        Assert.Equal(string.Empty, registeredCourse.Session);
    }

    [Fact]
    public void RegisteredCourse_SetStudentIdToZero_ShouldStoreValue()
    {
        // Arrange & Act
        var registeredCourse = new RegisteredCourse { StudentId = 0 };

        // Assert
        Assert.Equal(0, registeredCourse.StudentId);
    }

    [Theory]
    [InlineData("100")]
    [InlineData("200")]
    [InlineData("300")]
    public void RegisteredCourse_SetLevel_ShouldAcceptDifferentValues(string level)
    {
        // Arrange & Act
        var registeredCourse = new RegisteredCourse { Level = level };

        // Assert
        Assert.Equal(level, registeredCourse.Level);
    }
}
