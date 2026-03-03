using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Repositories;
using RMS.Infrastructure.Data;

namespace RMS.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Result entity
/// </summary>
public class ResultRepository : IResultRepository
{
    private readonly RmsDbContext _context;
    private readonly ILogger<ResultRepository> _logger;

    public ResultRepository(RmsDbContext context, ILogger<ResultRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Result>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Results
                .Where(r => r.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all results");
            throw;
        }
    }

    public async Task<Result?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Results
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving result with id {Id}", id);
            throw;
        }
    }

    public async Task<Result?> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Results
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving result for student id {StudentId}", studentId);
            throw;
        }
    }

    public async Task<Result> AddAsync(Result result, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Results.AddAsync(result, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new result");
            throw;
        }
    }

    public async Task UpdateAsync(Result result, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Results.Update(result);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating result with id {Id}", result.Id);
            throw;
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _context.Results.FindAsync(new object[] { id }, cancellationToken);
            if (result != null)
            {
                result.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting result with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Results
                .AnyAsync(r => r.Id == id && r.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if result exists with id {Id}", id);
            throw;
        }
    }
}
