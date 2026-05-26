using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Implementations;

public class SemesterService : ISemesterService
{
    private readonly ISemesterRepository _repo;

    public SemesterService(ISemesterRepository repo) => _repo = repo;

    public async Task<PagedResponse<SemesterResponse>> GetAllAsync(QueryParameters parameters)
    {
        var paged = await _repo.GetAllAsync(parameters);
        return new PagedResponse<SemesterResponse>
        {
            Items = paged.Items.Select(MapToResponse).ToList(),
            Pagination = new PaginationMeta
            {
                Page       = paged.Page,
                PageSize   = paged.PageSize,
                TotalItems = paged.TotalItems,
                TotalPages = paged.TotalPages
            }
        };
    }

    public async Task<SemesterResponse?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : MapToResponse(entity);
    }

    public async Task<SemesterResponse> CreateAsync(SemesterRequest request)
    {
        var entity = new Semester
        {
            SemesterName = request.SemesterName,
            StartDate    = request.StartDate,
            EndDate      = request.EndDate
        };
        var created = await _repo.CreateAsync(entity);
        return MapToResponse(created);
    }

    public async Task<SemesterResponse?> UpdateAsync(int id, SemesterRequest request)
    {
        var entity = new Semester
        {
            SemesterName = request.SemesterName,
            StartDate    = request.StartDate,
            EndDate      = request.EndDate
        };
        var updated = await _repo.UpdateAsync(id, entity);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static SemesterResponse MapToResponse(Semester s) => new()
    {
        SemesterId   = s.SemesterId,
        SemesterName = s.SemesterName,
        StartDate    = s.StartDate,
        EndDate      = s.EndDate,
        CourseCount  = s.Courses?.Count ?? 0
    };
}
