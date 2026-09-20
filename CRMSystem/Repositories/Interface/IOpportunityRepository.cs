using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface IOpportunityRepository
    {
        Task<IEnumerable<Opportunity>> GetAllOpportunity();
        Task<Opportunity> GetOpportunityById(Guid id);
        Task CreateOpportunity(Opportunity opportunity);    
        Task UpdateOpportunity(Opportunity opportunity);    
        Task DeleteOpportunity(Guid id);
    }
}
