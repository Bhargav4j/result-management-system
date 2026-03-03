using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Repositories;
using RMS.Infrastructure.Data;

namespace RMS.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Student entity
/// </summary>
public class StudentRepository : IStudentRepository
{
    private readonly RmsDbContext _context;
    private readonly ILogger<StudentRepository> _logger;

    public StudentRepository(RmsDbContext context, ILogger<StudentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Students
                .Where(s => s.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all students");
            throw;
        }
    }

    public async Task<Student?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student with id {Id}", id);
            throw;
        }
    }

    public async Task<Student> AddAsync(Student student, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Students.AddAsync(student, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return student;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding new student");
            throw;
        }
    }

    public async Task UpdateAsync(Student student, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student with id {Id}", student.Id);
            throw;
        }
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _context.Students.FindAsync(new object[] { id }, cancellationToken);
            if (student != null)
            {
                student.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Students
                .AnyAsync(s => s.Id == id && s.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if student exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Student>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Students
                .Where(s => s.IsActive &&
                    (s.FirstName.Contains(searchTerm) ||
                     s.LastName.Contains(searchTerm) ||
                     s.Email!.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching students with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
