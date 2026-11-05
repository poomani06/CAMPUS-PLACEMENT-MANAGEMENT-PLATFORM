using CampusPlacementManagement.DTOs.Auth;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IUserService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
}