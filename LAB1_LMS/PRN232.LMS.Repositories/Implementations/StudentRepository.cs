using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Helpers;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Implementations;

public class StudentRepository : IStudentRepository
{
    private readonly LmsDbContext _context;

    public StudentRepository(LmsDbContext context) => _context = context;

    public async Task<PagedResult<Student>> GetAllAsync(QueryParameters p)
    {
        var query = _context.Students.AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(p.Search))
        {
            var kw = p.Search.ToLower();
            query = query.Where(s =>
                s.FullName.ToLower().Contains(kw) ||
                s.Email.ToLower().Contains(kw));
        }

        // Expand
        if (!string.IsNullOrWhiteSpace(p.Expand))
        {
            var expands = p.Expand.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (expands.Any(e => e.Trim().Equals("enrollments", StringComparison.OrdinalIgnoreCase)))
                query = query.Include(s => s.Enrollments);
        }

        // Sort
        query = SortHelper.ApplySort(query, p.Sort);

        // Count before paging
        var total = await query.CountAsync();

        // Paging
        var items = await query
            .Skip((p.Page - 1) * p.Size)
            .Take(p.Size)
            .ToListAsync();

        return new PagedResult<Student>
        {
            Items      = items,
            Page       = p.Page,
            PageSize   = p.Size,
            TotalItems = total
        };
    }

    public async Task<Student?> GetByIdAsync(int id) =>
        await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .ThenInclude(c => c.Semester)
            .FirstOrDefaultAsync(s => s.StudentId == id);

    public async Task<Student> CreateAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student?> UpdateAsync(int id, Student student)
    {
        var existing = await _context.Students.FindAsync(id);
        if (existing is null) return null;

        existing.FullName    = student.FullName;
        existing.Email       = student.Email;
        existing.DateOfBirth = student.DateOfBirth;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Students.FindAsync(id);
        if (existing is null) return false;

        _context.Students.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Students.AnyAsync(s => s.StudentId == id);
}
