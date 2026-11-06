using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface IPlacementOfficerRepository
{
    Task<IEnumerable<PlacementOfficer>> GetAllAsync();

    Task<IEnumerable<PlacementOfficer>> GetPendingAsync();

    Task<PlacementOfficer?> GetByIdAsync(int id);

    Task<PlacementOfficer?> GetByUserIdAsync(string userId);

    Task AddAsync(PlacementOfficer placementOfficer);

    Task UpdateAsync(PlacementOfficer placementOfficer);

    Task<bool> SaveChangesAsync();
}