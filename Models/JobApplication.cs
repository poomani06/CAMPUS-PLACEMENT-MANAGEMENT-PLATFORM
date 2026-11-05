using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models;

public class JobApplication
{
    public int JobApplicationId { get; set; }

    public int StudentId { get; set; }

    public int JobId { get; set; }

    public ApplicationStatus Status { get; set; }
        = ApplicationStatus.Applied;

    public DateTime AppliedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties

    public Student Student { get; set; } = null!;

    public JobOpening Job { get; set; } = null!;

    public ICollection<ApplicationStatusHistory> StatusHistory
    { get; set; } = new List<ApplicationStatusHistory>();

    public ICollection<InterviewSchedule> InterviewSchedules
    { get; set; } = new List<InterviewSchedule>();
}
