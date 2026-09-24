using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Implementation;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class ActivityServices : IActivityServices
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILeadRepository _leadRepository;
        private readonly IOpportunityRepository _opportunityRepository;

        public ActivityServices(IActivityRepository activityRepository, ICurrentUserServices currentUserServices, ICustomerRepository customerRepository, ILeadRepository leadRepository, IOpportunityRepository opportunityRepository)
        {
            _activityRepository = activityRepository;
            _currentUserServices = currentUserServices;
            _customerRepository = customerRepository;
            _leadRepository = leadRepository;
            _opportunityRepository = opportunityRepository;
        }

        public async Task<ActivityResponseDto> CreateActivity(CreateActivityDto dto)
        {
            var organizationId = _currentUserServices.OrganizationId;
            var userId = _currentUserServices.UserId;

            if (dto.CustomerId.HasValue)
            {
                var customer = await _customerRepository.GetCustomerById( dto.CustomerId.Value,organizationId);

                if (customer == null)
                    throw new KeyNotFoundException(
                        "Customer not found.");
            }
            if (dto.LeadId.HasValue)
            {
                var lead = await _leadRepository.GetLeadById(dto.LeadId.Value,organizationId);

                if (lead == null)
                    throw new KeyNotFoundException(
                        "Lead not found.");
            }

            if (dto.OpportunityId.HasValue)
            {
                var opportunity = await _opportunityRepository.GetOpportunityById(dto.OpportunityId.Value,organizationId);

                if (opportunity == null)
                    throw new KeyNotFoundException(
                        "Opportunity not found.");
            }

            var activity = dto.Adapt<Activity>();
            activity.Id = Guid.NewGuid();
            activity.OrganizationId = organizationId;
            activity.UserId = userId;
            activity.CreatedAt = DateTime.UtcNow;

            await _activityRepository.CreateActivity(activity);

            return activity.Adapt<ActivityResponseDto>();
        }

        public async Task DeleteActivity(Guid activityId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var activity = await _activityRepository.GetActivityById(organizationId, activityId);

            if (activity == null)
                throw new KeyNotFoundException("Activity not found.");

            await _activityRepository.DeleteActivity(organizationId, activityId);
        }

        public async Task<ActivityResponseDto?> GetActivityById(Guid activityId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var activity = await _activityRepository.GetActivityById(organizationId, activityId);

            return activity?.Adapt<ActivityResponseDto>();
        }

        public async Task<IEnumerable<ActivityResponseDto>> GetAllActivities()
        {
            var organizationId = _currentUserServices.OrganizationId;

            var activities = await _activityRepository.GetAllActivity(organizationId);

            return activities.Adapt<IEnumerable<ActivityResponseDto>>();
        }

        public async Task<ActivityResponseDto> UpdateActivity(Guid activityId, UpdateActivityDto dto)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var activity = await _activityRepository.GetActivityById(organizationId, activityId);

            if (activity == null)
                throw new KeyNotFoundException("Activity not found.");

            if (dto.CustomerId.HasValue)
            {
                var customer = await _customerRepository.GetCustomerById(dto.CustomerId.Value, organizationId);

                if (customer == null)
                    throw new KeyNotFoundException(
                        "Customer not found.");
            }
            if (dto.LeadId.HasValue)
            {
                var lead = await _leadRepository.GetLeadById(dto.LeadId.Value, organizationId);

                if (lead == null)
                    throw new KeyNotFoundException(
                        "Lead not found.");
            }

            if (dto.OpportunityId.HasValue)
            {
                var opportunity = await _opportunityRepository.GetOpportunityById(dto.OpportunityId.Value, organizationId);

                if (opportunity == null)
                    throw new KeyNotFoundException(
                        "Opportunity not found.");
            }

            dto.Adapt(activity);

            await _activityRepository.UpdateActivity(activity);

            return activity.Adapt<ActivityResponseDto>();
        }
    }
}