using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Repositories;
using RMS.Infrastructure.Data;

namespace RMS.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for RegisteredCourse entity
/// </summary>
public class RegisteredCourseRepository : IRegisteredCourseRepository
{
    private readonly RmsDbContext _context;
    private readonly ILogger<RegisteredCourseRepository> _logger;

    public RegisteredCourseRepository(RmsDbContext context, ILogger<RegisteredCourseRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<RegisteredCourse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.RegisteredCourses
                .Where(rc => rc.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all registered courses");
            throw;
        }
    }

    public async Task<RegisteredCourse?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.RegisteredCourses
                .AsNoTracking()
                .FirstOrDefaultAsync(rc => rc.Id == id && rc.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving registered course with id {Id}", id);
            throw;
        }
    }

    public async Task<RegisteredCourse?> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.RegisteredCourses
                .AsNoTracking()
                .FirstOrDefaultAsync(rc => rc.StudentId == studentId && rc.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving registered courses for student id {StudentId}", studentId);
            throw;
        }
    }

    public async Task<RegisteredCourse> AddAsync(RegisteredCourse registeredCourse, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.RegisteredCourses.AddAsync(registeredCourse, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return registeredCourse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new registered course");
            throw;
        }
    }

    public async Task UpdateAsync(RegisteredCourse registeredCourse, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.RegisteredCourses.Update(registeredCourse);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating registered course with id {Id}", registeredCourse.Id);
            throw;
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var registeredCourse = await _context.RegisteredCourses.FindAsync(new object[] { id }, cancellationToken);
            if (registeredCourse != null)
            {
                registeredCourse.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting registered course with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.RegisteredCourses
                .AnyAsync(rc => rc.Id == id && rc.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if registered course exists with id {Id}", id);
            throw;
        }
    }
}
