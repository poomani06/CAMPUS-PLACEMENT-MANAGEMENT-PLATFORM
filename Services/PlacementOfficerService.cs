using CampusPlacementManagement.Constants;
using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Services;

public class PlacementOfficerService : IPlacementOfficerService
{
    private readonly IPlacementOfficerRepository _officerRepository;
    private readonly ICollegeRepository _collegeRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public PlacementOfficerService(
        IPlacementOfficerRepository officerRepository,
        ICollegeRepository collegeRepository,
        UserManager<ApplicationUser> userManager)
    {
        _officerRepository = officerRepository;
        _collegeRepository = collegeRepository;
        _userManager = userManager;
    }

    public async Task<PlacementOfficerResponseDto> RegisterAsync(
        RegisterPlacementOfficerDto dto)
    {
        var college = await _collegeRepository.GetByIdAsync(
            dto.CollegeId);

        if (college == null)
        {
            throw new InvalidOperationException(
                "College not found.");
        }

        if (college.Status != CollegeStatus.Approved)
        {
            throw new InvalidOperationException(
                "College must be approved before officer registration.");
        }

        var existingUser = await _userManager.FindByEmailAsync(
            dto.Email);

        if (existingUser != null)
        {
            throw new InvalidOperationException(
                "Email is already registered.");
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            AccountStatus = AccountStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(
            user,
            dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(e => e.Description));

            throw new InvalidOperationException(errors);
        }

        var officer = new PlacementOfficer
        {
            UserId = user.Id,
            CollegeId = dto.CollegeId,
            Designation = dto.Designation,
            Status = PlacementOfficerStatus.Pending,
            ApprovedBy = null,
            ApprovedAt = null
        };

        await _officerRepository.AddAsync(officer);

        await _officerRepository.SaveChangesAsync();

        return MapToResponse(officer);
    }

    public async Task<IEnumerable<PlacementOfficerResponseDto>>
        GetAllAsync()
    {
        var officers = await _officerRepository.GetAllAsync();

        return officers.Select(MapToResponse);
    }

    public async Task<IEnumerable<PlacementOfficerResponseDto>>
        GetPendingAsync()
    {
        var officers = await _officerRepository.GetPendingAsync();

        return officers.Select(MapToResponse);
    }

    public async Task<PlacementOfficerResponseDto?> GetByIdAsync(
        int id)
    {
        var officer = await _officerRepository.GetByIdAsync(id);

        if (officer == null)
        {
            return null;
        }

        return MapToResponse(officer);
    }

    public async Task<bool> ApproveAsync(
        int id,
        string approvedBy)
    {
        var officer = await _officerRepository.GetByIdAsync(id);

        if (officer == null)
        {
            return false;
        }

        if (officer.Status != PlacementOfficerStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending officers can be approved.");
        }

        var user = await _userManager.FindByIdAsync(
            officer.UserId);

        if (user == null)
        {
            throw new InvalidOperationException(
                "Officer user account not found.");
        }

        var college = await _collegeRepository.GetByIdAsync(
            officer.CollegeId);

        if (college == null ||
            college.Status != CollegeStatus.Approved)
        {
            throw new InvalidOperationException(
                "Officer's college is not approved.");
        }

        var roleResult = await _userManager.AddToRoleAsync(
            user,
            UserRoles.PlacementOfficer);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(
                "; ",
                roleResult.Errors.Select(e => e.Description));

            throw new InvalidOperationException(errors);
        }

        officer.Status = PlacementOfficerStatus.Approved;
        officer.ApprovedBy = approvedBy;
        officer.ApprovedAt = DateTime.UtcNow;

        user.AccountStatus = AccountStatus.Approved;
        user.UpdatedAt = DateTime.UtcNow;

        await _officerRepository.UpdateAsync(officer);

        var userUpdateResult = await _userManager.UpdateAsync(user);

        if (!userUpdateResult.Succeeded)
        {
            throw new InvalidOperationException(
                "Unable to update officer account.");
        }

        return await _officerRepository.SaveChangesAsync();
    }

    public async Task<bool> RejectAsync(int id)
    {
        var officer = await _officerRepository.GetByIdAsync(id);

        if (officer == null)
        {
            return false;
        }

        if (officer.Status != PlacementOfficerStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending officers can be rejected.");
        }

        var user = await _userManager.FindByIdAsync(
            officer.UserId);

        if (user == null)
        {
            throw new InvalidOperationException(
                "Officer user account not found.");
        }

        officer.Status = PlacementOfficerStatus.Rejected;
        user.AccountStatus = AccountStatus.Rejected;
        user.UpdatedAt = DateTime.UtcNow;

        await _officerRepository.UpdateAsync(officer);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                "Unable to update officer account.");
        }

        return await _officerRepository.SaveChangesAsync();
    }

    public async Task<bool> SuspendAsync(int id)
    {
        var officer = await _officerRepository.GetByIdAsync(id);

        if (officer == null)
        {
            return false;
        }

        var user = await _userManager.FindByIdAsync(
            officer.UserId);

        if (user == null)
        {
            throw new InvalidOperationException(
                "Officer user account not found.");
        }

        officer.Status = PlacementOfficerStatus.Suspended;
        user.AccountStatus = AccountStatus.Suspended;
        user.UpdatedAt = DateTime.UtcNow;

        await _officerRepository.UpdateAsync(officer);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                "Unable to update officer account.");
        }

        return await _officerRepository.SaveChangesAsync();
    }

    public async Task<PlacementOfficerResponseDto?> GetMeAsync(
        string userId)
    {
        var officer = await _officerRepository.GetByUserIdAsync(
            userId);

        if (officer == null)
        {
            return null;
        }

        return MapToResponse(officer);
    }

    private static PlacementOfficerResponseDto MapToResponse(
        PlacementOfficer officer)
    {
        return new PlacementOfficerResponseDto
        {
            PlacementOfficerId = officer.PlacementOfficerId,
            UserId = officer.UserId,
            FullName = officer.User?.FullName ?? string.Empty,
            Email = officer.User?.Email ?? string.Empty,
            PhoneNumber = officer.User?.PhoneNumber ?? string.Empty,
            CollegeId = officer.CollegeId,
            CollegeName = officer.College?.CollegeName ?? string.Empty,
            Designation = officer.Designation,
            Status = officer.Status,
            ApprovedBy = officer.ApprovedBy,
            ApprovedAt = officer.ApprovedAt
        };
    }
}