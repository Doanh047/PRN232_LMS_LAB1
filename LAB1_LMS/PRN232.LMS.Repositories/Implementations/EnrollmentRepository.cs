using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Helpers;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Implementations;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly LmsDbContext _context;

    public EnrollmentRepository(LmsDbContext context) => _context = context;

    public async Task<PagedResult<Enrollment>> GetByCourseIdAsync(int courseId, QueryParameters p)
    {
        // Always load Course + Semester + Subject (needed for rich response)
        var query = _context.Enrollments
            .Where(e => e.CourseId == courseId)
            .Include(e => e.Course)
                .ThenInclude(c => c.Semester)
            .Include(e => e.Course)
                .ThenInclude(c => c.Subject)
            .AsQueryable();

        // Optionally load Student
        if (!string.IsNullOrWhiteSpace(p.Expand))
        {
            var expands = p.Expand.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (expands.Any(e => e.Trim().Equals("student", StringComparison.OrdinalIgnoreCase)))
                query = query.Include(e => e.Student);
        }

        if (!string.IsNullOrWhiteSpace(p.Search))
        {
            var kw = p.Search.ToLower();
            query = query.Where(e => e.Status.ToLower().Contains(kw));
        }

        query = SortHelper.ApplySort(query, p.Sort);

        var total = await query.CountAsync();
        var items = await query
            .Skip((p.Page - 1) * p.Size)
            .Take(p.Size)
            .ToListAsync();

        return new PagedResult<Enrollment>
        {
            Items      = items,
            Page       = p.Page,
            PageSize   = p.Size,
            TotalItems = total
        };
    }

    public async Task<PagedResult<Enrollment>> GetAllAsync(QueryParameters p)
    {
        var query = _context.Enrollments.AsQueryable();

        // Search by status or student name (requires join)
        if (!string.IsNullOrWhiteSpace(p.Search))
        {
            var kw = p.Search.ToLower();
            query = query
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e =>
                    e.Status.ToLower().Contains(kw) ||
                    e.Student.FullName.ToLower().Contains(kw) ||
                    e.Course.CourseName.ToLower().Contains(kw));
        }

        // Expand
        if (!string.IsNullOrWhiteSpace(p.Expand))
        {
            var expands = p.Expand.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (expands.Any(e => e.Trim().Equals("student", StringComparison.OrdinalIgnoreCase)))
                query = query.Include(e => e.Student);
            if (expands.Any(e => e.Trim().Equals("course", StringComparison.OrdinalIgnoreCase)))
                query = query.Include(e => e.Course).ThenInclude(c => c.Semester);
        }

        query = SortHelper.ApplySort(query, p.Sort);

        var total = await query.CountAsync();
        var items = await query
            .Skip((p.Page - 1) * p.Size)
            .Take(p.Size)
            .ToListAsync();

        return new PagedResult<Enrollment>
        {
            Items      = items,
            Page       = p.Page,
            PageSize   = p.Size,
            TotalItems = total
        };
    }

    public async Task<Enrollment?> GetByIdAsync(int id) =>
        await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .ThenInclude(c => c.Semester)
            .FirstOrDefaultAsync(e => e.EnrollmentId == id);

    public async Task<Enrollment> CreateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    public async Task<Enrollment?> UpdateAsync(int id, Enrollment enrollment)
    {
        var existing = await _context.Enrollments.FindAsync(id);
        if (existing is null) return null;

        existing.StudentId  = enrollment.StudentId;
        existing.CourseId   = enrollment.CourseId;
        existing.EnrollDate = enrollment.EnrollDate;
        existing.Status     = enrollment.Status;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Enrollments.FindAsync(id);
        if (existing is null) return false;

        _context.Enrollments.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
