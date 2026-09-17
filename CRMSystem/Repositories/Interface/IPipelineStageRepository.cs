using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface IPipelineStageRepository
    {
        Task<IEnumerable<PipelineStage>> GetAllPipelineStages(Guid pipelineId);
        Task<PipelineStage?> GetPipelineStageById(Guid id, Guid pipelineId);
        Task CreatePipelineStage(PipelineStage stage);
        Task DeletePipelineStage(Guid id, Guid pipelineId);
    }
}