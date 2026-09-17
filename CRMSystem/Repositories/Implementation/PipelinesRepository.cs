using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class PipelinesRepository : IPipelinesRepository
    {
        private readonly ApplicationDbContext _context;

        public PipelinesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pipeline>> GetAllPipelines(Guid organizationId)
        {
            return await _context.Pipelines
                .Where(x => x.OrganizationId == organizationId)
                .ToListAsync();
        }

        public async Task<Pipeline?> GetPipelineById(Guid id, Guid organizationId)
        {
            return await _context.Pipelines
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);
        }

        public async Task CreatePipeline(Pipeline pipeline)
        {
            await _context.Pipelines.AddAsync(pipeline);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePipeline(Guid id, Guid organizationId)
        {
            var pipeline = await _context.Pipelines
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (pipeline == null)
                throw new KeyNotFoundException("Pipeline not found.");

            _context.Pipelines.Remove(pipeline);
            await _context.SaveChangesAsync();
        }
    }
}