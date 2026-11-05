using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories;

public class CollegeRepository : ICollegeRepository
{
    private readonly ApplicationDbContext _context;

    public CollegeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<College>> GetAllAsync()
    {
        return await _context.Colleges
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<College?> GetByIdAsync(int collegeId)
    {
        return await _context.Colleges
            .FirstOrDefaultAsync(c => c.CollegeId == collegeId);
    }

    public async Task<College?> GetByCodeAsync(string collegeCode)
    {
        return await _context.Colleges
            .FirstOrDefaultAsync(c => c.CollegeCode == collegeCode);
    }

    public async Task AddAsync(College college)
    {
        await _context.Colleges.AddAsync(college);
    }

    public Task UpdateAsync(College college)
    {
        _context.Colleges.Update(college);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(College college)
    {
        _context.Colleges.Remove(college);

        return Task.CompletedTask;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}