using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Interfaces;

public interface IEnrollmentService
{
    Task<PagedResponse<EnrollmentResponse>> GetAllAsync(QueryParameters parameters);
    Task<EnrollmentResponse?> GetByIdAsync(int id);
    Task<EnrollmentResponse> CreateAsync(EnrollmentRequest request);
    Task<EnrollmentResponse?> UpdateAsync(int id, EnrollmentRequest request);
    Task<bool> DeleteAsync(int id);
}
