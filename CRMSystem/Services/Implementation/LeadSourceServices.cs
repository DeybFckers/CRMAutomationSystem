using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class LeadSourceServices : ILeadSourceServices
    {
        private readonly ILeadSourceRepository _leadSourceRepository;
        private readonly ICurrentUserServices _currentUserServices;

        public LeadSourceServices(ILeadSourceRepository leadSourceRepository, ICurrentUserServices currentUserServices)
        {
            _leadSourceRepository = leadSourceRepository;
            _currentUserServices = currentUserServices;
        }

        public async Task<IEnumerable<LeadSourceResponseDto>> GetAllLeadSource()
        {
            var organizationId = _currentUserServices.OrganizationId;

            var leadSources = await _leadSourceRepository.GetAllLeadSource(organizationId);

            return leadSources.Adapt<IEnumerable<LeadSourceResponseDto>>();
        }

        public async Task<LeadSourceResponseDto> GetLeadSourceById(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var leadSource = await _leadSourceRepository.GetLeadSourceById(id, organizationId);

            if (leadSource == null)
                throw new KeyNotFoundException("Lead source not found.");

            return leadSource.Adapt<LeadSourceResponseDto>();
        }

        public async Task CreateLeadSource(CreateLeadSourceDto leadsource)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var leadSource = leadsource.Adapt<LeadSource>();

            leadSource.Id = Guid.NewGuid();
            leadSource.OrganizationId = organizationId;

            await _leadSourceRepository.CreateLeadSource(leadSource);
        }

        public async Task DeleteLeadSourceById(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId;

            await _leadSourceRepository.DeleteLeadSource(id, organizationId);
        }
    }
}