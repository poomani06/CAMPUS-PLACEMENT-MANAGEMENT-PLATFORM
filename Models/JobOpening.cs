using CampusPlacementManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models;

public class JobOpening
{
    [Key]
    public int JobOpeningId { get; set; }

    public int CompanyId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string JobDescription { get; set; } = string.Empty;

    public string RequiredSkills { get; set; } = string.Empty;

    public string Qualification { get; set; } = string.Empty;

    public decimal MinimumCGPA { get; set; }

    public int GraduationYear { get; set; }

    public decimal SalaryPackage { get; set; }

    public DateTime ApplicationDeadline { get; set; }

    public JobStatus Status { get; set; }
        = JobStatus.Draft;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // Navigation properties

    public Company Company { get; set; } = null!;

    public ICollection<JobTargetCollege> TargetColleges
    { get; set; } = new List<JobTargetCollege>();

    public ICollection<JobApplication> JobApplications
    { get; set; } = new List<JobApplication>();
}