using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Parameters;

namespace PRN232.LMS.Repositories.Interfaces;

public interface IStudentRepository
{
    Task<PagedResult<Student>> GetAllAsync(QueryParameters parameters);
    Task<Student?> GetByIdAsync(int id);
    Task<Student> CreateAsync(Student student);
    Task<Student?> UpdateAsync(int id, Student student);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
