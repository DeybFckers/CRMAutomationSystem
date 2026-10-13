export const CustomerContactValidator = (formData) => {
    const errors = {};

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const phoneRegex = /^[0-9+\-().\s]+$/;

    const firstName = (formData.firstName ?? "").trim();
    const lastName = (formData.lastName ?? "").trim();
    const email = (formData.email ?? "").trim();
    const phone = (formData.phone ?? "").trim();
    const position = (formData.position ?? "").trim();

    if (!firstName) {
        errors.firstName = "First name is required.";
    }

    if (!lastName) {
        errors.lastName = "Last name is required.";
    }

    if (!email) {
        errors.email = "Email is required.";
    } else if (!emailRegex.test(email)) {
        errors.email = "Please enter a valid email address.";
    }

    if (!phone) {
        errors.phone = "Phone number is required.";
    } else if (!phoneRegex.test(phone) || !/[0-9]/.test(phone)) {
        errors.phone = "Please enter a valid phone number.";
    }

    if (!position) {
        errors.position = "Position is required.";
    }

    return errors;
};