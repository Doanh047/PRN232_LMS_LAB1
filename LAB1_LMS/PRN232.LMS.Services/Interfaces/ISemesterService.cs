using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Interfaces;

public interface ISemesterService
{
    Task<PagedResponse<SemesterResponse>> GetAllAsync(QueryParameters parameters);
    Task<SemesterResponse?> GetByIdAsync(int id);
    Task<SemesterResponse> CreateAsync(SemesterRequest request);
    Task<SemesterResponse?> UpdateAsync(int id, SemesterRequest request);
    Task<bool> DeleteAsync(int id);
}
