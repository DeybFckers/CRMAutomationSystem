using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/pipelines/{pipelineId:guid}/stages")]
    [Authorize]
    public class PipelineStagesController : BaseController
    {
        private readonly IPipelineStageServices _pipelineStageServices;

        public PipelineStagesController(IPipelineStageServices pipelineStageServices)
        {
            _pipelineStageServices = pipelineStageServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpPost]
        public async Task<IActionResult> CreatePipelineStage(Guid pipelineId, CreatePipelineStageDto pipelinestage)
        {
            await _pipelineStageServices.CreatePipelineStage(pipelinestage, pipelineId);
            return Created("Pipeline stage created successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllPipelineStages(Guid pipelineId)
        {
            var stages = await _pipelineStageServices.GetAllPipelineStage(pipelineId);
            return Success("Pipeline stages retrieved successfully.", stages);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{stageId:guid}")]
        public async Task<IActionResult> GetPipelineStage(Guid pipelineId, Guid stageId)
        {
            var stage = await _pipelineStageServices.GetPipelineStage(stageId, pipelineId);
            return Success("Pipeline stage retrieved successfully.", stage);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{stageId:guid}")]
        public async Task<IActionResult> DeletePipelineStage(Guid pipelineId, Guid stageId)
        {
            await _pipelineStageServices.DeletePipelineStage(stageId, pipelineId);
            return Success("Pipeline stage deleted successfully.");
        }
    }
}