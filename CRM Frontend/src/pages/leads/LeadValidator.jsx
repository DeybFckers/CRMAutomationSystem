export const LeadValidator = (formData) => {
    const errors = {};
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const estimatedValue = Number(formData.estimatedValue);

    if (!formData.firstName.trim()) {
        errors.firstName = "First name is required.";
    }

    if (!formData.lastName.trim()) {
        errors.lastName = "Last name is required.";
    }

    if (!formData.email.trim()) {
        errors.email = "Email is required.";
    } else if (!emailRegex.test(formData.email)) {
        errors.email = "Please enter a valid email address.";
    }

    if (!formData.phone.trim()) {
        errors.phone = "Phone number is required.";
    }

    if (!formData.companyName.trim()) {
        errors.companyName = "Company name is required.";
    }

    if (!formData.estimatedValue) {
        errors.estimatedValue = "Estimated value is required.";
    } else if (estimatedValue <= 0) {
        errors.estimatedValue = "Estimated value must be greater than 0.";
    }

    if (!formData.statusId) {
        errors.statusId = "Status is required.";
    }

    if (!formData.sourceId) {
        errors.sourceId = "Source is required.";
    }

    if (!formData.assignedUserId) {
        errors.assignedUserId = "Sales representative is required.";
    }

    return errors;
};