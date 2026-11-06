using System.Security.Claims;
using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[Route("api/placement-officers")]
[ApiController]
public class PlacementOfficersController : ControllerBase
{
    private readonly IPlacementOfficerService _officerService;

    public PlacementOfficersController(
        IPlacementOfficerService officerService)
    {
        _officerService = officerService;
    }

    // Register Placement Officer
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterPlacementOfficerDto dto)
    {
        try
        {
            var officer = await _officerService.RegisterAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = officer.PlacementOfficerId },
                officer);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // Get all officers
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var officers = await _officerService.GetAllAsync();

        return Ok(officers);
    }

    // Get pending officers
    [HttpGet("pending")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetPending()
    {
        var officers = await _officerService.GetPendingAsync();

        return Ok(officers);
    }

    // Get officer by ID
    [HttpGet("{id:int}")]
    [Authorize(Roles = "SuperAdmin,PlacementOfficer")]
    public async Task<IActionResult> GetById(int id)
    {
        var officer = await _officerService.GetByIdAsync(id);

        if (officer == null)
        {
            return NotFound(new
            {
                message = "Placement Officer not found."
            });
        }

        // Placement Officer can view only their own profile.
        if (User.IsInRole("PlacementOfficer"))
        {
            var currentUserId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (officer.UserId != currentUserId)
            {
                return Forbid();
            }
        }

        return Ok(officer);
    }

    // Approve officer
    [HttpPut("{id:int}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var approvedBy = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(approvedBy))
        {
            return Unauthorized();
        }

        try
        {
            var result = await _officerService.ApproveAsync(
                id,
                approvedBy);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Placement Officer not found."
                });
            }

            return Ok(new
            {
                message = "Placement Officer approved successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // Reject officer
    [HttpPut("{id:int}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        try
        {
            var result = await _officerService.RejectAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Placement Officer not found."
                });
            }

            return Ok(new
            {
                message = "Placement Officer rejected successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // Suspend officer
    [HttpPut("{id:int}/suspend")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Suspend(int id)
    {
        try
        {
            var result = await _officerService.SuspendAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Placement Officer not found."
                });
            }

            return Ok(new
            {
                message = "Placement Officer suspended successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // Current Placement Officer
    [HttpGet("me")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var officer = await _officerService.GetMeAsync(userId);

        if (officer == null)
        {
            return NotFound(new
            {
                message = "Placement Officer profile not found."
            });
        }

        return Ok(officer);
    }
}