
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<CompanyResponseDto> RegisterAsync(
        CreateCompanyDto dto)
    {
        var existingCompany = await _companyRepository
            .GetByEmailAsync(dto.OfficialEmail);

        if (existingCompany != null)
        {
            throw new InvalidOperationException(
                "A company with this email already exists.");
        }

        var company = new Company
        {
            CompanyName = dto.CompanyName,
            Industry = dto.Industry,
            OfficialEmail = dto.OfficialEmail,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            Website = dto.Website,
            Status = AccountStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _companyRepository.AddAsync(company);
        await _companyRepository.SaveChangesAsync();

        return MapToDto(company);
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetAllAsync()
    {
        var companies = await _companyRepository.GetAllAsync();

        return companies.Select(MapToDto);
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetPendingAsync()
    {
        var companies = await _companyRepository.GetPendingAsync();

        return companies.Select(MapToDto);
    }

    public async Task<CompanyResponseDto?> GetByIdAsync(int companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);

        return company == null ? null : MapToDto(company);
    }

    public async Task<CompanyResponseDto> UpdateAsync(
        int companyId,
        UpdateCompanyDto dto)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);

        if (company == null)
        {
            throw new KeyNotFoundException("Company not found.");
        }

        company.CompanyName = dto.CompanyName;
        company.Industry = dto.Industry;
        company.OfficialEmail = dto.OfficialEmail;
        company.PhoneNumber = dto.PhoneNumber;
        company.Address = dto.Address;
        company.Website = dto.Website;

        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();

        return MapToDto(company);
    }

    public async Task<bool> ApproveAsync(int companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);

        if (company == null)
        {
            return false;
        }

        if (company.Status != AccountStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending companies can be approved.");
        }

        company.Status = AccountStatus.Approved;

        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RejectAsync(int companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);

        if (company == null)
        {
            return false;
        }

        if (company.Status != AccountStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending companies can be rejected.");
        }

        company.Status = AccountStatus.Rejected;

        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SuspendAsync(int companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);

        if (company == null)
        {
            return false;
        }

        if (company.Status != AccountStatus.Approved)
        {
            throw new InvalidOperationException(
                "Only approved companies can be suspended.");
        }

        company.Status = AccountStatus.Suspended;

        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();

        return true;
    }

    private static CompanyResponseDto MapToDto(Company company)
    {
        return new CompanyResponseDto
        {
            CompanyId = company.CompanyId,
            CompanyName = company.CompanyName,
            Industry = company.Industry,
            OfficialEmail = company.OfficialEmail,
            PhoneNumber = company.PhoneNumber,
            Address = company.Address,
            Website = company.Website,
            Status = company.Status,
            CreatedAt = company.CreatedAt
        };
    }
}
