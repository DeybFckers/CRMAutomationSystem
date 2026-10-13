import api from "../api";

// Get all notes belonging to a customer.
export const getNotesByCustomerId = async (customerId) => {
    const response = await api.get( `/api/notes/customer/${customerId}`);

    return response.data.data;
};

// Create a note.
export const createNotes = async (noteData) => {
    const response = await api.post("/api/notes", noteData);

    return response.data;
};

// Update an existing note.
export const updateNote = async (noteId, noteData) => {
    const response = await api.put(`/api/notes/${noteId}`, noteData);

    return response.data;
};

// Delete a note.
export const deleteNote = async (noteId) => {
    const response = await api.delete(`/api/notes/${noteId}`);

    return response.data;
};
