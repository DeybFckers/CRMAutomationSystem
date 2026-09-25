using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class PipelineStageRepository : IPipelineStageRepository
    {
        private readonly ApplicationDbContext _context;

        public PipelineStageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PipelineStage>> GetAllPipelineStages(Guid pipelineId)
        {
            return await _context.PipelineStages
                .Where(x => x.PipelineId == pipelineId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

        public async Task<PipelineStage?> GetPipelineStageById(Guid id, Guid pipelineId)
        {
            return await _context.PipelineStages
                .FirstOrDefaultAsync(x => x.Id == id && x.PipelineId == pipelineId);
        }

        public async Task CreatePipelineStage(PipelineStage stage)
        {
            await _context.PipelineStages.AddAsync(stage);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePipelineStage(Guid id, Guid pipelineId)
        {
            var stage = await _context.PipelineStages
                .FirstOrDefaultAsync(x => x.Id == id && x.PipelineId == pipelineId);

            if (stage == null)
                throw new KeyNotFoundException("Pipeline stage not found.");

            _context.PipelineStages.Remove(stage);
            await _context.SaveChangesAsync();
        }
    }
}