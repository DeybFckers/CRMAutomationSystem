using CRMSystem.Data;
using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class OpportunityServices : IOpportunityServices
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IPipelinesRepository _pipelinesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPipelineStageRepository _pipilineStageRepository;

        public OpportunityServices(IOpportunityRepository opportunityRepository, 
            ICustomerRepository customerRepository, 
            ICurrentUserServices currentUserServices, 
            IOrganizationRepository organizationRepository,
            IPipelinesRepository pipelinesRepository, 
            IUserRepository userRepository,
            IPipelineStageRepository pipilineStageRepository)
        {
            _opportunityRepository = opportunityRepository;
            _customerRepository = customerRepository;
            _currentUserServices = currentUserServices;
            _organizationRepository = organizationRepository;
            _pipelinesRepository = pipelinesRepository;
            _userRepository = userRepository;
            _pipilineStageRepository = pipilineStageRepository;
        }

        public async Task AssignOpportunity(Guid id, Guid? assignedUserId)
        {
            var opportunity = await _opportunityRepository.GetOpportunityById(id, _currentUserServices.OrganizationId);

            if (opportunity == null)
            {
                throw new Exception("Opportunity not found");
            }

            if (assignedUserId.HasValue)
            {
                var organization = await _organizationRepository.GetOrganizationById(_currentUserServices.OrganizationId);

                if (organization == null)
                {
                    throw new Exception("Organization not found");
                }

                var assignedUser = await _userRepository.GetUserById(assignedUserId.Value, organization);

                if (assignedUser == null)
                {
                    throw new Exception("Assigned user not found");
                }
            }

            opportunity.AssignedUserId = assignedUserId;
            opportunity.UpdatedAt = DateTime.UtcNow;

            await _opportunityRepository.UpdateOpportunity(opportunity);
        }

        public async Task CreateOpportunity(CreateOpportunityDto opportunity)
        {
            var organization = await _organizationRepository.GetOrganizationById(_currentUserServices.OrganizationId);
            var customer = await _customerRepository.GetCustomerById(opportunity.CustomerId, organization.Id);

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            if (opportunity.AssignedUserId.HasValue)
            {
                var assignedUser = await _userRepository.GetUserById(opportunity.AssignedUserId.Value,organization);

                if (assignedUser == null)
                    throw new KeyNotFoundException("Assigned user not found.");
            }


            var pipeline = await _pipelinesRepository.GetPipelineById(opportunity.PipelineId, organization.Id);
            if (pipeline == null)
                throw new KeyNotFoundException("Pipeline not found.");

            var stage = await _pipilineStageRepository.GetPipelineStageById(opportunity.StageId, organization.Id);

            if (stage == null)
                throw new KeyNotFoundException(
                    "Pipeline stage not found.");

            if (stage.PipelineId != opportunity.PipelineId)
                throw new InvalidOperationException(
                    "Pipeline stage does not belong to the selected pipeline.");

            var newOpporunity = opportunity.Adapt<Opportunity>();

            newOpporunity.Id = Guid.NewGuid();
            newOpporunity.OrganizationId = _currentUserServices.OrganizationId;
            newOpporunity.Status = "Open";
            newOpporunity.CreatedAt = DateTime.UtcNow;
            newOpporunity.UpdatedAt = DateTime.UtcNow;

            await _opportunityRepository.CreateOpportunity(newOpporunity);
        }

        public async Task DeleteOpportunity(Guid id)
        {
            var opportunity = await _opportunityRepository.GetOpportunityById(id, _currentUserServices.OrganizationId);

            if (opportunity == null)
            {
                throw new Exception("Opportunity not found");
            }

            await _opportunityRepository.DeleteOpportunity(opportunity.Id);
        }   

        public async Task<IEnumerable<OpportunityResponseDto>> GetAllOpportunity()
        {
            var opportunity = await _opportunityRepository.GetAllOpportunity(_currentUserServices.OrganizationId);
            if(opportunity == null)
            {
                throw new Exception("No opportunity found");
            }
            return opportunity.Adapt<IEnumerable<OpportunityResponseDto>>();
        }

        public async Task<OpportunityResponseDto> GetOpportunityById(Guid id)
        {
            var opportunity = await _opportunityRepository.GetOpportunityById(id, _currentUserServices.OrganizationId);
            if(opportunity == null)
            {
                throw new Exception("Opportunity not found");
            }

            return opportunity.Adapt<OpportunityResponseDto>();
        }

        public async Task<OpportunityResponseDto> UpdateOpportunity(Guid id, UpdateOpportunityDto opportunity)
        {
            var existingOpportunity = await _opportunityRepository.GetOpportunityById(id, _currentUserServices.OrganizationId);

            if (existingOpportunity == null)
            {
                throw new Exception("Opportunity not found");
            }
                                                                    //you can get the customer id inside of current the organization id
            var customer = await _customerRepository.GetCustomerById(opportunity.CustomerId, _currentUserServices.OrganizationId);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            //you can get the stage id inside of pipeline id not the organization
            var stage = await _pipilineStageRepository.GetPipelineStageById(opportunity.StageId, existingOpportunity.PipelineId);

            if (stage == null)
                throw new KeyNotFoundException(
                    "Pipeline stage not found.");

            if (opportunity.AssignedUserId.HasValue)
            {
                var organization = await _organizationRepository.GetOrganizationById(_currentUserServices.OrganizationId);

                if (organization == null)
                    throw new KeyNotFoundException(
                        "Organization not found.");

                var assignedUser = await _userRepository.GetUserById(opportunity.AssignedUserId.Value, organization);

                if (assignedUser == null)
                    throw new KeyNotFoundException(
                        "Assigned user not found.");
            }

            opportunity.Adapt(existingOpportunity);

            existingOpportunity.UpdatedAt = DateTime.UtcNow;

            await _opportunityRepository.UpdateOpportunity(existingOpportunity);

            return existingOpportunity.Adapt<OpportunityResponseDto>();
        }

        public async Task UpdateOpportunityStage(Guid id, Guid stageId)
        {
            var opportunity = await _opportunityRepository.GetOpportunityById(id, _currentUserServices.OrganizationId);

            if (opportunity == null)
            {
                throw new Exception("Opportunity not found");
            }
                                                                            //you can get the stage id inside of pipeline id not the organization
            var stage = await _pipilineStageRepository.GetPipelineStageById(stageId, opportunity.PipelineId);

            if (stage == null)
            {
                throw new Exception("Pipeline stage not found");
            }

            if (stage.PipelineId != opportunity.PipelineId)
            {
                throw new Exception("Pipeline stage does not belong to the opportunity pipeline");
            }

            opportunity.StageId = stageId;
            opportunity.UpdatedAt = DateTime.UtcNow;

            await _opportunityRepository.UpdateOpportunity(opportunity);
        }

        public async Task UpdateOpportunityStatus(Guid id, string status)
        {
            var opportunity = await _opportunityRepository.GetOpportunityById(id, _currentUserServices.OrganizationId);

            if (opportunity == null)
            {
                throw new Exception("Opportunity not found");
            }

            var allowedStatuses = new[] { "OPEN", "WON", "LOST" };

            if (!allowedStatuses.Contains(status.ToUpper()))
            {
                throw new Exception("Invalid opportunity status");
            }

            opportunity.Status = status.ToUpper();
            opportunity.UpdatedAt = DateTime.UtcNow;

            await _opportunityRepository.UpdateOpportunity(opportunity);
        }
    }
}
