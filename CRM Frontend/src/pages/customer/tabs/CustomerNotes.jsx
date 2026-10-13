import { useState } from "react";
import { Button } from "../../../components/common/Button";
import { Modal } from "../../../components/common/Modal";
import { TableActions } from "../../../components/common/TableActions";
import { Dropdown } from "../../../components/common/Dropdown";
import { useNotes } from "../../../hooks/useNotes";
import { useOpportunities } from "../../../hooks/useOpportunities";
import {createNotes,updateNote,deleteNote } from "../../../services/core/noteService";
import { CustomerNoteValidator } from "./validator/CustomerNoteValidator";

export const CustomerNotes = ({ customer }) => {
    // Selected note.
    const [selectedNote, setSelectedNote] = useState(null);

    // Modal states.
    const [isNoteModalOpen, setIsNoteModalOpen] = useState(false);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

    // Submission states.
    const [isEditMode, setIsEditMode] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [isDeleting, setIsDeleting] = useState(false);

    // Error states.
    const [submitError, setSubmitError] = useState("");
    const [deleteError, setDeleteError] = useState("");
    const [validationErrors, setValidationErrors] = useState({});

    // Note form data.
    const [formData, setFormData] = useState({
        content: "",
        opportunityId: ""
    });

    // Fetch customer notes.
    const {
        notes = [],
        loading: isLoadingNotes,
        error: notesError,
        refetch: refetchNotes
    } = useNotes(customer?.id);

    // Fetch opportunities belonging to this customer.
    const {
        opportunities = [],
        loading: isLoadingOpportunities
    } = useOpportunities(customer?.id);

    // Opportunity dropdown options.
    const opportunityOptions = opportunities.map((opportunity) => ({
        value: opportunity.id,
        label: opportunity.name
    }));

    // Extract a readable API error.
    const getErrorMessage = (error, fallbackMessage) => {
        const data = error.response?.data;

        const validationErrors = data?.errors
            ? Object.values(data.errors).flat()
            : [];

        return (
            validationErrors[0] ||
            data?.message ||
            data?.Message ||
            data?.title ||
            fallbackMessage
        );
    };

    // Reset form data and errors.
    const resetForm = () => {
        setFormData({
            content: "",
            opportunityId: ""
        });

        setSelectedNote(null);
        setIsEditMode(false);
        setSubmitError("");
        setValidationErrors({});
    };

    // Close create/edit modal.
    const closeNoteModal = () => {
        if (isSubmitting) return;

        setIsNoteModalOpen(false);
        resetForm();
    };

    // Close delete modal.
    const closeDeleteModal = () => {
        if (isDeleting) return;

        setIsDeleteModalOpen(false);
        setSelectedNote(null);
        setDeleteError("");
    };

    // Open create modal.
    const openCreateModal = () => {
        resetForm();
        setIsNoteModalOpen(true);
    };

    // Handle form inputs.
    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));

        setValidationErrors((prev) => ({
            ...prev,
            [name]: ""
        }));

        setSubmitError("");
    };

    // Create or update a note.
    const handleSubmit = async (e) => {
        e.preventDefault();

        const errors = CustomerNoteValidator(formData, customer?.id);

        setValidationErrors(errors);
        setSubmitError("");

        if (Object.keys(errors).length > 0) {
            return;
        }

        setIsSubmitting(true);

        try {
            if (isEditMode && selectedNote?.id) {
                // Update existing note content.
                await updateNote(selectedNote.id, {
                    content: formData.content.trim()
                });
            } else {
                // Create a note for this customer.
                await createNotes({
                    customerId: customer.id,
                    leadId: null,
                    opportunityId: formData.opportunityId || null,
                    content: formData.content.trim()
                });
            }

            await refetchNotes();

            setIsNoteModalOpen(false);
            resetForm();
        } catch (error) {
            console.error("Failed to save note:", error);
            console.error("API response:", error.response?.data);

            setSubmitError(
                getErrorMessage(
                    error,
                    isEditMode
                        ? "Failed to update note."
                        : "Failed to create note."
                )
            );
        } finally {
            setIsSubmitting(false);
        }
    };

    // Open an existing note for editing.
    const openEditModal = (note) => {
        resetForm();

        setSelectedNote(note);
        setIsEditMode(true);

        setFormData({
            content: note.content ?? "",
            opportunityId:
                note.opportunity?.id ??
                note.opportunityId ??
                ""
        });

        setSubmitError("");
        setValidationErrors({});
        setIsNoteModalOpen(true);
    };

    // Open delete confirmation.
    const openDeleteModal = (note) => {
        setSelectedNote(note);
        setDeleteError("");
        setIsDeleteModalOpen(true);
    };

    // Delete selected note.
    const handleDeleteSubmit = async () => {
        if (!selectedNote?.id) return;

        setIsDeleting(true);
        setDeleteError("");

        try {
            await deleteNote(selectedNote.id);

            await refetchNotes();

            setIsDeleteModalOpen(false);
            setSelectedNote(null);
        } catch (error) {
            console.error("Failed to delete note:", error);

            setDeleteError(
                getErrorMessage(error, "Failed to delete note.")
            );
        } finally {
            setIsDeleting(false);
        }
    };

    // Format date.
    const formatDate = (date) => {
        if (!date) return "Date unavailable";

        const parsedDate = new Date(date);

        if (Number.isNaN(parsedDate.getTime())) {
            return "Date unavailable";
        }

        return parsedDate.toLocaleString("en-PH", {
            year: "numeric",
            month: "short",
            day: "numeric",
            hour: "numeric",
            minute: "2-digit"
        });
    };

    // Sort newest notes first.
    const sortedNotes = [...notes].sort((a, b) => {
        const dateA = new Date(
            a.createdAt ?? a.createdDate ?? 0
        ).getTime();

        const dateB = new Date(
            b.createdAt ?? b.createdDate ?? 0
        ).getTime();

        return dateB - dateA;
    });

    // Shared input styling.
    const inputClass = "w-full rounded-md border border-border bg-slate-100 px-3 py-2 text-text placeholder:text-text-muted focus:border-primary focus:outline-none focus:ring-2 focus:ring-primary";

    return (
        <div className="flex min-w-0 flex-col gap-4">
            {/* Header */}
            <div className="flex items-center justify-between gap-3">
                <div className="flex min-w-0 flex-col">
                    <h3 className="text-lg font-semibold text-text">
                        Notes
                    </h3>

                    <span className="text-sm text-text-secondary">
                        Keep track of important customer information.
                    </span>
                </div>

                <Button
                    type="button"
                    onClick={openCreateModal}
                    disabled={!customer?.id}
                >
                    +
                </Button>
            </div>

            {/* Notes list */}
            {isLoadingNotes ? (
                <p className="py-6 text-center text-text-secondary">
                    Loading notes...
                </p>
            ) : notesError ? (
                <div className="py-6 text-center">
                    <p className="mb-3 text-sm text-red-500">
                        {notesError}
                    </p>

                    <Button
                        type="button"
                        variant="primary"
                        onClick={refetchNotes}
                    >
                        Retry
                    </Button>
                </div>
            ) : notes.length === 0 ? (
                <div className="rounded-lg border border-border p-6 text-center">
                    <p className="text-text-secondary">
                        No notes available for this customer.
                    </p>

                    <p className="mt-1 text-sm text-text-muted">
                        Add a note to keep track of important details.
                    </p>
                </div>
            ) : (
                <div className="flex min-w-0 flex-col gap-3">
                    {sortedNotes.map((note) => {
                        const actions = [
                            {
                                label: "Edit",
                                onClick: () => openEditModal(note)
                            },
                            {
                                label: "Delete",
                                variant: "danger",
                                onClick: () => openDeleteModal(note)
                            }
                        ];

                        return (
                            <div
                                key={note.id}
                                className="flex min-w-0 flex-col gap-3 rounded-lg border border-border p-4 transition-colors hover:bg-surface"
                            >
                                <div className="flex min-w-0 items-start justify-between gap-3">
                                    <p className="min-w-0 whitespace-pre-wrap break-words text-sm text-text">
                                        {note.content}
                                    </p>

                                    <TableActions options={actions} />
                                </div>

                                <div className="flex flex-wrap items-center gap-2 border-t border-border pt-3">
                                    <span className="text-xs text-text-secondary">
                                        {formatDate(
                                            note.createdAt ??
                                            note.createdDate
                                        )}
                                    </span>

                                    {(note.opportunity?.name ||
                                        note.opportunityId) && (
                                        <span className="rounded-full bg-slate-100 px-3 py-1 text-xs text-slate-700">
                                            Opportunity:{" "}
                                            {note.opportunity?.name ??
                                                "Linked opportunity"}
                                        </span>
                                    )}
                                </div>
                            </div>
                        );
                    })}
                </div>
            )}

            {/* Create / Edit Note Modal */}
            <Modal
                isOpen={isNoteModalOpen}
                onClose={closeNoteModal}
                title={
                    isEditMode ? "Edit Note" : "Add Customer Note"
                }
                className="w-full max-w-xl"
            >
                <form
                    className="flex max-h-[75vh] flex-col gap-4 overflow-y-auto"
                    onSubmit={handleSubmit}
                >
                    {/* Customer */}
                    <div className="flex flex-col gap-1">
                        <label className="text-sm font-medium text-text">
                            Customer
                        </label>

                        <p className="text-sm text-text-secondary">
                            {customer?.firstName} {customer?.lastName}
                        </p>
                    </div>

                    {/* Opportunity association */}
                    <div className="flex flex-col gap-2">
                        <label className="text-sm font-medium text-text">
                            Related Opportunity (Optional)
                        </label>

                        <Dropdown
                            label={
                                isLoadingOpportunities
                                    ? "Loading opportunities..."
                                    : "No opportunity selected"
                            }
                            value={formData.opportunityId}
                            onChange={(value) => {
                                setFormData((prev) => ({
                                    ...prev,
                                    opportunityId: value
                                }));

                                setSubmitError("");
                            }}
                            options={opportunityOptions}
                            disabled={
                                isLoadingOpportunities ||
                                opportunityOptions.length === 0
                            }
                            className="w-full"
                        />

                        <p className="text-xs text-text-secondary">
                            Leave empty to create a note for the customer
                            without linking it to an opportunity.
                        </p>
                    </div>

                    {/* Note content */}
                    <div className="flex flex-col gap-2">
                        <label
                            htmlFor="note-content"
                            className="text-sm font-medium text-text"
                        >
                            Note Content
                        </label>

                        <textarea
                            id="note-content"
                            name="content"
                            value={formData.content}
                            onChange={handleChange}
                            placeholder="Enter important customer information..."
                            rows={5}
                            maxLength={5000}
                            className={`${inputClass} resize-y`}
                        />

                        <div className="flex justify-between gap-2">
                            {validationErrors.content ? (
                                <p className="text-xs text-red-500">
                                    {validationErrors.content}
                                </p>
                            ) : (
                                <span />
                            )}

                            <span className="text-xs text-text-secondary">
                                {formData.content.length}/5000
                            </span>
                        </div>
                    </div>

                    {validationErrors.customerId && (
                        <p className="text-sm text-red-500">
                            {validationErrors.customerId}
                        </p>
                    )}

                    {submitError && (
                        <p className="text-sm text-red-500" role="alert">
                            {submitError}
                        </p>
                    )}

                    {/* Form actions */}
                    <div className="flex justify-end gap-2 border-t border-border pt-4">
                        <Button
                            type="button"
                            variant="secondary"
                            onClick={closeNoteModal}
                            disabled={isSubmitting}
                        >
                            Cancel
                        </Button>

                        <Button
                            type="submit"
                            variant="primary"
                            disabled={isSubmitting || !customer?.id}
                        >
                            {isSubmitting
                                ? "Saving..."
                                : isEditMode
                                    ? "Update Note"
                                    : "Create Note"}
                        </Button>
                    </div>
                </form>
            </Modal>

            {/* Delete Note Confirmation Modal */}
            <Modal
                isOpen={isDeleteModalOpen}
                onClose={closeDeleteModal}
                title="Delete Note"
                className="w-full max-w-md"
            >
                <div className="flex flex-col gap-4">
                    <p className="text-sm text-text-secondary">
                        Are you sure you want to delete this note? This
                        action cannot be undone.
                    </p>

                    {selectedNote?.content && (
                        <div className="max-h-40 overflow-y-auto whitespace-pre-wrap break-words rounded-md border border-border p-3 text-sm text-text">
                            {selectedNote.content}
                        </div>
                    )}

                    {deleteError && (
                        <p className="text-sm text-red-500" role="alert">
                            {deleteError}
                        </p>
                    )}

                    <div className="flex justify-end gap-2 border-t border-border pt-4">
                        <Button
                            type="button"
                            variant="secondary"
                            onClick={closeDeleteModal}
                            disabled={isDeleting}
                        >
                            Cancel
                        </Button>

                        <Button
                            type="button"
                            variant="danger"
                            onClick={handleDeleteSubmit}
                            disabled={isDeleting}
                        >
                            {isDeleting ? "Deleting..." : "Delete Note"}
                        </Button>
                    </div>
                </div>
            </Modal>
        </div>
    );
};
