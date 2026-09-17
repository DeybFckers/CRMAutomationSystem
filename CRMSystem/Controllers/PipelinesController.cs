using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/pipelines")]
    [Authorize]
    public class PipelinesController : ControllerBase
    {
        private readonly IPipelinesServices _pipelinesServices;

        public PipelinesController(IPipelinesServices pipelinesServices)
        {
            _pipelinesServices = pipelinesServices;
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PipelineResponseDto>>> GetAllPipeline()
        {
            var pipelines = await _pipelinesServices.GetAllPipeline();

            return Ok(pipelines);
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PipelineResponseDto>> GetPipelineById(Guid id)
        {
            try
            {
                var pipeline = await _pipelinesServices.GetPipelineById(id);

                return Ok(pipeline);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Pipeline not found."
                });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePipeline(CreatePipelineDto pipeline)
        {
            await _pipelinesServices.CreatePipeline(pipeline);

            return StatusCode(StatusCodes.Status201Created, new
            {
                message = "Pipeline created successfully."
            });
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePipeline(Guid id)
        {
            try
            {
                await _pipelinesServices.DeletePipeline(id);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Pipeline not found."
                });
            }
        }
    }
}