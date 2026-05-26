using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Interfaces;

public interface IStudentService
{
    Task<PagedResponse<StudentResponse>> GetAllAsync(QueryParameters parameters);
    Task<StudentResponse?> GetByIdAsync(int id);
    Task<StudentResponse> CreateAsync(StudentRequest request);
    Task<StudentResponse?> UpdateAsync(int id, StudentRequest request);
    Task<bool> DeleteAsync(int id);
}
