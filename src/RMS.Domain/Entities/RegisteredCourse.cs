namespace RMS.Domain.Entities;

/// <summary>
/// Represents registered courses for a student
/// </summary>
public class RegisteredCourse
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public string Program { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Session { get; set; } = string.Empty;
    public string? CourseId1 { get; set; }
    public string? CourseId2 { get; set; }
    public string? CourseId3 { get; set; }
    public string? CourseId4 { get; set; }
    public string? CourseId5 { get; set; }
    public string? CourseId6 { get; set; }
    public string? CourseId7 { get; set; }
    public string? CourseId8 { get; set; }
    public string? CourseId9 { get; set; }
    public string? CourseId10 { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsActive { get; set; } = true;
}
