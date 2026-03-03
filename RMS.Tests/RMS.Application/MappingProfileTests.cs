using Xunit;
using AutoMapper;
using RMS.Application.DTOs;
using RMS.Application.Mappings;
using RMS.Domain.Entities;

namespace RMS.Application.Tests;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void MappingProfile_Configuration_ShouldBeValid()
    {
        // Arrange
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        // Act & Assert
        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void MappingProfile_StudentToStudentDto_ShouldMapCorrectly()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            MiddleName = "M",
            Gender = "Male",
            BirthDate = new DateTime(2000, 1, 1),
            Phone = "1234567890",
            Email = "john@example.com",
            Program = "CS",
            Level = "100",
            Session = "2023/2024",
            DateCreated = DateTime.Now,
            UserCreated = "admin",
            IsActive = true
        };

        // Act
        var dto = _mapper.Map<StudentDto>(student);

        // Assert
        Assert.Equal(student.Id, dto.Id);
        Assert.Equal(student.FirstName, dto.FirstName);
        Assert.Equal(student.LastName, dto.LastName);
        Assert.Equal(student.MiddleName, dto.MiddleName);
        Assert.Equal(student.Gender, dto.Gender);
        Assert.Equal(student.BirthDate, dto.BirthDate);
        Assert.Equal(student.Phone, dto.Phone);
        Assert.Equal(student.Email, dto.Email);
        Assert.Equal(student.Program, dto.Program);
        Assert.Equal(student.Level, dto.Level);
        Assert.Equal(student.Session, dto.Session);
        Assert.Equal(student.DateCreated, dto.DateCreated);
        Assert.Equal(student.UserCreated, dto.UserCreated);
    }

    [Fact]
    public void MappingProfile_StudentCreateDtoToStudent_ShouldMapCorrectlyAndIgnoreId()
    {
        // Arrange
        var createDto = new StudentCreateDto
        {
            FirstName = "Jane",
            LastName = "Smith",
            Gender = "Female",
            Program = "Engineering",
            Level = "200",
            Session = "2023/2024",
            UserCreated = "system"
        };

        // Act
        var student = _mapper.Map<Student>(createDto);

        // Assert
        Assert.Equal(0, student.Id);
        Assert.Equal(createDto.FirstName, student.FirstName);
        Assert.Equal(createDto.LastName, student.LastName);
        Assert.Equal(createDto.Gender, student.Gender);
        Assert.Equal(createDto.Program, student.Program);
        Assert.Equal(createDto.Level, student.Level);
        Assert.Equal(createDto.Session, student.Session);
        Assert.Equal(createDto.UserCreated, student.UserCreated);
        Assert.True(student.IsActive);
    }

    [Fact]
    public void MappingProfile_StudentUpdateDtoToStudent_ShouldMapCorrectlyAndIgnoreSpecialFields()
    {
        // Arrange
        var updateDto = new StudentUpdateDto
        {
            FirstName = "Updated",
            LastName = "Name",
            Gender = "Male",
            Program = "Science",
            Level = "300",
            Session = "2024/2025"
        };

        // Act
        var student = _mapper.Map<Student>(updateDto);

        // Assert
        Assert.Equal(0, student.Id);
        Assert.Equal(updateDto.FirstName, student.FirstName);
        Assert.Equal(updateDto.LastName, student.LastName);
        Assert.Equal(updateDto.Gender, student.Gender);
        Assert.Equal(updateDto.Program, student.Program);
        Assert.Equal(updateDto.Level, student.Level);
        Assert.Equal(updateDto.Session, student.Session);
    }

    [Fact]
    public void MappingProfile_CourseToCourseDto_ShouldMapCorrectly()
    {
        // Arrange
        var course = new Course
        {
            Id = 1,
            CourseTitle = "Data Structures",
            CourseLevel = 200,
            DateCreated = DateTime.Now,
            IsActive = true
        };

        // Act
        var dto = _mapper.Map<CourseDto>(course);

        // Assert
        Assert.Equal(course.Id, dto.Id);
        Assert.Equal(course.CourseTitle, dto.CourseTitle);
        Assert.Equal(course.CourseLevel, dto.CourseLevel);
        Assert.Equal(course.DateCreated, dto.DateCreated);
    }

    [Fact]
    public void MappingProfile_CourseCreateDtoToCourse_ShouldMapCorrectlyAndIgnoreId()
    {
        // Arrange
        var createDto = new CourseCreateDto
        {
            CourseTitle = "Algorithms",
            CourseLevel = 300
        };

        // Act
        var course = _mapper.Map<Course>(createDto);

        // Assert
        Assert.Equal(0, course.Id);
        Assert.Equal(createDto.CourseTitle, course.CourseTitle);
        Assert.Equal(createDto.CourseLevel, course.CourseLevel);
        Assert.True(course.IsActive);
    }

    [Fact]
    public void MappingProfile_CourseUpdateDtoToCourse_ShouldMapCorrectlyAndIgnoreSpecialFields()
    {
        // Arrange
        var updateDto = new CourseUpdateDto
        {
            CourseTitle = "Machine Learning",
            CourseLevel = 400
        };

        // Act
        var course = _mapper.Map<Course>(updateDto);

        // Assert
        Assert.Equal(0, course.Id);
        Assert.Equal(updateDto.CourseTitle, course.CourseTitle);
        Assert.Equal(updateDto.CourseLevel, course.CourseLevel);
    }

    [Fact]
    public void MappingProfile_StudentWithNullOptionalFields_ShouldMapCorrectly()
    {
        // Arrange
        var student = new Student
        {
            Id = 1,
            FirstName = "Test",
            LastName = "User",
            MiddleName = null,
            Gender = "Male",
            BirthDate = null,
            Phone = null,
            Email = null,
            Program = "Test",
            Level = "100",
            Session = "2023/2024",
            DateCreated = DateTime.Now,
            UserCreated = "admin",
            IsActive = true
        };

        // Act
        var dto = _mapper.Map<StudentDto>(student);

        // Assert
        Assert.Null(dto.MiddleName);
        Assert.Null(dto.BirthDate);
        Assert.Null(dto.Phone);
        Assert.Null(dto.Email);
    }
}
