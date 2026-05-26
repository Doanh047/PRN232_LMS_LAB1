namespace PRN232.LMS.Services.Models.Responses;

public class CourseResponse
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public int SubjectId { get; set; }
    public SemesterSummaryResponse? Semester { get; set; }
    public SubjectSummaryResponse? Subject { get; set; }
    public int EnrollmentCount { get; set; }
}

public class SemesterSummaryResponse
{
    public int SemesterId { get; set; }
    public string SemesterName { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class SubjectSummaryResponse
{
    public int SubjectId { get; set; }
    public string SubjectCode { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public int Credit { get; set; }
}
