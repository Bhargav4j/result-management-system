using RMS.Domain.Entities;

namespace RMS.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Course entity
/// </summary>
public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Course?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Course> AddAsync(Course course, CancellationToken cancellationToken = default);
    Task UpdateAsync(Course course, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
