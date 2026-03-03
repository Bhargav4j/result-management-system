using Xunit;
using RMS.Application.DTOs;

namespace RMS.Application.Tests;

public class StudentDtoTests
{
    [Fact]
    public void StudentDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new StudentDto();

        // Assert
        Assert.Equal(0, dto.Id);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Null(dto.MiddleName);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Null(dto.BirthDate);
        Assert.Null(dto.Phone);
        Assert.Null(dto.Email);
        Assert.Equal(string.Empty, dto.Program);
        Assert.Equal(string.Empty, dto.Level);
        Assert.Equal(string.Empty, dto.Session);
        Assert.Equal(default(DateTime), dto.DateCreated);
        Assert.Equal(string.Empty, dto.UserCreated);
    }

    [Fact]
    public void StudentDto_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var dto = new StudentDto();
        var expectedDate = DateTime.Now;

        // Act
        dto.Id = 1;
        dto.FirstName = "Alice";
        dto.LastName = "Johnson";
        dto.MiddleName = "Marie";
        dto.Gender = "Female";
        dto.BirthDate = new DateTime(1999, 5, 15);
        dto.Phone = "9876543210";
        dto.Email = "alice@example.com";
        dto.Program = "Engineering";
        dto.Level = "200";
        dto.Session = "2023/2024";
        dto.DateCreated = expectedDate;
        dto.UserCreated = "admin";

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("Alice", dto.FirstName);
        Assert.Equal("Johnson", dto.LastName);
        Assert.Equal("Marie", dto.MiddleName);
        Assert.Equal("Female", dto.Gender);
        Assert.Equal(new DateTime(1999, 5, 15), dto.BirthDate);
        Assert.Equal("9876543210", dto.Phone);
        Assert.Equal("alice@example.com", dto.Email);
        Assert.Equal("Engineering", dto.Program);
        Assert.Equal("200", dto.Level);
        Assert.Equal("2023/2024", dto.Session);
        Assert.Equal(expectedDate, dto.DateCreated);
        Assert.Equal("admin", dto.UserCreated);
    }

    [Fact]
    public void StudentDto_OptionalFieldsCanBeNull()
    {
        // Arrange & Act
        var dto = new StudentDto
        {
            FirstName = "Bob",
            LastName = "Smith",
            MiddleName = null,
            Phone = null,
            Email = null,
            BirthDate = null
        };

        // Assert
        Assert.Null(dto.MiddleName);
        Assert.Null(dto.Phone);
        Assert.Null(dto.Email);
        Assert.Null(dto.BirthDate);
    }
}

public class StudentCreateDtoTests
{
    [Fact]
    public void StudentCreateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new StudentCreateDto();

        // Assert
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Null(dto.MiddleName);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Null(dto.BirthDate);
        Assert.Null(dto.Phone);
        Assert.Null(dto.Email);
        Assert.Equal(string.Empty, dto.Program);
        Assert.Equal(string.Empty, dto.Level);
        Assert.Equal(string.Empty, dto.Session);
        Assert.Equal(string.Empty, dto.UserCreated);
    }

    [Fact]
    public void StudentCreateDto_SetProperties_ShouldStoreValues()
    {
        // Arrange & Act
        var dto = new StudentCreateDto
        {
            FirstName = "Charlie",
            LastName = "Brown",
            MiddleName = "David",
            Gender = "Male",
            BirthDate = new DateTime(2001, 3, 20),
            Phone = "5551234567",
            Email = "charlie@example.com",
            Program = "Science",
            Level = "100",
            Session = "2024/2025",
            UserCreated = "system"
        };

        // Assert
        Assert.Equal("Charlie", dto.FirstName);
        Assert.Equal("Brown", dto.LastName);
        Assert.Equal("David", dto.MiddleName);
        Assert.Equal("Male", dto.Gender);
        Assert.Equal(new DateTime(2001, 3, 20), dto.BirthDate);
        Assert.Equal("5551234567", dto.Phone);
        Assert.Equal("charlie@example.com", dto.Email);
        Assert.Equal("Science", dto.Program);
        Assert.Equal("100", dto.Level);
        Assert.Equal("2024/2025", dto.Session);
        Assert.Equal("system", dto.UserCreated);
    }

    [Theory]
    [InlineData("Male")]
    [InlineData("Female")]
    public void StudentCreateDto_SetGender_ShouldAcceptDifferentValues(string gender)
    {
        // Arrange & Act
        var dto = new StudentCreateDto { Gender = gender };

        // Assert
        Assert.Equal(gender, dto.Gender);
    }
}

public class StudentUpdateDtoTests
{
    [Fact]
    public void StudentUpdateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new StudentUpdateDto();

        // Assert
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Null(dto.MiddleName);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Null(dto.BirthDate);
        Assert.Null(dto.Phone);
        Assert.Null(dto.Email);
        Assert.Equal(string.Empty, dto.Program);
        Assert.Equal(string.Empty, dto.Level);
        Assert.Equal(string.Empty, dto.Session);
    }

    [Fact]
    public void StudentUpdateDto_SetProperties_ShouldStoreValues()
    {
        // Arrange & Act
        var dto = new StudentUpdateDto
        {
            FirstName = "Diana",
            LastName = "Prince",
            MiddleName = "Elizabeth",
            Gender = "Female",
            BirthDate = new DateTime(2000, 7, 10),
            Phone = "5559876543",
            Email = "diana@example.com",
            Program = "Arts",
            Level = "300",
            Session = "2023/2024"
        };

        // Assert
        Assert.Equal("Diana", dto.FirstName);
        Assert.Equal("Prince", dto.LastName);
        Assert.Equal("Elizabeth", dto.MiddleName);
        Assert.Equal("Female", dto.Gender);
        Assert.Equal(new DateTime(2000, 7, 10), dto.BirthDate);
        Assert.Equal("5559876543", dto.Phone);
        Assert.Equal("diana@example.com", dto.Email);
        Assert.Equal("Arts", dto.Program);
        Assert.Equal("300", dto.Level);
        Assert.Equal("2023/2024", dto.Session);
    }

    [Fact]
    public void StudentUpdateDto_SetEmptyStrings_ShouldStoreEmptyValues()
    {
        // Arrange & Act
        var dto = new StudentUpdateDto
        {
            FirstName = "",
            LastName = "",
            Gender = ""
        };

        // Assert
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(string.Empty, dto.Gender);
    }
}
