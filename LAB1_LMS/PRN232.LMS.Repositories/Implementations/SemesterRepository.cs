using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Helpers;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Implementations;

public class SemesterRepository : ISemesterRepository
{
    private readonly LmsDbContext _context;

    public SemesterRepository(LmsDbContext context) => _context = context;

    public async Task<PagedResult<Semester>> GetAllAsync(QueryParameters p)
    {
        var query = _context.Semesters.AsQueryable();

        if (!string.IsNullOrWhiteSpace(p.Search))
        {
            var kw = p.Search.ToLower();
            query = query.Where(s => s.SemesterName.ToLower().Contains(kw));
        }

        query = SortHelper.ApplySort(query, p.Sort);

        var total = await query.CountAsync();
        var items = await query
            .Skip((p.Page - 1) * p.Size)
            .Take(p.Size)
            .ToListAsync();

        return new PagedResult<Semester>
        {
            Items      = items,
            Page       = p.Page,
            PageSize   = p.Size,
            TotalItems = total
        };
    }

    public async Task<Semester?> GetByIdAsync(int id) =>
        await _context.Semesters
            .Include(s => s.Courses)
            .ThenInclude(c => c.Subject)
            .FirstOrDefaultAsync(s => s.SemesterId == id);

    public async Task<Semester> CreateAsync(Semester semester)
    {
        _context.Semesters.Add(semester);
        await _context.SaveChangesAsync();
        return semester;
    }

    public async Task<Semester?> UpdateAsync(int id, Semester semester)
    {
        var existing = await _context.Semesters.FindAsync(id);
        if (existing is null) return null;

        existing.SemesterName = semester.SemesterName;
        existing.StartDate    = semester.StartDate;
        existing.EndDate      = semester.EndDate;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Semesters.FindAsync(id);
        if (existing is null) return false;

        _context.Semesters.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
