using CRMSystem.Models.Entities;

namespace CRMSystem.Services.Interface
{
    public interface IAutomationServices
    {
        Task OrganizationCreated(Organization organization);
    }
}
