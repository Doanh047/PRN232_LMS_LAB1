using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Helpers;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Implementations;

public class CourseRepository : ICourseRepository
{
    private readonly LmsDbContext _context;

    public CourseRepository(LmsDbContext context) => _context = context;

    public async Task<PagedResult<Course>> GetAllAsync(QueryParameters p)
    {
        var query = _context.Courses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(p.Search))
        {
            var kw = p.Search.ToLower();
            query = query.Where(c => c.CourseName.ToLower().Contains(kw));
        }

        if (!string.IsNullOrWhiteSpace(p.Expand))
        {
            var expands = p.Expand.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (expands.Any(e => e.Trim().Equals("semester", StringComparison.OrdinalIgnoreCase)))
                query = query.Include(c => c.Semester);
            if (expands.Any(e => e.Trim().Equals("subject", StringComparison.OrdinalIgnoreCase)))
                query = query.Include(c => c.Subject);
        }

        query = SortHelper.ApplySort(query, p.Sort);

        var total = await query.CountAsync();
        var items = await query
            .Skip((p.Page - 1) * p.Size)
            .Take(p.Size)
            .ToListAsync();

        return new PagedResult<Course>
        {
            Items      = items,
            Page       = p.Page,
            PageSize   = p.Size,
            TotalItems = total
        };
    }

    public async Task<Course?> GetByIdAsync(int id) =>
        await _context.Courses
            .Include(c => c.Semester)
            .Include(c => c.Subject)
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.CourseId == id);

    public async Task<Course> CreateAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course?> UpdateAsync(int id, Course course)
    {
        var existing = await _context.Courses.FindAsync(id);
        if (existing is null) return null;

        existing.CourseName  = course.CourseName;
        existing.SemesterId  = course.SemesterId;
        existing.SubjectId   = course.SubjectId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Courses.FindAsync(id);
        if (existing is null) return false;

        _context.Courses.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
