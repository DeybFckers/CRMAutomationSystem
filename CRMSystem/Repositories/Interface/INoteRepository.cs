using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface INoteRepository
    {
        Task CreateNote(Note note);
        Task DeleteNote(Guid organizationId, Guid noteId);
        Task<Note?> GetNoteById(Guid organizationId, Guid noteId);
        Task<IEnumerable<Note>> GetAllNotes(Guid organizationId);
        Task UpdateNote(Note note);
    }

}
