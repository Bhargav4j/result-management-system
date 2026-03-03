using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.Domain.Interfaces.Repositories;
using RMS.Domain.Interfaces.Services;

namespace RMS.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;
    private readonly ILogger<CourseService> _logger;

    public CourseService(ICourseRepository repository, ILogger<CourseService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all courses");
            return await _repository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all courses");
            throw;
        }
    }

    public async Task<Course?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting course with id {Id}", id);
            return await _repository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting course with id {Id}", id);
            throw;
        }
    }

    public async Task<Course> CreateAsync(Course course, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new course");
            course.DateCreated = DateTime.Now;
            course.IsActive = true;
            return await _repository.AddAsync(course, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating course");
            throw;
        }
    }

    public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating course with id {Id}", course.Id);
            await _repository.UpdateAsync(course, cancellationToken);
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
            _logger.LogInformation("Deleting course with id {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting course with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Course>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching courses with term {SearchTerm}", searchTerm);
            return await _repository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching courses with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
