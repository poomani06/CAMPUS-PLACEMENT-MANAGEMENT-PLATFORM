
using CampusPlacementManagement.DTOs.Companies;

namespace CampusPlacementManagement.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyResponseDto> RegisterAsync(CreateCompanyDto dto);

    Task<IEnumerable<CompanyResponseDto>> GetAllAsync();

    Task<IEnumerable<CompanyResponseDto>> GetPendingAsync();

    Task<CompanyResponseDto?> GetByIdAsync(int companyId);

    Task<CompanyResponseDto> UpdateAsync(
        int companyId,
        UpdateCompanyDto dto);

    Task<bool> ApproveAsync(int companyId);

    Task<bool> RejectAsync(int companyId);

    Task<bool> SuspendAsync(int companyId);
}
