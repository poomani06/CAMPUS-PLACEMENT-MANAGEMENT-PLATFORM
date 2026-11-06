
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync();

    Task<IEnumerable<Company>> GetPendingAsync();

    Task<Company?> GetByIdAsync(int companyId);

    Task<Company?> GetByEmailAsync(string officialEmail);

    Task AddAsync(Company company);

    void Update(Company company);

    Task SaveChangesAsync();
}
