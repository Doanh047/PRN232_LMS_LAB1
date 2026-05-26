using PRN232.LMS.Repositories.Parameters;
using PRN232.LMS.Services.Models.Requests;
using PRN232.LMS.Services.Models.Responses;

namespace PRN232.LMS.Services.Interfaces;

public interface ISubjectService
{
    Task<PagedResponse<SubjectResponse>> GetAllAsync(QueryParameters parameters);
    Task<SubjectResponse?> GetByIdAsync(int id);
    Task<SubjectResponse> CreateAsync(SubjectRequest request);
    Task<SubjectResponse?> UpdateAsync(int id, SubjectRequest request);
    Task<bool> DeleteAsync(int id);
}
