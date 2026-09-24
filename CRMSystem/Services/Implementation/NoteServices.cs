using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class NoteServices : INoteServices
    {
        private readonly INoteRepository _noteRepository;
        private readonly ICurrentUserServices _currentUserServices;

        public NoteServices(INoteRepository noteRepository, ICurrentUserServices currentUserServices)
        {
            _noteRepository = noteRepository;
            _currentUserServices = currentUserServices;
        }

        public async Task<NoteResponseDto> CreateNote(CreateNoteDto dto)
        {
            var organizationId = _currentUserServices.OrganizationId;
            var userId = _currentUserServices.UserId;

            var note = dto.Adapt<Note>();
            note.Id = Guid.NewGuid();
            note.OrganizationId = organizationId;
            note.UserId = userId;
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.CreateNote(note);

            return note.Adapt<NoteResponseDto>();
        }

        public async Task DeleteNote(Guid noteId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var note = await _noteRepository.GetNoteById(organizationId, noteId);

            if (note == null)
                throw new KeyNotFoundException("Note not found.");

            await _noteRepository.DeleteNote(organizationId, noteId);
        }

        public async Task<NoteResponseDto?> GetNoteById(Guid noteId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var note = await _noteRepository.GetNoteById(organizationId, noteId);

            return note?.Adapt<NoteResponseDto>();
        }

        public async Task<IEnumerable<NoteResponseDto>> GetAllNotes()
        {
            var organizationId = _currentUserServices.OrganizationId;

            var notes = await _noteRepository.GetAllNotes(organizationId);

            return notes.Adapt<IEnumerable<NoteResponseDto>>();
        }

        public async Task<NoteResponseDto> UpdateNote(Guid noteId, UpdateNoteDto dto)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var note = await _noteRepository.GetNoteById(organizationId, noteId);

            if (note == null)
                throw new KeyNotFoundException("Note not found.");

            dto.Adapt(note);
            note.UpdatedAt = DateTime.UtcNow;

            await _noteRepository.UpdateNote(note);

            return note.Adapt<NoteResponseDto>();
        }
    }
}