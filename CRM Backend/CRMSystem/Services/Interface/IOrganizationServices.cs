using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface IOrganizationServices
    {
        Task<OrganizationResponseDto> CreateOrganization(CreateOrganizationDto dto);

        Task<IEnumerable<OrganizationResponseDto>> GetAll();

        Task<OrganizationResponseDto?> GetOrganizationById(Guid id);

        Task<bool> UpdateOrganization(Guid id,UpdateOrganizationDto dto);

        Task<bool> DeleteOrganization(Guid id);
    }
}
