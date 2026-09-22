using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/pipelines")]
    [Authorize]
    public class PipelinesController : BaseController
    {
        private readonly IPipelinesServices _pipelinesServices;

        public PipelinesController(IPipelinesServices pipelinesServices)
        {
            _pipelinesServices = pipelinesServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllPipeline()
        {
            var pipelines = await _pipelinesServices.GetAllPipeline();
            return Success("Pipelines retrieved successfully.", pipelines);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{pipelinesId:guid}")]
        public async Task<IActionResult> GetPipelineById(Guid pipelinesId)
        {
            var pipeline = await _pipelinesServices.GetPipelineById(pipelinesId);
            return Success("Pipeline retrieved successfully.", pipeline);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePipeline(CreatePipelineDto pipeline)
        {
            await _pipelinesServices.CreatePipeline(pipeline);
            return Created("Pipeline created successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{pipelinesId:guid}")]
        public async Task<IActionResult> DeletePipeline(Guid pipelinesId)
        {
            await _pipelinesServices.DeletePipeline(pipelinesId);
            return Success("Pipeline deleted successfully.");
        }
    }
}