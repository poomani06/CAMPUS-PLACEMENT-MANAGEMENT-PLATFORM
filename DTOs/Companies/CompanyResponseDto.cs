
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.Companies;

public class CompanyResponseDto
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string Industry { get; set; } = string.Empty;

    public string OfficialEmail { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string? Website { get; set; }

    public AccountStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}
