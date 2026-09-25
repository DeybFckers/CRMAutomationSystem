using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface IPipelinesRepository
    {
        Task<IEnumerable<Pipeline>> GetAllPipelines(Guid organizationId);
        Task<Pipeline?> GetPipelineById(Guid id, Guid organizationId);
        Task CreatePipeline(Pipeline pipeline);
        Task DeletePipeline(Guid id, Guid organizationId);
    }
}