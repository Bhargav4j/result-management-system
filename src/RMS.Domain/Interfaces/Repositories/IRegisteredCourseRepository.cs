using RMS.Domain.Entities;

namespace RMS.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for RegisteredCourse entity
/// </summary>
public interface IRegisteredCourseRepository
{
    Task<IEnumerable<RegisteredCourse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RegisteredCourse?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<RegisteredCourse?> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<RegisteredCourse> AddAsync(RegisteredCourse registeredCourse, CancellationToken cancellationToken = default);
    Task UpdateAsync(RegisteredCourse registeredCourse, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}
