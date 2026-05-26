using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Interfaces;

public interface ICourseService
{
    Task<PagedResponse<CourseResponse>> GetAllAsync(QueryParameters parameters);
    Task<CourseResponse?> GetByIdAsync(int id);
    Task<CourseResponse> CreateAsync(CourseRequest request);
    Task<CourseResponse?> UpdateAsync(int id, CourseRequest request);
    Task<bool> DeleteAsync(int id);
    Task<PagedResponse<EnrollmentResponse>?> GetEnrollmentsByCourseIdAsync(int courseId, QueryParameters parameters);
}
