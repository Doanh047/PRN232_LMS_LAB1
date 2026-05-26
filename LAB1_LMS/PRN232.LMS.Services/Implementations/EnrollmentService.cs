using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _repo;

    public EnrollmentService(IEnrollmentRepository repo) => _repo = repo;

    public async Task<PagedResponse<EnrollmentResponse>> GetAllAsync(QueryParameters parameters)
    {
        var paged = await _repo.GetAllAsync(parameters);
        return new PagedResponse<EnrollmentResponse>
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

    public async Task<EnrollmentResponse?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : MapToResponse(entity);
    }

    public async Task<EnrollmentResponse> CreateAsync(EnrollmentRequest request)
    {
        var entity = new Enrollment
        {
            StudentId  = request.StudentId,
            CourseId   = request.CourseId,
            EnrollDate = request.EnrollDate,
            Status     = request.Status
        };
        var created = await _repo.CreateAsync(entity);
        return MapToResponse(created);
    }

    public async Task<EnrollmentResponse?> UpdateAsync(int id, EnrollmentRequest request)
    {
        var entity = new Enrollment
        {
            StudentId  = request.StudentId,
            CourseId   = request.CourseId,
            EnrollDate = request.EnrollDate,
            Status     = request.Status
        };
        var updated = await _repo.UpdateAsync(id, entity);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static EnrollmentResponse MapToResponse(Enrollment e) => new()
    {
        EnrollmentId = e.EnrollmentId,
        StudentId    = e.StudentId,
        CourseId     = e.CourseId,
        EnrollDate   = e.EnrollDate,
        Status       = e.Status,
        Student = e.Student is null ? null : new StudentSummaryResponse
        {
            StudentId = e.Student.StudentId,
            FullName  = e.Student.FullName,
            Email     = e.Student.Email
        },
        Course = e.Course is null ? null : new CourseSummaryResponse
        {
            CourseId     = e.Course.CourseId,
            CourseName   = e.Course.CourseName,
            SemesterName = e.Course.Semester?.SemesterName
        }
    };
}
