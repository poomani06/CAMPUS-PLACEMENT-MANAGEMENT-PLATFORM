using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models;

public class InterviewSchedule
{
    public int InterviewScheduleId { get; set; }

    public int JobApplicationId { get; set; }

    public int InterviewRoundId { get; set; }

    public DateTime InterviewDateTime { get; set; }

    public string? Location { get; set; }

    public string? MeetingLink { get; set; }

    public InterviewStatus Status { get; set; }
        = InterviewStatus.Scheduled;

    public string? Feedback { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // Navigation properties

    public JobApplication JobApplication { get; set; } = null!;

    public InterviewRound InterviewRound { get; set; } = null!;
}