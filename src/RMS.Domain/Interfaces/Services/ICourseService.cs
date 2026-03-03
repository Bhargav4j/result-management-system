using RMS.Domain.Entities;

namespace RMS.Domain.Interfaces.Services;

public interface ICourseService
{
    Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Course?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Course> CreateAsync(Course course, CancellationToken cancellationToken = default);
    Task UpdateAsync(Course course, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
