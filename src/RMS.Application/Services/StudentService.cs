using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Repositories;
using RMS.Domain.Interfaces.Services;

namespace RMS.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IStudentRepository repository, ILogger<StudentService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all students");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all students");
            throw;
        }
    }

    public async Task<Student?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting student with id {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student with id {Id}", id);
            throw;
        }
    }

    public async Task<Student> CreateAsync(Student student, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new student");
            student.DateCreated = DateTime.Now;
            student.IsActive = true;
            return await _repository.AddAsync(student, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student");
            throw;
        }
    }

    public async Task UpdateAsync(Student student, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating student with id {Id}", student.Id);
            await _repository.UpdateAsync(student, cancellationToken);
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
            _logger.LogInformation("Deleting student with id {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Student>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching students with term {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching students with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
