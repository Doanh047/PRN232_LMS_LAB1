using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Implementations;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _repo;

    public SubjectService(ISubjectRepository repo) => _repo = repo;

    public async Task<PagedResponse<SubjectResponse>> GetAllAsync(QueryParameters parameters)
    {
        var paged = await _repo.GetAllAsync(parameters);
        return new PagedResponse<SubjectResponse>
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

    public async Task<SubjectResponse?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : MapToResponse(entity);
    }

    public async Task<SubjectResponse> CreateAsync(SubjectRequest request)
    {
        var entity = new Subject
        {
            SubjectCode = request.SubjectCode,
            SubjectName = request.SubjectName,
            Credit      = request.Credit
        };
        var created = await _repo.CreateAsync(entity);
        return MapToResponse(created);
    }

    public async Task<SubjectResponse?> UpdateAsync(int id, SubjectRequest request)
    {
        var entity = new Subject
        {
            SubjectCode = request.SubjectCode,
            SubjectName = request.SubjectName,
            Credit      = request.Credit
        };
        var updated = await _repo.UpdateAsync(id, entity);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static SubjectResponse MapToResponse(Subject s) => new()
    {
        SubjectId   = s.SubjectId,
        SubjectCode = s.SubjectCode,
        SubjectName = s.SubjectName,
        Credit      = s.Credit,
        CourseCount = s.Courses?.Count ?? 0
    };
}
