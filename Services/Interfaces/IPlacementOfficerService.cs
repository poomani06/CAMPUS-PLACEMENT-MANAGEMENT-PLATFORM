using CampusPlacementManagement.DTOs.PlacementOfficers;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IPlacementOfficerService
{
    Task<PlacementOfficerResponseDto> RegisterAsync(
        RegisterPlacementOfficerDto dto);

    Task<IEnumerable<PlacementOfficerResponseDto>> GetAllAsync();

    Task<IEnumerable<PlacementOfficerResponseDto>> GetPendingAsync();

    Task<PlacementOfficerResponseDto?> GetByIdAsync(int id);

    Task<bool> ApproveAsync(int id, string approvedBy);

    Task<bool> RejectAsync(int id);

    Task<bool> SuspendAsync(int id);

    Task<PlacementOfficerResponseDto?> GetMeAsync(string userId);
}