using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateNote(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNote(Guid organizationId, Guid noteId)
        {
            var note = await _context.Notes.FirstOrDefaultAsync(n => n.OrganizationId == organizationId && n.Id == noteId);

            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Note?> GetNoteById(Guid organizationId, Guid noteId)
        {
            return await _context.Notes.FirstOrDefaultAsync(n => n.OrganizationId == organizationId && n.Id == noteId);
        }

        public async Task<IEnumerable<Note>> GetAllNotes(Guid organizationId)
        {
            return await _context.Notes.Where(n => n.OrganizationId == organizationId).ToListAsync();
        }

        public async Task UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }
    }
}