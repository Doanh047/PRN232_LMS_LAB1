using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<PagedResult<Course>> GetAllAsync(QueryParameters parameters);
    Task<Course?> GetByIdAsync(int id);
    Task<Course> CreateAsync(Course course);
    Task<Course?> UpdateAsync(int id, Course course);
    Task<bool> DeleteAsync(int id);
}
