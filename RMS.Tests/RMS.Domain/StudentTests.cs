using Xunit;
using RMS.Domain.Entities;

namespace RMS.Domain.Tests;

public class StudentTests
{
    [Fact]
    public void Student_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var student = new Student();

        // Assert
        Assert.Equal(0, student.Id);
        Assert.Equal(string.Empty, student.LastName);
        Assert.Equal(string.Empty, student.FirstName);
        Assert.Null(student.MiddleName);
        Assert.Equal(string.Empty, student.Gender);
        Assert.Null(student.BirthDate);
        Assert.Null(student.Phone);
        Assert.Null(student.Email);
        Assert.Equal(string.Empty, student.Program);
        Assert.Equal(string.Empty, student.Level);
        Assert.Equal(string.Empty, student.Session);
        Assert.Equal(default(DateTime), student.DateCreated);
        Assert.Equal(string.Empty, student.UserCreated);
        Assert.True(student.IsActive);
    }

    [Fact]
    public void Student_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var student = new Student();
        var expectedDate = DateTime.Now;

        // Act
        student.Id = 1;
        student.FirstName = "John";
        student.LastName = "Doe";
        student.MiddleName = "M";
        student.Gender = "Male";
        student.BirthDate = new DateTime(2000, 1, 1);
        student.Phone = "1234567890";
        student.Email = "john.doe@example.com";
        student.Program = "Computer Science";
        student.Level = "100";
        student.Session = "2023/2024";
        student.DateCreated = expectedDate;
        student.UserCreated = "admin";
        student.IsActive = true;

        // Assert
        Assert.Equal(1, student.Id);
        Assert.Equal("John", student.FirstName);
        Assert.Equal("Doe", student.LastName);
        Assert.Equal("M", student.MiddleName);
        Assert.Equal("Male", student.Gender);
        Assert.Equal(new DateTime(2000, 1, 1), student.BirthDate);
        Assert.Equal("1234567890", student.Phone);
        Assert.Equal("john.doe@example.com", student.Email);
        Assert.Equal("Computer Science", student.Program);
        Assert.Equal("100", student.Level);
        Assert.Equal("2023/2024", student.Session);
        Assert.Equal(expectedDate, student.DateCreated);
        Assert.Equal("admin", student.UserCreated);
        Assert.True(student.IsActive);
    }

    [Fact]
    public void Student_SetIsActiveFalse_ShouldReturnFalse()
    {
        // Arrange
        var student = new Student { IsActive = false };

        // Act & Assert
        Assert.False(student.IsActive);
    }

    [Fact]
    public void Student_OptionalFieldsCanBeNull()
    {
        // Arrange & Act
        var student = new Student
        {
            FirstName = "Jane",
            LastName = "Smith",
            MiddleName = null,
            Phone = null,
            Email = null,
            BirthDate = null
        };

        // Assert
        Assert.Null(student.MiddleName);
        Assert.Null(student.Phone);
        Assert.Null(student.Email);
        Assert.Null(student.BirthDate);
    }

    [Fact]
    public void Student_SetNegativeId_ShouldStoreValue()
    {
        // Arrange & Act
        var student = new Student { Id = -1 };

        // Assert
        Assert.Equal(-1, student.Id);
    }

    [Fact]
    public void Student_SetEmptyStrings_ShouldStoreEmptyValues()
    {
        // Arrange & Act
        var student = new Student
        {
            FirstName = "",
            LastName = "",
            Gender = "",
            Program = "",
            Level = "",
            Session = "",
            UserCreated = ""
        };

        // Assert
        Assert.Equal(string.Empty, student.FirstName);
        Assert.Equal(string.Empty, student.LastName);
        Assert.Equal(string.Empty, student.Gender);
        Assert.Equal(string.Empty, student.Program);
        Assert.Equal(string.Empty, student.Level);
        Assert.Equal(string.Empty, student.Session);
        Assert.Equal(string.Empty, student.UserCreated);
    }

    [Theory]
    [InlineData("Male")]
    [InlineData("Female")]
    [InlineData("Other")]
    public void Student_SetGender_ShouldAcceptDifferentValues(string gender)
    {
        // Arrange & Act
        var student = new Student { Gender = gender };

        // Assert
        Assert.Equal(gender, student.Gender);
    }

    [Theory]
    [InlineData("100")]
    [InlineData("200")]
    [InlineData("300")]
    [InlineData("400")]
    public void Student_SetLevel_ShouldAcceptDifferentValues(string level)
    {
        // Arrange & Act
        var student = new Student { Level = level };

        // Assert
        Assert.Equal(level, student.Level);
    }
}
