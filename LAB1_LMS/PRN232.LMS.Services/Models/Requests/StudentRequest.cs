using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models.Requests;

public class StudentRequest
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }
}
