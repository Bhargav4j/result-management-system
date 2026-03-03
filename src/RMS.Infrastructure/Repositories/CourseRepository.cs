using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Repositories;
using RMS.Infrastructure.Data;

namespace RMS.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Course entity
/// </summary>
public class CourseRepository : ICourseRepository
{
    private readonly RmsDbContext _context;
    private readonly ILogger<CourseRepository> _logger;

    public CourseRepository(RmsDbContext context, ILogger<CourseRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Courses
                .Where(c => c.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all courses");
            throw;
        }
    }

    public async Task<Course?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving course with id {Id}", id);
            throw;
        }
    }

    public async Task<Course> AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Courses.AddAsync(course, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return course;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new course");
            throw;
        }
    }

    public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating course with id {Id}", course.Id);
            throw;
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var course = await _context.Courses.FindAsync(new object[] { id }, cancellationToken);
            if (course != null)
            {
                course.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting course with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Courses
                .AnyAsync(c => c.Id == id && c.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if course exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Course>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Courses
                .Where(c => c.IsActive && c.CourseTitle.Contains(searchTerm))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching courses with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
