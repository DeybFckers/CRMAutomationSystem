using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class PipelineStageServices : IPipelineStageServices
    {
        private readonly IPipelineStageRepository _pipelineStageRepository;

        public PipelineStageServices(IPipelineStageRepository pipelineStageRepository)
        {
            _pipelineStageRepository = pipelineStageRepository;
        }

        public async Task<IEnumerable<PipelineStageResponseDto>> GetAllPipelineStage(Guid pipelineId)
        {
            var stages = await _pipelineStageRepository.GetAllPipelineStages(pipelineId);

            return stages.Adapt<IEnumerable<PipelineStageResponseDto>>();
        }

        public async Task<PipelineStageResponseDto> GetPipelineStage(Guid id, Guid pipelineId)
        {
            var stage = await _pipelineStageRepository.GetPipelineStageById(id, pipelineId);

            if (stage == null)
                throw new KeyNotFoundException("Pipeline stage not found.");

            return stage.Adapt<PipelineStageResponseDto>();
        }

        public async Task CreatePipelineStage(CreatePipelineStageDto pipelinestage, Guid pipelineId)
        {
            var stage = pipelinestage.Adapt<PipelineStage>();

            stage.Id = Guid.NewGuid();
            stage.PipelineId = pipelineId;

            await _pipelineStageRepository.CreatePipelineStage(stage);
        }

        public async Task DeletePipelineStage(Guid id, Guid pipelineId)
        {
            await _pipelineStageRepository.DeletePipelineStage(id, pipelineId);
        }
    }
}