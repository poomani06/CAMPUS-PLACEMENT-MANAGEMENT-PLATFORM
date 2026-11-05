using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}