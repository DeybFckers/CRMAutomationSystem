using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface IPipelinesServices
    {
        Task<IEnumerable<PipelineResponseDto>> GetAllPipeline();
        Task<PipelineResponseDto> GetPipelineById(Guid id);
        Task CreatePipeline(CreatePipelineDto pipeline);
        Task DeletePipeline(Guid id);
    }
}