using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RMS.Domain.Entities;

namespace RMS.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Student
/// </summary>
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.MiddleName)
            .HasMaxLength(50);

        builder.Property(e => e.Gender)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Phone)
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .HasMaxLength(50);

        builder.Property(e => e.Program)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Level)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Session)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.UserCreated)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.DateCreated)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);
    }
}
