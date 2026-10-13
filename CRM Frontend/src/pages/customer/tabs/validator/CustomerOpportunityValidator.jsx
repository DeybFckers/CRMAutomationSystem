
export const validateOpportunity = (formData, customerId) => {
    const errors = {};

    // Validate customer.
    if (!customerId) {
        errors.customerId = "Customer information is missing.";
    }

    // Validate opportunity name.
    if (!formData.name.trim()) {
        errors.name = "Opportunity name is required.";
    }

    // Validate pipeline.
    if (!formData.pipelineId) {
        errors.pipelineId = "Please select a pipeline.";
    }

    // Validate stage.
    if (!formData.stageId) {
        errors.stageId = "Please select a stage.";
    }

    // Validate opportunity value.
    const opportunityValue = Number(formData.value);

    if (
        formData.value === "" ||
        !Number.isFinite(opportunityValue) ||
        opportunityValue < 0
    ) {
        errors.value = "Please enter a valid opportunity value.";
    }

    return errors;
};
