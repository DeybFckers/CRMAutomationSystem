using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NoteController : BaseController
    {
        private readonly INoteServices _noteServices;

        public NoteController(INoteServices noteServices)
        {
            _noteServices = noteServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateNoteDto dto)
        {
            var note = await _noteServices.CreateNote(dto);

            return Created("Note created successfully.", note);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteServices.GetAllNotes();

            return Success("Notes retrieved successfully.", notes);
        }

        [HttpGet("{noteId:guid}")]
        public async Task<IActionResult> GetNoteById(Guid noteId)
        {
            var note = await _noteServices.GetNoteById(noteId);

            if (note == null)
                throw new KeyNotFoundException("Note not found.");

            return Success("Note retrieved successfully.", note);
        }

        [HttpPut("{noteId:guid}")]
        public async Task<IActionResult> UpdateNote(Guid noteId, UpdateNoteDto dto)
        {
            var note = await _noteServices.UpdateNote(noteId, dto);

            return Success("Note updated successfully.", note);
        }

        [HttpDelete("{noteId:guid}")]
        public async Task<IActionResult> DeleteNote(Guid noteId)
        {
            await _noteServices.DeleteNote(noteId);

            return Success("Note deleted successfully.");
        }
    }
}