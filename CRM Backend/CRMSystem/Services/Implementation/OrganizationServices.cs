using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class OrganizationServices : IOrganizationServices
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAutomationServices _automationServices;

        public OrganizationServices(IOrganizationRepository organizationRepository, IAutomationServices automationServices)
        {
            _organizationRepository = organizationRepository;
            _automationServices = automationServices;
        }

        public async Task<OrganizationResponseDto> CreateOrganization(
            CreateOrganizationDto dto)
        {
            var organization = dto.Adapt<Organization>();

            organization.CreatedAt = DateTime.UtcNow;

            await _organizationRepository.CreateOrganization(organization);

            await _automationServices.OrganizationCreated(organization);

            return organization.Adapt<OrganizationResponseDto>();
        }

        public async Task<IEnumerable<OrganizationResponseDto>> GetAll()
        {
            var organizations =
                await _organizationRepository.GetAllOrganization();

            return organizations.Adapt<IEnumerable<OrganizationResponseDto>>();
        }

        public async Task<OrganizationResponseDto?> GetOrganizationById(Guid id)
        {
            var organization =
                await _organizationRepository.GetOrganizationById(id);

            if (organization == null)
            {
                return null;
            }

            return organization.Adapt<OrganizationResponseDto>();
        }

        public async Task<bool> UpdateOrganization(
            Guid id,
            UpdateOrganizationDto dto)
        {
            var organization =
                await _organizationRepository.GetOrganizationById(id);

            if (organization == null)
            {
                return false;
            }

            dto.Adapt(organization);

            organization.UpdatedAt = DateTime.UtcNow;

            await _organizationRepository.UpdateOrganization(organization);

            return true;
        }

        public async Task<bool> DeleteOrganization(Guid id)
        {
            var organization =
                await _organizationRepository.GetOrganizationById(id);

            if (organization == null)
            {
                return false;
            }

            await _organizationRepository.DeleteOrganization(id);

            return true;
        }
    }
}