using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Helpers;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Implementations;

public class SubjectRepository : ISubjectRepository
{
    private readonly LmsDbContext _context;

    public SubjectRepository(LmsDbContext context) => _context = context;

    public async Task<PagedResult<Subject>> GetAllAsync(QueryParameters p)
    {
        var query = _context.Subjects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(p.Search))
        {
            var kw = p.Search.ToLower();
            query = query.Where(s =>
                s.SubjectName.ToLower().Contains(kw) ||
                s.SubjectCode.ToLower().Contains(kw));
        }

        query = SortHelper.ApplySort(query, p.Sort);

        var total = await query.CountAsync();
        var items = await query
            .Skip((p.Page - 1) * p.Size)
            .Take(p.Size)
            .ToListAsync();

        return new PagedResult<Subject>
        {
            Items      = items,
            Page       = p.Page,
            PageSize   = p.Size,
            TotalItems = total
        };
    }

    public async Task<Subject?> GetByIdAsync(int id) =>
        await _context.Subjects
            .Include(s => s.Courses)
            .FirstOrDefaultAsync(s => s.SubjectId == id);

    public async Task<Subject> CreateAsync(Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return subject;
    }

    public async Task<Subject?> UpdateAsync(int id, Subject subject)
    {
        var existing = await _context.Subjects.FindAsync(id);
        if (existing is null) return null;

        existing.SubjectCode = subject.SubjectCode;
        existing.SubjectName = subject.SubjectName;
        existing.Credit      = subject.Credit;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Subjects.FindAsync(id);
        if (existing is null) return false;

        _context.Subjects.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
