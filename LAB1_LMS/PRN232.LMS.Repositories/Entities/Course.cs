namespace PRN232.LMS.Repositories.Entities;

public partial class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = null!;
    public int SemesterId { get; set; }
    public int SubjectId { get; set; }

    public virtual Semester Semester { get; set; } = null!;
    public virtual Subject Subject { get; set; } = null!;
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
