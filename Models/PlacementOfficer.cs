
using CampusPlacementManagement.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models;

public class PlacementOfficer
{
    [Key]
    public int PlacementOfficerId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int CollegeId { get; set; }

    public string Designation { get; set; } = string.Empty;

    public PlacementOfficerStatus Status { get; set; }
        = PlacementOfficerStatus.Pending;

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // Navigation properties

    public ApplicationUser User { get; set; } = null!;

    public College College { get; set; } = null!;
}
