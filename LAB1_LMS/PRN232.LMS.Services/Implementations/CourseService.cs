using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository    _repo;
    private readonly IEnrollmentRepository _enrollmentRepo;

    public CourseService(ICourseRepository repo, IEnrollmentRepository enrollmentRepo)
    {
        _repo           = repo;
        _enrollmentRepo = enrollmentRepo;
    }

    public async Task<PagedResponse<CourseResponse>> GetAllAsync(QueryParameters parameters)
    {
        var paged = await _repo.GetAllAsync(parameters);
        return new PagedResponse<CourseResponse>
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

    public async Task<CourseResponse?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : MapToDetailResponse(entity);
    }

    public async Task<CourseResponse> CreateAsync(CourseRequest request)
    {
        var entity = new Course
        {
            CourseName = request.CourseName,
            SemesterId = request.SemesterId,
            SubjectId  = request.SubjectId
        };
        var created = await _repo.CreateAsync(entity);
        return MapToResponse(created);
    }

    public async Task<CourseResponse?> UpdateAsync(int id, CourseRequest request)
    {
        var entity = new Course
        {
            CourseName = request.CourseName,
            SemesterId = request.SemesterId,
            SubjectId  = request.SubjectId
        };
        var updated = await _repo.UpdateAsync(id, entity);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static CourseResponse MapToResponse(Course c) => new()
    {
        CourseId        = c.CourseId,
        CourseName      = c.CourseName,
        SemesterId      = c.SemesterId,
        SubjectId       = c.SubjectId,
        EnrollmentCount = c.Enrollments?.Count ?? 0,
        Semester = c.Semester is null ? null : new SemesterSummaryResponse
        {
            SemesterId   = c.Semester.SemesterId,
            SemesterName = c.Semester.SemesterName
        },
        Subject = c.Subject is null ? null : new SubjectSummaryResponse
        {
            SubjectId   = c.Subject.SubjectId,
            SubjectCode = c.Subject.SubjectCode,
            SubjectName = c.Subject.SubjectName,
            Credit      = c.Subject.Credit
        }
    };

    private static CourseResponse MapToDetailResponse(Course c)
    {
        var resp = MapToResponse(c);
        resp.EnrollmentCount = c.Enrollments?.Count ?? 0;
        return resp;
    }

    public async Task<PagedResponse<EnrollmentResponse>?> GetEnrollmentsByCourseIdAsync(int courseId, QueryParameters parameters)
    {
        // Return null if course doesn't exist
        var course = await _repo.GetByIdAsync(courseId);
        if (course is null) return null;

        var paged = await _enrollmentRepo.GetByCourseIdAsync(courseId, parameters);
        return new PagedResponse<EnrollmentResponse>
        {
            Items = paged.Items.Select(e => new EnrollmentResponse
            {
                EnrollmentId = e.EnrollmentId,
                StudentId    = e.StudentId,
                CourseId     = e.CourseId,
                EnrollDate   = e.EnrollDate,
                Status       = e.Status,
                Student = e.Student is null ? null : new StudentSummaryResponse
                {
                    StudentId   = e.Student.StudentId,
                    FullName    = e.Student.FullName,
                    Email       = e.Student.Email,
                    DateOfBirth = e.Student.DateOfBirth
                },
                Course = e.Course is null ? null : new CourseSummaryResponse
                {
                    CourseId     = e.Course.CourseId,
                    CourseName   = e.Course.CourseName,
                    SemesterName = e.Course.Semester?.SemesterName,
                    Semester = e.Course.Semester is null ? null : new SemesterSummaryResponse
                    {
                        SemesterId   = e.Course.Semester.SemesterId,
                        SemesterName = e.Course.Semester.SemesterName,
                        StartDate    = e.Course.Semester.StartDate,
                        EndDate      = e.Course.Semester.EndDate
                    },
                    Subject = e.Course.Subject is null ? null : new SubjectSummaryResponse
                    {
                        SubjectId   = e.Course.Subject.SubjectId,
                        SubjectCode = e.Course.Subject.SubjectCode,
                        SubjectName = e.Course.Subject.SubjectName,
                        Credit      = e.Course.Subject.Credit
                    }
                }
            }).ToList(),
            Pagination = new PaginationMeta
            {
                Page       = paged.Page,
                PageSize   = paged.PageSize,
                TotalItems = paged.TotalItems,
                TotalPages = paged.TotalPages
            }
        };
    }
}
