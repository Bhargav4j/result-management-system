namespace RMS.Domain.Entities;

/// <summary>
/// Represents a student result entity
/// </summary>
public class Result
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public string Program { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Session { get; set; } = string.Empty;
    public string? CourseId1 { get; set; }
    public string? Score1 { get; set; }
    public string? Grade1 { get; set; }
    public string? CourseId2 { get; set; }
    public string? Score2 { get; set; }
    public string? Grade2 { get; set; }
    public string? CourseId3 { get; set; }
    public string? Score3 { get; set; }
    public string? Grade3 { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsActive { get; set; } = true;
}
