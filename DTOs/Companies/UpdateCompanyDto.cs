
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Companies;

public class UpdateCompanyDto
{
    [Required]
    [StringLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Industry { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string OfficialEmail { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [Url]
    public string? Website { get; set; }
}
