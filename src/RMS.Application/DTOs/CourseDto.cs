namespace RMS.Application.DTOs;

/// <summary>
/// Data transfer object for Course
/// </summary>
public class CourseDto
{
    public long Id { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public int CourseLevel { get; set; }
    public DateTime DateCreated { get; set; }
}

/// <summary>
/// DTO for creating a new Course
/// </summary>
public class CourseCreateDto
{
    public string CourseTitle { get; set; } = string.Empty;
    public int CourseLevel { get; set; }
}

/// <summary>
/// DTO for updating a Course
/// </summary>
public class CourseUpdateDto
{
    public string CourseTitle { get; set; } = string.Empty;
    public int CourseLevel { get; set; }
}
