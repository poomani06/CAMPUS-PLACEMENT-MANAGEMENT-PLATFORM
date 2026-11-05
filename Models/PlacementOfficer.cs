using CampusPlacementManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models;

public class PlacementOfficer
{
    [Key]
    public int PlacementOfficerId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int CollegeId { get; set; }

    public string Designation { get; set; } = string.Empty;

    public AccountStatus Status { get; set; }
        = AccountStatus.Pending;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // Navigation properties

    public ApplicationUser User { get; set; } = null!;

    public College College { get; set; } = null!;
}