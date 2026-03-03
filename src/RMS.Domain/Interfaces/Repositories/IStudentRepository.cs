using RMS.Domain.Entities;

namespace RMS.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Student entity
/// </summary>
public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Student?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Student> AddAsync(Student student, CancellationToken cancellationToken = default);
    Task UpdateAsync(Student student, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Student>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
