using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Data;

/// <summary>
/// Database context for RMS application
/// </summary>
public class RmsDbContext : DbContext
{
    public RmsDbContext(DbContextOptions<RmsDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Result> Results => Set<Result>();
    public DbSet<RegisteredCourse> RegisteredCourses => Set<RegisteredCourse>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RmsDbContext).Assembly);
    }
}
