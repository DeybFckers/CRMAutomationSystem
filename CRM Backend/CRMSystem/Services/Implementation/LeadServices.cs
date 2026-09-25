using CRMSystem.Data;
using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Services.Implementation
{
    public class LeadServices : ILeadServices
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ICustomerCodeServices _customerCodeServices;
        private readonly ILeadSourceRepository _leadSourceRepository;
        private readonly ILeadStatusRepository _leadStatusRepository;

        public LeadServices(
            ILeadRepository leadRepository,
            ICurrentUserServices currentUserServices,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ICustomerRepository customerRepository,
            IOrganizationRepository organizationRepository,   
            ICustomerCodeServices customerCodeServices,
            ILeadSourceRepository leadSourceRepository,
            ILeadStatusRepository leadStatusRepository)
        {
            _leadRepository = leadRepository;
            _currentUserServices = currentUserServices;
            _context = context;
            _userManager = userManager;
            _customerRepository = customerRepository;
            _organizationRepository = organizationRepository; 
            _customerCodeServices = customerCodeServices;
            _leadSourceRepository = leadSourceRepository;
            _leadStatusRepository = leadStatusRepository;
        }


        public async Task<IEnumerable<LeadResponseDto>> GetAllLead()
        {
            var organizationId = _currentUserServices.OrganizationId;

            var leads = await _leadRepository.GetAllLeads(organizationId);

            return leads.Adapt<IEnumerable<LeadResponseDto>>();
        }

        public async Task<LeadResponseDto> GetLeadById(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var lead = await _leadRepository.GetLeadById(id, organizationId);

            if (lead == null)
                throw new KeyNotFoundException("Lead not found.");

            return lead.Adapt<LeadResponseDto>();
        }

        public async Task CreateLead(CreateLeadDto lead)
        {
            var organizationId = _currentUserServices.OrganizationId;
            var currentUserId = _currentUserServices.UserId;
            var currentUserRoles = _currentUserServices.Roles;

            var sourceExists = await _leadSourceRepository.GetLeadSourceById(lead.SourceId, organizationId);
            if(sourceExists == null)
                throw new KeyNotFoundException("Lead source not found.");

            var statusExists = await _leadStatusRepository.GetLeadStatusById(lead.StatusId, organizationId);
            if(statusExists == null)
                throw new KeyNotFoundException("Lead status not found.");

            var newLead = lead.Adapt<Lead>();

            newLead.Id = Guid.NewGuid();
            newLead.OrganizationId = organizationId;
            newLead.CreatedByUserId = currentUserId;
            newLead.CreatedAt = DateTime.UtcNow;
            newLead.UpdatedAt = DateTime.UtcNow;

            if (lead.AssignedUserId.HasValue)
            {
                if (!currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("SalesManager"))
                    throw new UnauthorizedAccessException("You are not allowed to assign a lead to another user.");

                await ValidateAssignedUser(lead.AssignedUserId.Value, organizationId);

                newLead.AssignedUserId = lead.AssignedUserId.Value;
            }
            else
            {
                if (currentUserRoles.Contains("Admin"))
                {
                    newLead.AssignedUserId = null;
                }
                else
                {
                    newLead.AssignedUserId = currentUserId;
                }
            }

            await _leadRepository.CreateLead(newLead);
        }

        public async Task<LeadResponseDto> UpdateLead(Guid id, UpdateLeadDto lead)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var existingLead = await _leadRepository.GetLeadById(id, organizationId);

            if (existingLead == null)
                throw new KeyNotFoundException("Lead not found.");

            var sourceExists = await _leadSourceRepository.GetLeadSourceById(lead.SourceId, organizationId);
            if (sourceExists == null)
                throw new KeyNotFoundException("Lead source not found.");

            var statusExists = await _leadStatusRepository.GetLeadStatusById(lead.StatusId, organizationId);
            if (statusExists == null)
                throw new KeyNotFoundException("Lead status not found.");


            lead.Adapt(existingLead);

            existingLead.Id = id;
            existingLead.OrganizationId = organizationId;
            existingLead.UpdatedAt = DateTime.UtcNow;

            await _leadRepository.UpdateLead(existingLead);

            return existingLead.Adapt<LeadResponseDto>();
        }

        public async Task<LeadResponseDto> AssignLead(Guid id, AssignLeadDto lead)
        {
            var organizationId = _currentUserServices.OrganizationId;
            var currentUserRoles = _currentUserServices.Roles;

            if (!currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("SalesManager"))
                throw new UnauthorizedAccessException("You are not allowed to assign leads.");

            var existingLead = await _leadRepository.GetLeadById(id, organizationId);

            if (existingLead == null)
                throw new KeyNotFoundException("Lead not found.");

            await ValidateAssignedUser(lead.AssignedUserId, organizationId);

            existingLead.AssignedUserId = lead.AssignedUserId;

            await _leadRepository.UpdateLead(existingLead);

            return existingLead.Adapt<LeadResponseDto>();
        }

        public async Task<LeadResponseDto> UpdateLeadStatus(Guid id, UpdateLeadStatusDto lead)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var existingLead = await _leadRepository.GetLeadById(id, organizationId);

            if (existingLead == null)
                throw new KeyNotFoundException("Lead not found.");

            var statusExists = await _context.LeadStatuses
                .AnyAsync(x => x.Id == lead.StatusId && x.OrganizationId == organizationId);

            if (!statusExists)
                throw new KeyNotFoundException("Lead status not found.");

            existingLead.StatusId = lead.StatusId;

            await _leadRepository.UpdateLead(existingLead);

            return existingLead.Adapt<LeadResponseDto>();
        }

        public async Task DeleteLead(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId;

            await _leadRepository.DeleteLead(id, organizationId);
        }

        private async Task ValidateAssignedUser(Guid assignedUserId, Guid organizationId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == assignedUserId && x.OrganizationId == organizationId);

            if (user == null)
                throw new KeyNotFoundException("Assigned user not found.");
        }

        public async Task ConvertLeadToCustomer(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId; 
            var lead = await _leadRepository.GetLeadById(id, organizationId); 
            if (lead == null) throw new KeyNotFoundException("Lead not found."); 
            if (lead.ConvertedCustomerId.HasValue)
                throw new InvalidOperationException("This lead has already been converted to a customer."); 

            var convertedStatus = await _leadRepository.GetStatusByName(organizationId, "Converted"); 
            if (convertedStatus == null) throw new KeyNotFoundException("Converted lead status not found.");

            var organization = await _organizationRepository.GetOrganizationById(organizationId); 
            if (organization == null) throw new KeyNotFoundException("Organization not found."); 

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                AssignedUserId = lead.AssignedUserId,
                FirstName = lead.FirstName,
                LastName = lead.LastName,
                CompanyName = lead.CompanyName,
                Email = lead.Email,
                Phone = lead.Phone,
                CustomerCode = await _customerCodeServices.GenerateCustomerCode(organization.Id, organization.Name, DateTime.UtcNow),
                Status = "ACTIVE",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            lead.ConvertedCustomerId = customer.Id; 
            lead.ConvertedAt = DateTime.UtcNow;
            lead.StatusId = convertedStatus.Id; 
            lead.UpdatedAt = DateTime.UtcNow;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            _customerRepository.AddCustomer(customer);
            _leadRepository.UpdateLeadNoSave(lead);      

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}