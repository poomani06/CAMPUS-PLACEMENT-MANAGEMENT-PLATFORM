namespace CampusPlacementManagement.Models;

public class InterviewRound
{
    public int InterviewRoundId { get; set; }

    public int RecruitmentDriveId { get; set; }

    public string RoundName { get; set; } = string.Empty;

    public int RoundNumber { get; set; }

    public string? Description { get; set; }

    // Navigation properties

    public RecruitmentDrive RecruitmentDrive { get; set; } = null!;

    public ICollection<InterviewSchedule> InterviewSchedules
    { get; set; } = new List<InterviewSchedule>();
}