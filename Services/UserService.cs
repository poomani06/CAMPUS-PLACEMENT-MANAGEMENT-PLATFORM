using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public UserService(
        IUserRepository userRepository,
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(
            loginDto.Email);

        if (user == null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var passwordValid = await _userManager
            .CheckPasswordAsync(user, loginDto.Password);

        if (!passwordValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        // Only approved accounts are allowed to log in.
        if (user.AccountStatus != AccountStatus.Approved)
        {
            throw new UnauthorizedAccessException(
                "Your account is not approved or active.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Any())
        {
            throw new UnauthorizedAccessException(
                "No role is assigned to this account.");
        }

        var token = await _jwtService.GenerateTokenAsync(user);

        var expirationMinutes = 60;

        return new LoginResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            Roles = roles.ToList(),
            Token = token,
            TokenExpiration = DateTime.UtcNow.AddMinutes(
                expirationMinutes)
        };
    }
}