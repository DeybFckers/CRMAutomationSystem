using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class TaskItemServices : ITaskItemServices
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILeadRepository _leadRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOpportunityRepository _opportunityRepository;

        public TaskItemServices(ITaskItemRepository taskItemRepository, ICurrentUserServices currentUserServices, IOrganizationRepository organizationRepository, IUserRepository userRepository, ILeadRepository leadRepository, ICustomerRepository customerRepository, IOpportunityRepository opportunityRepository)
        {
            _taskItemRepository = taskItemRepository;
            _currentUserServices = currentUserServices;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _leadRepository = leadRepository;
            _customerRepository = customerRepository;
            _opportunityRepository = opportunityRepository;
        }

        public async Task CreateTask(CreateTaskItemDto task)
        {
            var organization = await _organizationRepository.GetOrganizationById(_currentUserServices.OrganizationId);
            var assignedUser = await _userRepository.GetUserById(task.AssignedUserId, organization);

            if (assignedUser == null)
            {
                throw new Exception("Assigned user not found in the organization.");
            }

            if (task.LeadId.HasValue)
            {
                var lead = await _leadRepository.GetLeadById(task.LeadId.Value, organization.Id);

                if (lead == null)
                {
                    throw new Exception("Lead not found in the organization.");
                }
            }
            if (task.CustomerId.HasValue)
            {
                var customer = await _customerRepository.GetCustomerById(task.CustomerId.Value, organization.Id);

                if (customer == null)
                {
                    throw new Exception("Customer not found in the organization.");
                }
            }

            if (task.OpportunityId.HasValue)
            {
                var opportunity = await _opportunityRepository.GetOpportunityById(task.OpportunityId.Value, organization.Id);

                if (opportunity == null)
                {
                    throw new Exception("Opportunity not found in the organization.");
                }
            }

            var newTaskItem = task.Adapt<TaskItem>();
            newTaskItem.Id = Guid.NewGuid();
            newTaskItem.OrganizationId = organization.Id;
            newTaskItem.CreatedAt = DateTime.UtcNow;
            newTaskItem.Status = "PENDING";
            newTaskItem.CreatedAt = DateTime.UtcNow;

            await _taskItemRepository.CreateTask(newTaskItem);
        }


        public async Task DeleteTask(Guid id, Guid organizationId)
        {
            await _taskItemRepository.DeleteTask(id, organizationId);
        }

        public async Task<IEnumerable<TaskItemResponseDto>> GetAllTask(Guid organizationId)
        {
            var tasks = await _taskItemRepository.GetAllTask(organizationId);

            return tasks.Adapt<IEnumerable<TaskItemResponseDto>>();
        }

        public async Task<TaskItemResponseDto?> GetTaskById(Guid id, Guid organizationId)
        {
            var task = await _taskItemRepository.GetTaskById(id, organizationId);

            if (task == null)
            {
                return null;
            }

            return task.Adapt<TaskItemResponseDto>();
        }

        public async Task UpdateTask(Guid id, UpdateTaskItemDto task)
        {
            var organization = await _organizationRepository.GetOrganizationById(_currentUserServices.OrganizationId);

            if (organization == null)
            {
                throw new Exception("Organization not found.");
            }

            var existingTask = await _taskItemRepository.GetTaskById(id, organization.Id);

            if (existingTask == null)
            {
                throw new Exception("Task not found.");
            }

            var assignedUser = await _userRepository.GetUserById(task.AssignedUserId, organization);

            if (assignedUser == null)
            {
                throw new Exception("Assigned user not found in the organization.");
            }

            if (task.LeadId.HasValue)
            {
                var lead = await _leadRepository.GetLeadById(task.LeadId.Value, organization.Id);

                if (lead == null)
                {
                    throw new Exception("Lead not found in the organization.");
                }
            }

            if (task.CustomerId.HasValue)
            {
                var customer = await _customerRepository.GetCustomerById(task.CustomerId.Value, organization.Id);

                if (customer == null)
                {
                    throw new Exception("Customer not found in the organization.");
                }
            }

            if (task.OpportunityId.HasValue)
            {
                var opportunity = await _opportunityRepository.GetOpportunityById(task.OpportunityId.Value, organization.Id);

                if (opportunity == null)
                {
                    throw new Exception("Opportunity not found in the organization.");
                }
            }

            task.Adapt(existingTask);

            existingTask.OrganizationId = organization.Id;

            await _taskItemRepository.UpdateTask(existingTask);
        }

        public async Task CompleteTask(Guid id, Guid organizationId)
        {
            var task = await _taskItemRepository.GetTaskById(id, organizationId);

            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            task.Status = "COMPLETED";
            task.CompletedAt = DateTime.UtcNow;

            await _taskItemRepository.UpdateTask(task);
        }

        public async Task UpdateTaskStatus(Guid id, Guid organizationId, string status)
        {
            var task = await _taskItemRepository.GetTaskById(id, organizationId);

            if (task == null)
            {
                throw new Exception("Task not found.");
            }

            status = status.ToUpperInvariant();

            if (status == "COMPLETED")
            {
                task.CompletedAt = DateTime.UtcNow;
            }
            else
            {
                task.CompletedAt = null;
            }

            await _taskItemRepository.UpdateTask(task);
        }

        public async Task AssignTask(Guid id, Guid organizationId, Guid assignedUserId)
        {
            var organization = await _organizationRepository.GetOrganizationById(organizationId);

            if (organization == null)
            {
                throw new Exception("Organization not found.");
            }

            var task = await _taskItemRepository.GetTaskById(id, organizationId);

            if (task == null)
            {
                throw new Exception("Task not found.");
            }

            var assignedUser = await _userRepository.GetUserById(assignedUserId, organization);

            if (assignedUser == null)
            {
                throw new Exception("Assigned user not found in the organization.");
            }

            task.AssignedUserId = assignedUserId;

            await _taskItemRepository.UpdateTask(task);
        }


    }
}
