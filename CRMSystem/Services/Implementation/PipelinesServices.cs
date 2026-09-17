using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class PipelinesServices : IPipelinesServices
    {
        private readonly IPipelinesRepository _pipelinesRepository;
        private readonly ICurrentUserServices _currentUserServices;

        public PipelinesServices(IPipelinesRepository pipelinesRepository, ICurrentUserServices currentUserServices)
        {
            _pipelinesRepository = pipelinesRepository;
            _currentUserServices = currentUserServices;
        }

        public async Task<IEnumerable<PipelineResponseDto>> GetAllPipeline()
        {
            var organizationId = _currentUserServices.OrganizationId;

            var pipelines = await _pipelinesRepository.GetAllPipelines(organizationId);

            return pipelines.Adapt<IEnumerable<PipelineResponseDto>>();
        }

        public async Task<PipelineResponseDto> GetPipelineById(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var pipeline = await _pipelinesRepository.GetPipelineById(id, organizationId);

            if (pipeline == null)
                throw new KeyNotFoundException("Pipeline not found.");

            return pipeline.Adapt<PipelineResponseDto>();
        }

        public async Task CreatePipeline(CreatePipelineDto pipeline)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var newPipeline = pipeline.Adapt<Pipeline>();

            newPipeline.Id = Guid.NewGuid();
            newPipeline.OrganizationId = organizationId;

            await _pipelinesRepository.CreatePipeline(newPipeline);
        }

        public async Task DeletePipeline(Guid id)
        {
            var organizationId = _currentUserServices.OrganizationId;

            await _pipelinesRepository.DeletePipeline(id, organizationId);
        }
    }
}