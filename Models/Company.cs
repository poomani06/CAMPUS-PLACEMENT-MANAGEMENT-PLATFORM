using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models;

public class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string Industry { get; set; } = string.Empty;

    public string OfficialEmail { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string? Website { get; set; }

    public AccountStatus Status { get; set; }
        = AccountStatus.Pending;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // Navigation properties

    public ICollection<CompanyHR> CompanyHRs
    { get; set; } = new List<CompanyHR>();

    public ICollection<JobOpening> JobOpenings
    { get; set; } = new List<JobOpening>();
}