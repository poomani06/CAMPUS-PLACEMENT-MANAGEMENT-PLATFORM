using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models;

public class ApplicationStatusHistory
{
    public int ApplicationStatusHistoryId { get; set; }

    public int JobApplicationId { get; set; }

    public ApplicationStatus PreviousStatus { get; set; }

    public ApplicationStatus NewStatus { get; set; }

    public string ChangedByUserId { get; set; } = string.Empty;

    public string? Remarks { get; set; }

    public DateTime ChangedAt { get; set; }
        = DateTime.UtcNow;

    // Navigation property

    public JobApplication JobApplication { get; set; } = null!;
}