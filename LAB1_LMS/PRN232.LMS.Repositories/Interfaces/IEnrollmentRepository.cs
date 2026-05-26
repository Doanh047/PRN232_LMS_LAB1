using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Interfaces;

public interface IEnrollmentRepository
{
    Task<PagedResult<Enrollment>> GetAllAsync(QueryParameters parameters);
    Task<PagedResult<Enrollment>> GetByCourseIdAsync(int courseId, QueryParameters parameters);
    Task<Enrollment?> GetByIdAsync(int id);
    Task<Enrollment> CreateAsync(Enrollment enrollment);
    Task<Enrollment?> UpdateAsync(int id, Enrollment enrollment);
    Task<bool> DeleteAsync(int id);
}
