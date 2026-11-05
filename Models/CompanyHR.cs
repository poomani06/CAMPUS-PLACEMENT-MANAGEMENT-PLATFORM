using CampusPlacementManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models;

public class CompanyHR
{
    [Key]
    public int CompanyHRId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int CompanyId { get; set; }

    public string Designation { get; set; } = string.Empty;

    public AccountStatus Status { get; set; }
        = AccountStatus.Pending;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // Navigation properties

    public ApplicationUser User { get; set; } = null!;

    public Company Company { get; set; } = null!;
}