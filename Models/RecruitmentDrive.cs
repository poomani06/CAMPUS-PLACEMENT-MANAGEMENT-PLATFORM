namespace CampusPlacementManagement.Models;

public class RecruitmentDrive
{
    public int RecruitmentDriveId { get; set; }

    public int JobId { get; set; }

    public int CollegeId { get; set; }

    public string DriveName { get; set; } = string.Empty;

    public DateTime DriveDate { get; set; }

    public string? Location { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // Navigation properties

    public JobOpening Job { get; set; } = null!;

    public College College { get; set; } = null!;

    public ICollection<InterviewRound> InterviewRounds
    { get; set; } = new List<InterviewRound>();
}