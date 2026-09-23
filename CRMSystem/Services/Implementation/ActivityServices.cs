using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class ActivityServices : IActivityServices
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ICurrentUserServices _currentUserServices;

        public ActivityServices(IActivityRepository activityRepository, ICurrentUserServices currentUserServices)
        {
            _activityRepository = activityRepository;
            _currentUserServices = currentUserServices;
        }

        public async Task<ActivityResponseDto> CreateActivity(CreateActivityDto dto)
        {
            var organizationId = _currentUserServices.OrganizationId;
            var userId = _currentUserServices.UserId;

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

            dto.Adapt(activity);

            await _activityRepository.UpdateActivity(activity);

            return activity.Adapt<ActivityResponseDto>();
        }
    }
}