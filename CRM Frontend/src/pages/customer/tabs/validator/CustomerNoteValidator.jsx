
export const CustomerNoteValidator = (formData, customerId) => {
    const errors = {};

    if (!customerId) {
        errors.customerId = "A customer must be selected.";
    }

    if (!formData.content?.trim()) {
        errors.content = "Note content is required.";
    } else if (formData.content.trim().length > 5000) {
        errors.content = "Note content cannot exceed 5000 characters.";
    }

    return errors;
};
