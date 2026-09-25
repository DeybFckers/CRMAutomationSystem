using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface INoteServices
    {
        Task<NoteResponseDto> CreateNote(CreateNoteDto dto);
        Task DeleteNote(Guid noteId);
        Task<NoteResponseDto?> GetNoteById(Guid noteId);
        Task<IEnumerable<NoteResponseDto>> GetAllNotes();
        Task<NoteResponseDto> UpdateNote(Guid noteId, UpdateNoteDto dto);
    }
}