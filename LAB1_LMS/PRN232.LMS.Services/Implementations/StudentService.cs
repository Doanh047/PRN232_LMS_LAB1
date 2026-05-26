using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repo;

    public StudentService(IStudentRepository repo) => _repo = repo;

    public async Task<PagedResponse<StudentResponse>> GetAllAsync(QueryParameters parameters)
    {
        var paged = await _repo.GetAllAsync(parameters);
        return new PagedResponse<StudentResponse>
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

    public async Task<StudentResponse?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : MapToDetailResponse(entity);
    }

    public async Task<StudentResponse> CreateAsync(StudentRequest request)
    {
        var entity = new Student
        {
            FullName    = request.FullName,
            Email       = request.Email,
            DateOfBirth = request.DateOfBirth
        };
        var created = await _repo.CreateAsync(entity);
        return MapToResponse(created);
    }

    public async Task<StudentResponse?> UpdateAsync(int id, StudentRequest request)
    {
        var entity = new Student
        {
            FullName    = request.FullName,
            Email       = request.Email,
            DateOfBirth = request.DateOfBirth
        };
        var updated = await _repo.UpdateAsync(id, entity);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

    // ---- mapping helpers ----
    private static StudentResponse MapToResponse(Student s) => new()
    {
        StudentId   = s.StudentId,
        FullName    = s.FullName,
        Email       = s.Email,
        DateOfBirth = s.DateOfBirth
    };

    private static StudentResponse MapToDetailResponse(Student s) => new()
    {
        StudentId   = s.StudentId,
        FullName    = s.FullName,
        Email       = s.Email,
        DateOfBirth = s.DateOfBirth,
        Enrollments = s.Enrollments.Select(e => new EnrollmentSummaryResponse
        {
            EnrollmentId = e.EnrollmentId,
            CourseId     = e.CourseId,
            CourseName   = e.Course?.CourseName,
            EnrollDate   = e.EnrollDate,
            Status       = e.Status
        }).ToList()
    };
}
