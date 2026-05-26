using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Interfaces;

public interface ISubjectRepository
{
    Task<PagedResult<Subject>> GetAllAsync(QueryParameters parameters);
    Task<Subject?> GetByIdAsync(int id);
    Task<Subject> CreateAsync(Subject subject);
    Task<Subject?> UpdateAsync(int id, Subject subject);
    Task<bool> DeleteAsync(int id);
}
