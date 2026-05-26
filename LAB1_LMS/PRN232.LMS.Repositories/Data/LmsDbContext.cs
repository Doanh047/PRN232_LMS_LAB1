using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Data;

public partial class LmsDbContext : DbContext
{
    public LmsDbContext() { }

    public LmsDbContext(DbContextOptions<LmsDbContext> options) : base(options) { }

    public virtual DbSet<Semester> Semesters { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Semester>(entity =>
        {
            entity.ToTable("Semester");
            entity.HasKey(e => e.SemesterId);
            entity.Property(e => e.SemesterName).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject");
            entity.HasKey(e => e.SubjectId);
            entity.HasIndex(e => e.SubjectCode).IsUnique();
            entity.Property(e => e.SubjectCode).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.SubjectName).HasMaxLength(100);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");
            entity.HasKey(e => e.CourseId);
            entity.Property(e => e.CourseName).HasMaxLength(100);

            entity.HasOne(e => e.Semester)
                  .WithMany(s => s.Courses)
                  .HasForeignKey(e => e.SemesterId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Subject)
                  .WithMany(s => s.Courses)
                  .HasForeignKey(e => e.SubjectId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");
            entity.HasKey(e => e.StudentId);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.ToTable("Enrollment");
            entity.HasKey(e => e.EnrollmentId);
            entity.Property(e => e.Status).HasMaxLength(20).IsUnicode(false).HasDefaultValue("Active");
            entity.Property(e => e.EnrollDate).HasColumnType("datetime");

            entity.HasOne(e => e.Student)
                  .WithMany(s => s.Enrollments)
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Course)
                  .WithMany(c => c.Enrollments)
                  .HasForeignKey(e => e.CourseId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
