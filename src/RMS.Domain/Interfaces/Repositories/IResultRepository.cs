using RMS.Domain.Entities;

namespace RMS.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Result entity
/// </summary>
public interface IResultRepository
{
    Task<IEnumerable<Result>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Result?> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(Result result, CancellationToken cancellationToken = default);
    Task UpdateAsync(Result result, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
}
