using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Services;
using RMS.Web.Pages.Students;

namespace RMS.Web.Tests;

public class StudentsCreateModelTests
{
    private readonly Mock<IStudentService> _mockStudentService;
    private readonly Mock<ILogger<CreateModel>> _mockLogger;
    private readonly CreateModel _model;

    public StudentsCreateModelTests()
    {
        _mockStudentService = new Mock<IStudentService>();
        _mockLogger = new Mock<ILogger<CreateModel>>();
        _model = new CreateModel(_mockStudentService.Object, _mockLogger.Object);
    }

    [Fact]
    public void CreateModel_Constructor_ShouldInitializeSuccessfully()
    {
        // Arrange & Act
        var model = new CreateModel(_mockStudentService.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(model);
        Assert.NotNull(model.Input);
    }

    [Fact]
    public void OnGet_ShouldExecuteWithoutException()
    {
        // Arrange & Act
        _model.OnGet();

        // Assert - No exception thrown
        Assert.NotNull(_model);
    }

    [Fact]
    public async Task OnPostAsync_ShouldReturnPage_WhenModelStateInvalid()
    {
        // Arrange
        _model.ModelState.AddModelError("FirstName", "Required");

        // Act
        var result = await _model.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        _mockStudentService.Verify(s => s.CreateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task OnPostAsync_ShouldCreateStudentAndRedirect_WhenValid()
    {
        // Arrange
        _model.Input = new CreateModel.InputModel
        {
            FirstName = "Test",
            LastName = "User",
            Gender = "Male",
            Program = "CS",
            Level = "100",
            Session = "2023/2024"
        };

        var createdStudent = new Student { Id = 1, FirstName = "Test", LastName = "User" };
        _mockStudentService.Setup(s => s.CreateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdStudent);

        // Act
        var result = await _model.OnPostAsync();

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
        var redirectResult = result as RedirectToPageResult;
        Assert.Equal("./Index", redirectResult!.PageName);
        _mockStudentService.Verify(s => s.CreateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OnPostAsync_ShouldSetDefaultValues_WhenCreatingStudent()
    {
        // Arrange
        _model.Input = new CreateModel.InputModel
        {
            FirstName = "Test",
            LastName = "User",
            Gender = "Male",
            Program = "CS",
            Level = "100",
            Session = "2023/2024"
        };

        Student? capturedStudent = null;
        _mockStudentService.Setup(s => s.CreateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
            .Callback<Student, CancellationToken>((student, ct) => capturedStudent = student)
            .ReturnsAsync(new Student());

        // Act
        await _model.OnPostAsync();

        // Assert
        Assert.NotNull(capturedStudent);
        Assert.True(capturedStudent.IsActive);
        Assert.NotEqual(default(DateTime), capturedStudent.DateCreated);
    }

    [Fact]
    public async Task OnPostAsync_ShouldReturnPage_WhenExceptionThrown()
    {
        // Arrange
        _model.Input = new CreateModel.InputModel
        {
            FirstName = "Test",
            LastName = "User",
            Gender = "Male",
            Program = "CS",
            Level = "100",
            Session = "2023/2024"
        };

        _mockStudentService.Setup(s => s.CreateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _model.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.False(_model.ModelState.IsValid);
    }

    [Fact]
    public void InputModel_Properties_ShouldInitializeWithDefaults()
    {
        // Arrange & Act
        var input = new CreateModel.InputModel();

        // Assert
        Assert.Equal(string.Empty, input.FirstName);
        Assert.Equal(string.Empty, input.LastName);
        Assert.Null(input.MiddleName);
        Assert.Equal(string.Empty, input.Gender);
        Assert.Null(input.BirthDate);
        Assert.Null(input.Email);
        Assert.Null(input.Phone);
        Assert.Equal(string.Empty, input.Program);
        Assert.Equal(string.Empty, input.Level);
        Assert.Equal(string.Empty, input.Session);
    }

    [Fact]
    public void InputModel_SetProperties_ShouldStoreValues()
    {
        // Arrange & Act
        var input = new CreateModel.InputModel
        {
            FirstName = "John",
            LastName = "Doe",
            MiddleName = "M",
            Gender = "Male",
            BirthDate = new DateTime(2000, 1, 1),
            Email = "john@example.com",
            Phone = "1234567890",
            Program = "CS",
            Level = "100",
            Session = "2023/2024"
        };

        // Assert
        Assert.Equal("John", input.FirstName);
        Assert.Equal("Doe", input.LastName);
        Assert.Equal("M", input.MiddleName);
        Assert.Equal("Male", input.Gender);
        Assert.Equal(new DateTime(2000, 1, 1), input.BirthDate);
        Assert.Equal("john@example.com", input.Email);
        Assert.Equal("1234567890", input.Phone);
        Assert.Equal("CS", input.Program);
        Assert.Equal("100", input.Level);
        Assert.Equal("2023/2024", input.Session);
    }
}
