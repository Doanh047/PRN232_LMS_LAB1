namespace PRN232.LMS.Services.Models.Business;

public class CourseBM
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public string SemesterName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
}
