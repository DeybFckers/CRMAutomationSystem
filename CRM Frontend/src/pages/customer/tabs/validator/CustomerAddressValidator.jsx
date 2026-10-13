

export const CustomerAddressValidator = (formData) => {
    const errors = {};

    if (!formData.type.trim()) {
        errors.type = "Address type is required.";
    }

    if (!formData.addressLine.trim()) {
        errors.addressLine = "Address line is required.";
    }

    if (!formData.city.trim()) {
        errors.city = "City is required.";
    }

    if (!formData.province.trim()) {
        errors.province = "Province is required.";
    }

    if (!formData.postalCode.trim()) {
        errors.postalCode = "Postal code is required.";
    }

    if (!formData.country.trim()) {
        errors.country = "Country is required.";
    }

    return errors;
};