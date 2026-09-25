using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface IOpportunityRepository
    {
        Task<IEnumerable<Opportunity>> GetAllOpportunity(Guid organizationId);
        Task<Opportunity?> GetOpportunityById(Guid id, Guid organizationId);
        Task CreateOpportunity(Opportunity opportunity);    
        Task UpdateOpportunity(Opportunity opportunity);    
        Task DeleteOpportunity(Guid id);
    }
}
