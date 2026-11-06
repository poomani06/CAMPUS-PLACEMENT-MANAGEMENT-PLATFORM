
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Company>> GetPendingAsync()
    {
        return await _context.Companies
            .AsNoTracking()
            .Where(c => c.Status == AccountStatus.Pending)
            .ToListAsync();
    }

    public async Task<Company?> GetByIdAsync(int companyId)
    {
        return await _context.Companies
            .Include(c => c.CompanyHRs)
            .FirstOrDefaultAsync(c => c.CompanyId == companyId);
    }

    public async Task<Company?> GetByEmailAsync(string officialEmail)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(c => c.OfficialEmail == officialEmail);
    }

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
    }

    public void Update(Company company)
    {
        _context.Companies.Update(company);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
