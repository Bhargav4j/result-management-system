namespace RMS.Domain.Entities;

/// <summary>
/// Represents a student entity
/// </summary>
public class Student
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
    public bool IsActive { get; set; } = true;
}
