using RMS.Domain.Entities;

namespace RMS.Domain.Interfaces.Services;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Student?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Student> CreateAsync(Student student, CancellationToken cancellationToken = default);
    Task UpdateAsync(Student student, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Student>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
