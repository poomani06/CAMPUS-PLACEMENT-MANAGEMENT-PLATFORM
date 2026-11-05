using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollegesController : ControllerBase
{
    private readonly ICollegeService _collegeService;

    public CollegesController(ICollegeService collegeService)
    {
        _collegeService = collegeService;
    }

    // College registration
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(
        [FromBody] CreateCollegeDto createCollegeDto)
    {
        try
        {
            var college = await _collegeService.CreateAsync(
                createCollegeDto);

            return CreatedAtAction(
                nameof(GetById),
                new { collegeId = college.CollegeId },
                college);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // Get all colleges
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var colleges = await _collegeService.GetAllAsync();

        return Ok(colleges);
    }

    // Get college by ID
    [HttpGet("{collegeId:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetById(int collegeId)
    {
        var college = await _collegeService.GetByIdAsync(
            collegeId);

        if (college == null)
        {
            return NotFound(new
            {
                message = "College not found."
            });
        }

        return Ok(college);
    }

    // Update college
    [HttpPut("{collegeId:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Update(
        int collegeId,
        [FromBody] UpdateCollegeDto updateCollegeDto)
    {
        try
        {
            var college = await _collegeService.UpdateAsync(
                collegeId,
                updateCollegeDto);

            if (college == null)
            {
                return NotFound(new
                {
                    message = "College not found."
                });
            }

            return Ok(college);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    // Approve college
    [HttpPut("{collegeId:int}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int collegeId)
    {
        try
        {
            var result = await _collegeService.ApproveAsync(
                collegeId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "College not found."
                });
            }

            return Ok(new
            {
                message = "College approved successfully."
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

    // Reject college
    [HttpPut("{collegeId:int}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int collegeId)
    {
        try
        {
            var result = await _collegeService.RejectAsync(
                collegeId);

            if (!result)
            {
                return NotFound(new
                {
                    message = "College not found."
                });
            }

            return Ok(new
            {
                message = "College rejected successfully."
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
}