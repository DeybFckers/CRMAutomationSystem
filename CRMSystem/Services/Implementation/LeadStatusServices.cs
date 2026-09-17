using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class LeadStatusServices : ILeadStatusServices
    {
        private readonly ILeadStatusRepository _leadStatusRepository;
        private readonly ICurrentUserServices _currentUserServices;

        public LeadStatusServices(ILeadStatusRepository leadStatusRepository, ICurrentUserServices currentUserServices)
        {
            _leadStatusRepository = leadStatusRepository;
            _currentUserServices = currentUserServices;
        }

        public async Task<IEnumerable<LeadStatusResponseDto>> GetAllLeadStatus()
        {
            var organizationId = _currentUserServices.OrganizationId;

            var leadStatuses = await _leadStatusRepository.GetAllStatus(organizationId);

            return leadStatuses.Adapt<IEnumerable<LeadStatusResponseDto>>();
        }

        public async Task<LeadStatusResponseDto> GetLeadStatusById(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var leadStatus = await _leadStatusRepository.GetLeadStatusById(id, organizationId);

            if (leadStatus == null)
                throw new KeyNotFoundException("Lead status not found.");

            return leadStatus.Adapt<LeadStatusResponseDto>();
        }

        public async Task CreateLeadStatus(CreateLeadStatusDto leadstatus)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var leadStatus = leadstatus.Adapt<LeadStatus>();

            leadStatus.Id = Guid.NewGuid();
            leadStatus.OrganizationId = organizationId;

            await _leadStatusRepository.CreateLeadStatus(leadStatus);
        }

        public async Task DeleteLeadStatusById(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId;

            await _leadStatusRepository.DeleteLeadStatus(id, organizationId);
        }
    }
}