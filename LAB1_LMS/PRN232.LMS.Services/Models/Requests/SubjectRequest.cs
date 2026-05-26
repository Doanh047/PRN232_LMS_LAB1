using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models.Requests;

public class SubjectRequest
{
    [Required]
    [MaxLength(20)]
    public string SubjectCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string SubjectName { get; set; } = string.Empty;

    [Required]
    [Range(1, 10)]
    public int Credit { get; set; }
}
