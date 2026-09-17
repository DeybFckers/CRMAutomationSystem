using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/pipelines/{pipelineId:guid}/stages")]
    [Authorize]
    public class PipelineStagesController : ControllerBase
    {
        private readonly IPipelineStageServices _pipelineStageServices;

        public PipelineStagesController(IPipelineStageServices pipelineStageServices)
        {
            _pipelineStageServices = pipelineStageServices;
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpPost]
        public async Task<IActionResult> CreatePipelineStage(Guid pipelineId, CreatePipelineStageDto pipelinestage)
        {
            await _pipelineStageServices.CreatePipelineStage(pipelinestage, pipelineId);

            return StatusCode(StatusCodes.Status201Created, new
            {
                message = "Pipeline stage created successfully."
            });
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PipelineStageResponseDto>>> GetAllPipelineStages(Guid pipelineId)
        {
            var stages = await _pipelineStageServices.GetAllPipelineStage(pipelineId);

            return Ok(stages);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("{stageId:guid}")]
        public async Task<ActionResult<PipelineStageResponseDto>> GetPipelineStage(Guid pipelineId, Guid stageId)
        {
            try
            {
                var stage = await _pipelineStageServices.GetPipelineStage(stageId, pipelineId);

                return Ok(stage);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Pipeline stage not found."
                });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{stageId:guid}")]
        public async Task<IActionResult> DeletePipelineStage(Guid pipelineId, Guid stageId)
        {
            try
            {
                await _pipelineStageServices.DeletePipelineStage(stageId, pipelineId);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Pipeline stage not found."
                });
            }
        }
    }
}