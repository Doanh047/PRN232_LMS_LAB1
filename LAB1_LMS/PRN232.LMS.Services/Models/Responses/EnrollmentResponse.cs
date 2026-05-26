namespace PRN232.LMS.Services.Models.Responses;

public class EnrollmentResponse
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public StudentSummaryResponse? Student { get; set; }
    public CourseSummaryResponse? Course { get; set; }
}

public class StudentSummaryResponse
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
}

public class CourseSummaryResponse
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string? SemesterName { get; set; }
    public SemesterSummaryResponse? Semester { get; set; }
    public SubjectSummaryResponse? Subject { get; set; }
}
