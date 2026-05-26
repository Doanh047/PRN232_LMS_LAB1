using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Interfaces;

public interface ISemesterRepository
{
    Task<PagedResult<Semester>> GetAllAsync(QueryParameters parameters);
    Task<Semester?> GetByIdAsync(int id);
    Task<Semester> CreateAsync(Semester semester);
    Task<Semester?> UpdateAsync(int id, Semester semester);
    Task<bool> DeleteAsync(int id);
}
