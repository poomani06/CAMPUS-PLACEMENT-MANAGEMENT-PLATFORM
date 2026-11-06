
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers;

[ApiController]
[Route("api/companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    // Public company registration
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        CreateCompanyDto dto)
    {
        var result = await _companyService.RegisterAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.CompanyId },
            result);
    }

    // SuperAdmin: Get all companies
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _companyService.GetAllAsync();

        return Ok(result);
    }

    // SuperAdmin: Get pending companies
    [HttpGet("pending")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _companyService.GetPendingAsync();

        return Ok(result);
    }

    // Get company by ID
    [HttpGet("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _companyService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                message = "Company not found."
            });
        }

        return Ok(result);
    }

    // SuperAdmin: Approve company
    [HttpPut("{id:int}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _companyService.ApproveAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Company not found."
            });
        }

        return Ok(new
        {
            message = "Company approved successfully."
        });
    }

    // SuperAdmin: Reject company
    [HttpPut("{id:int}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _companyService.RejectAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Company not found."
            });
        }

        return Ok(new
        {
            message = "Company rejected successfully."
        });
    }

    // SuperAdmin: Suspend company
    [HttpPut("{id:int}/suspend")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Suspend(int id)
    {
        var result = await _companyService.SuspendAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Company not found."
            });
        }

        return Ok(new
        {
            message = "Company suspended successfully."
        });
    }

    // SuperAdmin: Update company
    [HttpPut("{id:int}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCompanyDto dto)
    {
        var result = await _companyService.UpdateAsync(id, dto);

        return Ok(result);
    }
}
