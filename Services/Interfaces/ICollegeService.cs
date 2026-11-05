using CampusPlacementManagement.DTOs.Colleges;

namespace CampusPlacementManagement.Services.Interfaces;

public interface ICollegeService
{
    Task<IEnumerable<CollegeResponseDto>> GetAllAsync();

    Task<CollegeResponseDto?> GetByIdAsync(int collegeId);

    Task<CollegeResponseDto> CreateAsync(
        CreateCollegeDto createCollegeDto);

    Task<CollegeResponseDto?> UpdateAsync(
        int collegeId,
        UpdateCollegeDto updateCollegeDto);

    Task<bool> ApproveAsync(int collegeId);

    Task<bool> RejectAsync(int collegeId);
}