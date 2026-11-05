using CampusPlacementManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models;

public class Student
{
    [Key]
    public int StudentId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int CollegeId { get; set; }

    public string RegisterNumber { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Degree { get; set; } = string.Empty;

    public int GraduationYear { get; set; }

    public decimal CGPA { get; set; }

    public string? ResumeUrl { get; set; }

    public AccountStatus Status { get; set; }
        = AccountStatus.Pending;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties

    public ApplicationUser User { get; set; } = null!;

    public College College { get; set; } = null!;

    public ICollection<JobApplication> JobApplications
    { get; set; } = new List<JobApplication>();
}