namespace RMS.Domain.Entities;

/// <summary>
/// Represents a course entity
/// </summary>
public class Course
{
    public long Id { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public int CourseLevel { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsActive { get; set; } = true;
}
