using Xunit;
using RMS.Domain.Entities;

namespace RMS.Domain.Tests;

public class ResultTests
{
    [Fact]
    public void Result_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var result = new Result();

        // Assert
        Assert.Equal(0, result.Id);
        Assert.Equal(0, result.StudentId);
        Assert.Equal(string.Empty, result.Program);
        Assert.Equal(string.Empty, result.Level);
        Assert.Equal(string.Empty, result.Session);
        Assert.Null(result.CourseId1);
        Assert.Null(result.Score1);
        Assert.Null(result.Grade1);
        Assert.Null(result.CourseId2);
        Assert.Null(result.Score2);
        Assert.Null(result.Grade2);
        Assert.Null(result.CourseId3);
        Assert.Null(result.Score3);
        Assert.Null(result.Grade3);
        Assert.Equal(default(DateTime), result.DateCreated);
        Assert.True(result.IsActive);
    }

    [Fact]
    public void Result_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var result = new Result();
        var expectedDate = DateTime.Now;

        // Act
        result.Id = 1;
        result.StudentId = 100;
        result.Program = "Computer Science";
        result.Level = "100";
        result.Session = "2023/2024";
        result.CourseId1 = "CS101";
        result.Score1 = "85";
        result.Grade1 = "A";
        result.CourseId2 = "CS102";
        result.Score2 = "90";
        result.Grade2 = "A";
        result.CourseId3 = "CS103";
        result.Score3 = "78";
        result.Grade3 = "B";
        result.DateCreated = expectedDate;
        result.IsActive = true;

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal(100, result.StudentId);
        Assert.Equal("Computer Science", result.Program);
        Assert.Equal("100", result.Level);
        Assert.Equal("2023/2024", result.Session);
        Assert.Equal("CS101", result.CourseId1);
        Assert.Equal("85", result.Score1);
        Assert.Equal("A", result.Grade1);
        Assert.Equal("CS102", result.CourseId2);
        Assert.Equal("90", result.Score2);
        Assert.Equal("A", result.Grade2);
        Assert.Equal("CS103", result.CourseId3);
        Assert.Equal("78", result.Score3);
        Assert.Equal("B", result.Grade3);
        Assert.Equal(expectedDate, result.DateCreated);
        Assert.True(result.IsActive);
    }

    [Fact]
    public void Result_SetIsActiveFalse_ShouldReturnFalse()
    {
        // Arrange
        var result = new Result { IsActive = false };

        // Act & Assert
        Assert.False(result.IsActive);
    }

    [Fact]
    public void Result_CourseFieldsCanBeNull()
    {
        // Arrange & Act
        var result = new Result
        {
            StudentId = 1,
            Program = "Engineering",
            CourseId1 = null,
            Score1 = null,
            Grade1 = null
        };

        // Assert
        Assert.Null(result.CourseId1);
        Assert.Null(result.Score1);
        Assert.Null(result.Grade1);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("F")]
    public void Result_SetGrades_ShouldAcceptDifferentValues(string grade)
    {
        // Arrange & Act
        var result = new Result { Grade1 = grade };

        // Assert
        Assert.Equal(grade, result.Grade1);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("50")]
    [InlineData("100")]
    public void Result_SetScores_ShouldAcceptDifferentValues(string score)
    {
        // Arrange & Act
        var result = new Result { Score1 = score };

        // Assert
        Assert.Equal(score, result.Score1);
    }

    [Fact]
    public void Result_SetMultipleCourses_ShouldStoreAllCourses()
    {
        // Arrange & Act
        var result = new Result
        {
            CourseId1 = "CS101",
            CourseId2 = "CS102",
            CourseId3 = "CS103"
        };

        // Assert
        Assert.Equal("CS101", result.CourseId1);
        Assert.Equal("CS102", result.CourseId2);
        Assert.Equal("CS103", result.CourseId3);
    }

    [Fact]
    public void Result_SetStudentIdToZero_ShouldStoreValue()
    {
        // Arrange & Act
        var result = new Result { StudentId = 0 };

        // Assert
        Assert.Equal(0, result.StudentId);
    }

    [Fact]
    public void Result_SetEmptyStrings_ShouldStoreEmptyValues()
    {
        // Arrange & Act
        var result = new Result
        {
            Program = "",
            Level = "",
            Session = ""
        };

        // Assert
        Assert.Equal(string.Empty, result.Program);
        Assert.Equal(string.Empty, result.Level);
        Assert.Equal(string.Empty, result.Session);
    }
}
