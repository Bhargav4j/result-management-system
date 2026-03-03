namespace RMS.Application.DTOs;

/// <summary>
/// Data transfer object for Student
/// </summary>
public class StudentDto
{
    public long Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string Program { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Session { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public string UserCreated { get; set; } = string.Empty;
}

/// <summary>
/// DTO for creating a new Student
/// </summary>
public class StudentCreateDto
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string Program { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Session { get; set; } = string.Empty;
    public string UserCreated { get; set; } = string.Empty;
}

/// <summary>
/// DTO for updating a Student
/// </summary>
public class StudentUpdateDto
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string Program { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Session { get; set; } = string.Empty;
}
