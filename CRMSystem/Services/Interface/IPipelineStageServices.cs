using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface IPipelineStageServices
    {
        Task<IEnumerable<PipelineStageResponseDto>> GetAllPipelineStage(Guid pipelineId);
        Task<PipelineStageResponseDto> GetPipelineStage(Guid id, Guid pipelineId);
        Task CreatePipelineStage(CreatePipelineStageDto pipelinestage, Guid pipelineId);
        Task DeletePipelineStage(Guid id, Guid pipelineId);
    }
}