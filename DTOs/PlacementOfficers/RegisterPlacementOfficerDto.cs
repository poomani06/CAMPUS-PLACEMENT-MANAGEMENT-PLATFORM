using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.PlacementOfficers;

public class RegisterPlacementOfficerDto
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public int CollegeId { get; set; }

    [Required]
    [StringLength(100)]
    public string Designation { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}