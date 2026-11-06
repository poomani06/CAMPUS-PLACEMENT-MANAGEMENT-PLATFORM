using CampusPlacementManagement.Data;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories;

public class PlacementOfficerRepository
    : IPlacementOfficerRepository
{
    private readonly ApplicationDbContext _context;

    public PlacementOfficerRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlacementOfficer>> GetAllAsync()
    {
        return await _context.PlacementOfficers
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.College)
            .ToListAsync();
    }

    public async Task<IEnumerable<PlacementOfficer>> GetPendingAsync()
    {
        return await _context.PlacementOfficers
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.College)
            .Where(p => p.Status == PlacementOfficerStatus.Pending)
            .ToListAsync();
    }

    public async Task<PlacementOfficer?> GetByIdAsync(int id)
    {
        return await _context.PlacementOfficers
            .Include(p => p.User)
            .Include(p => p.College)
            .FirstOrDefaultAsync(
                p => p.PlacementOfficerId == id);
    }

    public async Task<PlacementOfficer?> GetByUserIdAsync(
        string userId)
    {
        return await _context.PlacementOfficers
            .Include(p => p.User)
            .Include(p => p.College)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task AddAsync(PlacementOfficer placementOfficer)
    {
        await _context.PlacementOfficers.AddAsync(
            placementOfficer);
    }

    public Task UpdateAsync(PlacementOfficer placementOfficer)
    {
        _context.PlacementOfficers.Update(placementOfficer);

        return Task.CompletedTask;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}