using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services;

public class CollegeService : ICollegeService
{
    private readonly ICollegeRepository _collegeRepository;

    public CollegeService(ICollegeRepository collegeRepository)
    {
        _collegeRepository = collegeRepository;
    }

    public async Task<IEnumerable<CollegeResponseDto>> GetAllAsync()
    {
        var colleges = await _collegeRepository.GetAllAsync();

        return colleges.Select(MapToResponse);
    }

    public async Task<CollegeResponseDto?> GetByIdAsync(
        int collegeId)
    {
        var college = await _collegeRepository.GetByIdAsync(
            collegeId);

        if (college == null)
        {
            return null;
        }

        return MapToResponse(college);
    }

    public async Task<CollegeResponseDto> CreateAsync(
        CreateCollegeDto createCollegeDto)
    {
        var existingCollege = await _collegeRepository
            .GetByCodeAsync(createCollegeDto.CollegeCode);

        if (existingCollege != null)
        {
            throw new InvalidOperationException(
                "College code already exists.");
        }

        var college = new College
        {
            CollegeName = createCollegeDto.CollegeName,
            CollegeCode = createCollegeDto.CollegeCode,
            UniversityName = createCollegeDto.UniversityName,
            OfficialEmail = createCollegeDto.OfficialEmail,
            PhoneNumber = createCollegeDto.PhoneNumber,
            Address = createCollegeDto.Address,
            Website = createCollegeDto.Website,

            // College registration requires SuperAdmin approval.
            Status = CollegeStatus.Pending,

            CreatedAt = DateTime.UtcNow
        };

        await _collegeRepository.AddAsync(college);

        await _collegeRepository.SaveChangesAsync();

        return MapToResponse(college);
    }

    public async Task<CollegeResponseDto?> UpdateAsync(
        int collegeId,
        UpdateCollegeDto updateCollegeDto)
    {
        var college = await _collegeRepository.GetByIdAsync(
            collegeId);

        if (college == null)
        {
            return null;
        }

        var existingCollege = await _collegeRepository
            .GetByCodeAsync(updateCollegeDto.CollegeCode);

        if (existingCollege != null &&
            existingCollege.CollegeId != collegeId)
        {
            throw new InvalidOperationException(
                "College code already exists.");
        }

        college.CollegeName = updateCollegeDto.CollegeName;
        college.CollegeCode = updateCollegeDto.CollegeCode;
        college.UniversityName = updateCollegeDto.UniversityName;
        college.OfficialEmail = updateCollegeDto.OfficialEmail;
        college.PhoneNumber = updateCollegeDto.PhoneNumber;
        college.Address = updateCollegeDto.Address;
        college.Website = updateCollegeDto.Website;

        await _collegeRepository.UpdateAsync(college);

        await _collegeRepository.SaveChangesAsync();

        return MapToResponse(college);
    }

    public async Task<bool> ApproveAsync(int collegeId)
    {
        var college = await _collegeRepository.GetByIdAsync(
            collegeId);

        if (college == null)
        {
            return false;
        }

        if (college.Status != CollegeStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending colleges can be approved.");
        }

        college.Status = CollegeStatus.Approved;

        await _collegeRepository.UpdateAsync(college);

        return await _collegeRepository.SaveChangesAsync();
    }

    public async Task<bool> RejectAsync(int collegeId)
    {
        var college = await _collegeRepository.GetByIdAsync(
            collegeId);

        if (college == null)
        {
            return false;
        }

        if (college.Status != CollegeStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending colleges can be rejected.");
        }

        college.Status = CollegeStatus.Rejected;

        await _collegeRepository.UpdateAsync(college);

        return await _collegeRepository.SaveChangesAsync();
    }

    private static CollegeResponseDto MapToResponse(
        College college)
    {
        return new CollegeResponseDto
        {
            CollegeId = college.CollegeId,
            CollegeName = college.CollegeName,
            CollegeCode = college.CollegeCode,
            UniversityName = college.UniversityName,
            OfficialEmail = college.OfficialEmail,
            PhoneNumber = college.PhoneNumber,
            Address = college.Address,
            Website = college.Website,
            Status = college.Status,
            CreatedAt = college.CreatedAt
        };
    }
}