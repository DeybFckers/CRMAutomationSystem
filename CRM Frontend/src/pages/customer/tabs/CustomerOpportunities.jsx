
import { useState } from "react";
import { Button } from "../../../components/common/Button";
import { Modal } from "../../../components/common/Modal";
import { Dropdown } from "../../../components/common/Dropdown";
import { TableActions } from "../../../components/common/TableActions";
import { usePipelines } from "../../../hooks/usePipelines";
import { useStages } from "../../../hooks/useStages";
import { useUsers } from "../../../hooks/useUsers";
import { useOpportunities } from "../../../hooks/useOpportunities";
import {createOpportunity, updateOpportunity, updateStageOpportunity, reassigneduserOpportunity,updateStatusOpportunity,deleteOpportunity} from "../../../services/core/opportunityService";
import { CustomerOpportunityValidator } from "./validator/CustomerOpportunityValidator";
import { BriefcaseBusiness, Wallet } from "lucide-react";

export const CustomerOpportunities = ({ customer }) => {
    // Selected opportunity.
    const [selectedOpportunity, setSelectedOpportunity] = useState(null);

    // Modal states.
    const [isOpportunityModalOpen, setIsOpportunityModalOpen] = useState(false);
    const [isStageModalOpen, setIsStageModalOpen] = useState(false);
    const [isAssignModalOpen, setIsAssignModalOpen] = useState(false);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [isStatusModalOpen, setIsStatusModalOpen] = useState(false);

    // Submission states.
    const [isEditMode, setIsEditMode] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [isActionSubmitting, setIsActionSubmitting] = useState(false);

    // Error states.
    const [submitError, setSubmitError] = useState("");
    const [actionError, setActionError] = useState("");
    const [validationErrors, setValidationErrors] = useState({});

    // Modal form states.
    const [selectedStageId, setSelectedStageId] = useState("");
    const [selectedAssignedUserId, setSelectedAssignedUserId] = useState("");
    const [selectedStatus, setSelectedStatus] = useState("");

    // Opportunity status options.
    const STATUS_OPTIONS = [
        { value: "Open", label: "Open" },
        { value: "Won", label: "Won" },
        { value: "Lost", label: "Lost" }
    ];

    // Normalize status values.
    const normalizeStatus = (status) => {
        return STATUS_OPTIONS.find(
            (option) =>
                option.value.toLowerCase() ===
                String(status ?? "").toLowerCase()
        )?.value ?? "";
    };

    // Opportunity form data.
    const [formData, setFormData] = useState({
        pipelineId: "",
        stageId: "",
        assignedUserId: "",
        name: "",
        value: "",
        expectedCloseDate: "",
        description: ""
    });

    // Fetch opportunities belonging to this customer.
    const {
        opportunities = [],
        loading: isLoadingOpportunities,
        error: opportunitiesError,
        refetch: refetchOpportunities
    } = useOpportunities(customer?.id);

    // Fetch users.
    const { users = [] } = useUsers();

    // Fetch pipelines.
    const {
        pipelines = [],
        loading: isLoadingPipelines
    } = usePipelines();

    // Determine which pipeline's stages should be loaded.
    const stagePipelineId = isStageModalOpen
        ? selectedOpportunity?.pipeline?.id ?? ""
        : formData.pipelineId;

    // Fetch stages for the selected pipeline.
    const {
        stages = [],
        loading: isLoadingStages,
        error: stagesError
    } = useStages(stagePipelineId);

    // Dropdown options.
    const pipelineOptions = pipelines.map((pipeline) => ({
        value: pipeline.id,
        label: pipeline.name
    }));

    const stageOptions = stages.map((stage) => ({
        value: stage.id,
        label: stage.name
    }));

    const userOptions = users.map((user) => ({
        value: user.id,
        label: `${user.firstName} ${user.lastName}`.trim()
    }));

    // Extract a readable API error message.
    const getErrorMessage = (error, fallbackMessage) => {
        const data = error.response?.data;

        const apiValidationErrors = data?.errors
            ? Object.values(data.errors).flat()
            : [];

        return (
            apiValidationErrors[0] ||
            data?.message ||
            data?.title ||
            fallbackMessage
        );
    };

    // Reset the create/edit form.
    const resetForm = () => {
        setFormData({
            pipelineId: "",
            stageId: "",
            assignedUserId: "",
            name: "",
            value: "",
            expectedCloseDate: "",
            description: ""
        });

        setSelectedOpportunity(null);
        setIsEditMode(false);
        setSubmitError("");
        setValidationErrors({});
    };

    // Close the create/edit modal.
    const closeOpportunityModal = () => {
        if (isSubmitting) return;

        setIsOpportunityModalOpen(false);
        resetForm();
    };

    // Close the stage modal.
    const closeStageModal = () => {
        if (isActionSubmitting) return;

        setIsStageModalOpen(false);
        setSelectedOpportunity(null);
        setSelectedStageId("");
        setActionError("");
    };

    // Close the reassignment modal.
    const closeAssignModal = () => {
        if (isActionSubmitting) return;

        setIsAssignModalOpen(false);
        setSelectedOpportunity(null);
        setSelectedAssignedUserId("");
        setActionError("");
    };

    // Close the delete modal.
    const closeDeleteModal = () => {
        if (isActionSubmitting) return;

        setIsDeleteModalOpen(false);
        setSelectedOpportunity(null);
        setActionError("");
    };

    // Close the status modal.
    const closeStatusModal = () => {
        if (isActionSubmitting) return;

        setIsStatusModalOpen(false);
        setSelectedOpportunity(null);
        setSelectedStatus("");
        setActionError("");
    };

    // Open the create opportunity modal.
    const openCreateModal = () => {
        resetForm();
        setIsOpportunityModalOpen(true);
    };

    // Handle standard input fields.
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

    // Change pipeline and reset the selected stage.
    const handlePipelineChange = (pipelineId) => {
        setFormData((prev) => ({
            ...prev,
            pipelineId,
            stageId: ""
        }));

        setValidationErrors((prev) => ({
            ...prev,
            pipelineId: "",
            stageId: ""
        }));

        setSubmitError("");
    };

    // Handle stage selection in the create/edit form.
    const handleStageChange = (stageId) => {
        setFormData((prev) => ({
            ...prev,
            stageId
        }));

        setValidationErrors((prev) => ({
            ...prev,
            stageId: ""
        }));

        setSubmitError("");
    };

    // Create or update an opportunity.
    const handleSubmit = async (e) => {
        e.preventDefault();

        // Validate the form using the separate validator.
        const errors = CustomerOpportunityValidator(formData, customer?.id);

        setValidationErrors(errors);
        setSubmitError("");

        // Stop submission when validation fails.
        if (Object.keys(errors).length > 0) {
            return;
        }

        setIsSubmitting(true);

        try {
            const opportunityData = {
                customerId: customer.id,
                pipelineId: formData.pipelineId,
                stageId: formData.stageId,
                assignedUserId: formData.assignedUserId || null,
                name: formData.name.trim(),
                value: Number(formData.value),
                expectedCloseDate: formData.expectedCloseDate || null,
                description: formData.description.trim() || null
            };

            if (isEditMode && selectedOpportunity?.id) {
                await updateOpportunity(
                    selectedOpportunity.id,
                    opportunityData
                );
            } else {
                await createOpportunity(opportunityData);
            }

            await refetchOpportunities();

            setIsOpportunityModalOpen(false);
            resetForm();
        } catch (error) {
            console.error("Failed to save opportunity:", error);
            console.error("API response:", error.response?.data);

            setSubmitError(
                getErrorMessage(
                    error,
                    isEditMode
                        ? "Failed to update opportunity."
                        : "Failed to create opportunity."
                )
            );
        } finally {
            setIsSubmitting(false);
        }
    };

    // Open an existing opportunity in edit mode.
    const openEditModal = (opportunity) => {
        resetForm();

        setSelectedOpportunity(opportunity);
        setIsEditMode(true);

        setFormData({
            pipelineId: opportunity.pipeline?.id ?? "",
            stageId: opportunity.stage?.id ?? "",
            assignedUserId: opportunity.assignedUser?.id ?? "",
            name: opportunity.name ?? "",
            value: opportunity.value != null
                ? String(opportunity.value)
                : "",
            expectedCloseDate: opportunity.expectedCloseDate
                ? String(opportunity.expectedCloseDate).slice(0, 10)
                : "",
            description: opportunity.description ?? ""
        });

        setSubmitError("");
        setValidationErrors({});
        setIsOpportunityModalOpen(true);
    };

    // Open the stage selection modal.
    const openStageModal = (opportunity) => {
        setSelectedOpportunity(opportunity);
        setSelectedStageId(opportunity.stage?.id ?? "");
        setActionError("");
        setIsStageModalOpen(true);
    };

    // Save the selected stage.
    const handleStageSubmit = async (e) => {
        e.preventDefault();

        if (!selectedOpportunity?.id || !selectedStageId) {
            setActionError("Please select a stage.");
            return;
        }

        if (selectedStageId === selectedOpportunity.stage?.id) {
            setActionError("Please select a different stage.");
            return;
        }

        setIsActionSubmitting(true);
        setActionError("");

        try {
            await updateStageOpportunity(
                selectedOpportunity.id,
                selectedStageId
            );

            await refetchOpportunities();

            setIsStageModalOpen(false);
            setSelectedOpportunity(null);
            setSelectedStageId("");
        } catch (error) {
            console.error("Failed to update opportunity stage:", error);

            setActionError(
                getErrorMessage(
                    error,
                    "Failed to update opportunity stage."
                )
            );
        } finally {
            setIsActionSubmitting(false);
        }
    };

    // Open the reassignment modal.
    const openAssignModal = (opportunity) => {
        setSelectedOpportunity(opportunity);
        setSelectedAssignedUserId(opportunity.assignedUser?.id ?? "");
        setActionError("");
        setIsAssignModalOpen(true);
    };

    // Reassign the opportunity.
    const handleAssignSubmit = async (e) => {
        e.preventDefault();

        if (!selectedOpportunity?.id || !selectedAssignedUserId) {
            setActionError("Please select a sales representative.");
            return;
        }

        if (
            selectedAssignedUserId ===
            selectedOpportunity.assignedUser?.id
        ) {
            setActionError(
                "Please select a different sales representative."
            );
            return;
        }

        setIsActionSubmitting(true);
        setActionError("");

        try {
            await reassigneduserOpportunity(
                selectedOpportunity.id,
                selectedAssignedUserId
            );

            await refetchOpportunities();

            setIsAssignModalOpen(false);
            setSelectedOpportunity(null);
            setSelectedAssignedUserId("");
        } catch (error) {
            console.error("Failed to reassign opportunity:", error);

            setActionError(
                getErrorMessage(
                    error,
                    "Failed to reassign opportunity."
                )
            );
        } finally {
            setIsActionSubmitting(false);
        }
    };

    // Open the status confirmation modal.
    const openStatusModal = (opportunity) => {
        setSelectedOpportunity(opportunity);
        setSelectedStatus(normalizeStatus(opportunity.status));
        setActionError("");
        setIsStatusModalOpen(true);
    };

    // Save the selected status.
    const handleStatusSubmit = async (e) => {
        e.preventDefault();

        if (!selectedOpportunity?.id || !selectedStatus) {
            setActionError("Please select a status.");
            return;
        }

        if (
            selectedStatus === normalizeStatus(selectedOpportunity.status)
        ) {
            setActionError("Please select a different status.");
            return;
        }

        setIsActionSubmitting(true);
        setActionError("");

        try {
            await updateStatusOpportunity(
                selectedOpportunity.id,
                selectedStatus
            );

            await refetchOpportunities();

            setIsStatusModalOpen(false);
            setSelectedOpportunity(null);
            setSelectedStatus("");
        } catch (error) {
            console.error("Failed to update opportunity status:", error);

            setActionError(
                getErrorMessage(
                    error,
                    "Failed to update opportunity status."
                )
            );
        } finally {
            setIsActionSubmitting(false);
        }
    };

    // Open the delete confirmation modal.
    const openDeleteModal = (opportunity) => {
        setSelectedOpportunity(opportunity);
        setActionError("");
        setIsDeleteModalOpen(true);
    };

    // Delete the selected opportunity.
    const handleDeleteSubmit = async () => {
        if (!selectedOpportunity?.id) return;

        setIsActionSubmitting(true);
        setActionError("");

        try {
            await deleteOpportunity(selectedOpportunity.id);

            await refetchOpportunities();

            setIsDeleteModalOpen(false);
            setSelectedOpportunity(null);
        } catch (error) {
            console.error("Failed to delete opportunity:", error);

            setActionError(
                getErrorMessage(
                    error,
                    "Failed to delete opportunity."
                )
            );
        } finally {
            setIsActionSubmitting(false);
        }
    };

    // Return the appropriate stage badge style.
    const getStageStyle = (stage) => {
        if (stage?.isWon) {
            return "bg-emerald-100 text-emerald-700 ring-1 ring-emerald-200";
        }

        if (stage?.isLost) {
            return "bg-red-100 text-red-700 ring-1 ring-red-200";
        }

        switch (stage?.name?.toLowerCase()) {
            case "new lead":
                return "bg-slate-100 text-slate-700 ring-1 ring-slate-200";
            case "contacted":
                return "bg-sky-100 text-sky-700 ring-1 ring-sky-200";
            case "qualified":
                return "bg-violet-100 text-violet-700 ring-1 ring-violet-200";
            case "proposal sent":
                return "bg-amber-100 text-amber-700 ring-1 ring-amber-200";
            default:
                return "bg-gray-100 text-gray-700 ring-1 ring-gray-200";
        }
    };

    // Format opportunity value as Philippine pesos.
    const formatCurrency = (value) => {
        if (value == null || !Number.isFinite(Number(value))) {
            return "—";
        }

        return `₱${Number(value).toLocaleString("en-PH", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        })}`;
    };

    // Format the expected close date.
    const formatDate = (date) => {
        if (!date) return "Not set";

        const dateValue = String(date).slice(0, 10);
        const parsedDate = new Date(`${dateValue}T00:00:00`);

        if (Number.isNaN(parsedDate.getTime())) {
            return "Not set";
        }

        return parsedDate.toLocaleDateString("en-PH", {
            year: "numeric",
            month: "short",
            day: "numeric"
        });
    };

    const sortedOpportunities = [...opportunities].sort(
        (a, b) =>
            new Date(a.createdAt).getTime() -
            new Date(b.createdAt).getTime()
    );

    // Shared input styling.
    const inputClass =
        "h-10 w-full min-w-0 rounded-md border border-border bg-slate-100 p-2 text-text placeholder:text-text-muted focus:border-primary focus:outline-none focus:ring-2 focus:ring-primary";

    // Calculate total number of opportunities.
    const totalOpportunities = opportunities.length;

    // Calculate the total potential value of all opportunities.
    const totalPotentialValue = opportunities.reduce(
        (total, opportunity) => {
            const value = Number(opportunity.value);

            return total + (Number.isFinite(value) ? value : 0);
        },
        0
    );

    return (
        <div className="flex min-w-0 flex-col gap-4">
            {/* Header */}
            <div className="flex flex-col gap-4">
                {/* First layer: Title and Add Button */}
                <div className="mb-2 flex items-center justify-between gap-3">
                    <div className="flex min-w-0 flex-col gap-1">
                        <h3 className="text-lg font-semibold text-text">
                            Opportunities
                        </h3>

                        <span className="text-sm text-text-secondary">
                            Manage potential sales for this customer.
                        </span>
                    </div>

                    <Button
                        type="button"
                        onClick={openCreateModal}
                    >
                        +
                    </Button>
                </div>

                {/* Second layer: Opportunity Summary */}
                <div className="grid grid-cols-1 gap-3 sm:grid-cols-2">

                    {/* Total Opportunities */}
                    <div className="flex flex-col gap-2 rounded-lg border border-border bg-surface p-4">
                        <div className="flex items-center gap-2">
                            <span className="flex h-9 w-9 items-center justify-center rounded-lg bg-blue-100 text-blue-700">
                                <BriefcaseBusiness size={20} />
                            </span>

                            <span className="text-sm text-text-secondary">
                                Total Opportunities
                            </span>
                        </div>

                        <h3 className="text-2xl font-semibold text-text">
                            {isLoadingOpportunities ? "..." : totalOpportunities}
                        </h3>

                        <p className="text-xs text-text-secondary">
                            Opportunities linked to this customer
                        </p>
                    </div>

                    {/* Total Potential Value */}
                    <div className="flex flex-col gap-2 rounded-lg border border-border bg-surface p-4">
                        <div className="flex items-center gap-2">
                            <span className="flex h-9 w-9 items-center justify-center rounded-lg bg-emerald-100 text-emerald-700">
                                <Wallet size={20} />
                            </span>

                            <span className="text-sm text-text-secondary">
                                Total Potential Value
                            </span>
                        </div>

                        <h3 className="text-2xl font-semibold text-text">
                            {isLoadingOpportunities
                                ? "..."
                                : formatCurrency(totalPotentialValue)}
                        </h3>

                        <p className="text-xs text-text-secondary">
                            Combined value of all opportunities
                        </p>
                    </div>

                </div>
            </div>

            {/* Opportunity list */}
            {isLoadingOpportunities ? (
                <p className="py-6 text-center text-text-secondary">
                    Loading opportunities...
                </p>
            ) : opportunitiesError ? (
                <div className="py-6 text-center">
                    <p className="mb-3 text-red-500">
                        {opportunitiesError}
                    </p>

                    <Button
                        type="button"
                        variant="primary"
                        onClick={refetchOpportunities}
                    >
                        Retry
                    </Button>
                </div>
            ) : opportunities.length === 0 ? (

                <div className="border border-border rounded-lg">
                    <p className="py-6 text-center text-text-secondary">
                    No opportunities available for this customer.
                    </p>
                </div>

            ) : (
                <div className="flex min-w-0 flex-col gap-3">
                    {sortedOpportunities.map((opportunity) => {
                        const actions = [
                            {
                                label: "Edit",
                                onClick: () => openEditModal(opportunity)
                            },
                            {
                                label: "Stage",
                                onClick: () => openStageModal(opportunity)
                            },
                            {
                                label: "Reassign",
                                onClick: () => openAssignModal(opportunity)
                            },
                            {
                                label: "Status",
                                onClick: () => openStatusModal(opportunity)
                            },
                            {
                                label: "Delete",
                                variant: "danger",
                                onClick: () => openDeleteModal(opportunity)
                            }
                        ];

                        return (
                            <div
                                key={opportunity.id}
                                className="flex min-w-0 flex-col gap-3 rounded-lg border border-border p-4 transition-colors hover:bg-surface"
                            >
                                <div className="flex min-w-0 flex-col gap-2">
                                    <div className="flex min-w-0 items-start justify-between gap-3">
                                        <h4 className="min-w-0 wrap-break-word font-semibold text-text">
                                            {opportunity.name}
                                        </h4>

                                        <TableActions options={actions} />
                                    </div>

                                    <p className="wrap-break-word text-sm text-text-secondary">
                                        {opportunity.pipeline?.name ?? "No pipeline"}
                                    </p>

                                    <span className="font-medium text-text">
                                        {formatCurrency(opportunity.value)}
                                    </span>
                                </div>

                                <div className="flex flex-wrap items-center gap-2">
                                    <span
                                        className={`inline-flex w-fit items-center whitespace-nowrap rounded-full px-3 py-1 text-xs font-medium ${getStageStyle(
                                            opportunity.stage
                                        )}`}
                                    >
                                        {opportunity.stage?.name ?? "No stage"}
                                    </span>

                                    {opportunity.status && (
                                        <span className="inline-flex w-fit items-center whitespace-nowrap rounded-full bg-slate-100 px-3 py-1 text-xs font-medium text-slate-700 ring-1 ring-slate-200">
                                            {normalizeStatus(opportunity.status) ||
                                                opportunity.status}
                                        </span>
                                    )}

                                    <span className="text-xs text-text-secondary">
                                        Expected Close Date:{" "}
                                        {formatDate(opportunity.expectedCloseDate)}
                                    </span>
                                </div>

                                {opportunity.description && (
                                    <p className="wrap-break-word border-t border-border pt-3 text-sm text-text-secondary">
                                        {opportunity.description}
                                    </p>
                                )}
                            </div>
                        );
                    })}
                </div>
            )}

            {/* Create / Edit Opportunity Modal */}
            <Modal
                isOpen={isOpportunityModalOpen}
                onClose={closeOpportunityModal}
                className="w-full max-w-2xl"
                title={`${isEditMode ? "Edit" : "Add"} Opportunity - ${customer?.firstName ?? ""} ${customer?.lastName ?? ""}`}
            >
                <form
                    className="flex max-h-[75vh] flex-col gap-4 overflow-y-auto"
                    onSubmit={handleSubmit}
                >
                    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                        {/* Opportunity Name */}
                        <div className="flex min-w-0 flex-col">
                            <label className="mb-2 text-sm font-medium text-text">
                                Opportunity Name
                            </label>

                            <input
                                type="text"
                                name="name"
                                value={formData.name}
                                onChange={handleChange}
                                placeholder="e.g. Website Redesign"
                                maxLength={200}
                                className={inputClass}
                            />

                            {validationErrors.name && (
                                <p className="mt-1 text-xs text-red-500">
                                    {validationErrors.name}
                                </p>
                            )}
                        </div>

                        {/* Assigned User */}
                        <div className="flex min-w-0 flex-col">
                            <label className="mb-2 text-sm font-medium text-text">
                                Assigned To
                            </label>

                            <Dropdown
                                label="Select Sales Representative"
                                value={formData.assignedUserId}
                                onChange={(value) =>
                                    setFormData((prev) => ({
                                        ...prev,
                                        assignedUserId: value
                                    }))
                                }
                                options={userOptions}
                                className="w-full"
                            />
                        </div>
                    </div>

                    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                        {/* Pipeline */}
                        <div className="flex min-w-0 flex-col">
                            <label className="mb-2 text-sm font-medium text-text">
                                Pipeline
                            </label>

                            <Dropdown
                                label={
                                    isLoadingPipelines
                                        ? "Loading pipelines..."
                                        : "Select pipeline"
                                }
                                value={formData.pipelineId}
                                onChange={handlePipelineChange}
                                options={pipelineOptions}
                                className="w-full"
                            />

                            {validationErrors.pipelineId && (
                                <p className="mt-1 text-xs text-red-500">
                                    {validationErrors.pipelineId}
                                </p>
                            )}
                        </div>

                        {/* Stage */}
                        <div className="flex min-w-0 flex-col">
                            <label className="mb-2 text-sm font-medium text-text">
                                Stage
                            </label>

                            <Dropdown
                                label={
                                    !formData.pipelineId
                                        ? "Select a pipeline first"
                                        : isLoadingStages
                                            ? "Loading stages..."
                                            : stagesError
                                                ? "Failed to load stages"
                                                : stageOptions.length === 0
                                                    ? "No stages available"
                                                    : "Select stage"
                                }
                                value={formData.stageId}
                                onChange={handleStageChange}
                                options={formData.pipelineId ? stageOptions : []}
                                disabled={
                                    !formData.pipelineId ||
                                    isLoadingStages ||
                                    !!stagesError ||
                                    stageOptions.length === 0
                                }
                                className="w-full"
                            />

                            {stagesError && (
                                <p className="mt-1 text-xs text-red-500">
                                    {stagesError}
                                </p>
                            )}

                            {validationErrors.stageId && (
                                <p className="mt-1 text-xs text-red-500">
                                    {validationErrors.stageId}
                                </p>
                            )}
                        </div>
                    </div>

                    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                        {/* Opportunity Value */}
                        <div className="flex min-w-0 flex-col">
                            <label className="mb-2 text-sm font-medium text-text">
                                Value (₱)
                            </label>

                            <input
                                type="number"
                                name="value"
                                min="0"
                                step="0.01"
                                value={formData.value}
                                onChange={handleChange}
                                placeholder="0.00"
                                className={inputClass}
                            />

                            {validationErrors.value && (
                                <p className="mt-1 text-xs text-red-500">
                                    {validationErrors.value}
                                </p>
                            )}
                        </div>

                        {/* Expected Close Date */}
                        <div className="flex min-w-0 flex-col">
                            <label className="mb-2 text-sm font-medium text-text">
                                Expected Close Date
                            </label>

                            <input
                                type="date"
                                name="expectedCloseDate"
                                value={formData.expectedCloseDate}
                                onChange={handleChange}
                                className={inputClass}
                            />
                        </div>
                    </div>

                    {/* Description */}
                    <div className="flex flex-col">
                        <label className="mb-2 text-sm font-medium text-text">
                            Description
                        </label>

                        <textarea
                            name="description"
                            value={formData.description}
                            onChange={handleChange}
                            placeholder="Describe the opportunity, customer needs, or proposed solution..."
                            rows={3}
                            className="w-full resize-y rounded-md border border-border bg-slate-100 px-3 py-2 text-text placeholder:text-text-muted focus:border-primary focus:outline-none focus:ring-2 focus:ring-primary"
                        />
                    </div>

                    {/* Customer validation error */}
                    {validationErrors.customerId && (
                        <p className="text-sm text-red-500">
                            {validationErrors.customerId}
                        </p>
                    )}

                    {/* API error */}
                    {submitError && (
                        <p className="text-sm text-red-500">
                            {submitError}
                        </p>
                    )}

                    {/* Form actions */}
                    <div className="flex justify-end gap-2 border-t border-border pt-4">
                        <Button
                            type="button"
                            variant="secondary"
                            onClick={closeOpportunityModal}
                            disabled={isSubmitting}
                        >
                            Cancel
                        </Button>

                        <Button
                            type="submit"
                            variant="primary"
                            disabled={
                                isSubmitting ||
                                isLoadingPipelines ||
                                (Boolean(formData.pipelineId) && isLoadingStages)
                            }
                        >
                            {isSubmitting
                                ? "Saving..."
                                : isEditMode
                                    ? "Update Opportunity"
                                    : "Create Opportunity"}
                        </Button>
                    </div>
                </form>
            </Modal>

            {/* Change Stage Modal */}
            <Modal
                isOpen={isStageModalOpen}
                onClose={closeStageModal}
                title="Change Opportunity Stage"
                className="w-full max-w-md"
            >
                <form
                    className="flex max-h-[75vh] flex-col gap-4 overflow-y-auto"
                    onSubmit={handleStageSubmit}
                >
                    <div className="flex flex-col gap-1">
                        <label className="text-sm font-medium text-text">
                            Opportunity
                        </label>

                        <p className="wrap-break-word text-text-secondary">
                            {selectedOpportunity?.name}
                        </p>
                    </div>

                    <div className="flex flex-col gap-1">
                        <label className="text-sm font-medium text-text">
                            Pipeline
                        </label>

                        <p className="wrap-break-word text-text-secondary">
                            {selectedOpportunity?.pipeline?.name ?? "No pipeline"}
                        </p>
                    </div>

                    <div className="flex flex-col gap-2">
                        <label className="text-sm font-medium text-text">
                            New Stage
                        </label>

                        <Dropdown
                            label={
                                !selectedOpportunity?.pipeline?.id
                                    ? "No pipeline selected"
                                    : isLoadingStages
                                        ? "Loading stages..."
                                        : stagesError
                                            ? "Failed to load stages"
                                            : stageOptions.length === 0
                                                ? "No stages available"
                                                : "Select stage"
                            }
                            value={selectedStageId}
                            onChange={(value) => {
                                setSelectedStageId(value);
                                setActionError("");
                            }}
                            options={stageOptions}
                            className="w-full"
                        />
                    </div>

                    {stagesError && (
                        <p className="text-sm text-red-500">
                            {stagesError}
                        </p>
                    )}

                    {actionError && (
                        <p className="text-sm text-red-500">
                            {actionError}
                        </p>
                    )}

                    <div className="flex justify-end gap-2 border-t border-border pt-4">
                        <Button
                            type="button"
                            variant="secondary"
                            onClick={closeStageModal}
                            disabled={isActionSubmitting}
                        >
                            Cancel
                        </Button>

                        <Button
                            type="submit"
                            disabled={
                                isActionSubmitting ||
                                isLoadingStages ||
                                !selectedStageId ||
                                selectedStageId === selectedOpportunity?.stage?.id
                            }
                        >
                            {isActionSubmitting ? "Saving..." : "Update Stage"}
                        </Button>
                    </div>
                </form>
            </Modal>

            {/* Reassign Opportunity Modal */}
            <Modal
                isOpen={isAssignModalOpen}
                onClose={closeAssignModal}
                title="Reassign Opportunity"
                className="w-full max-w-md"
            >
                <form
                    className="flex max-h-[75vh] flex-col gap-4 overflow-y-auto"
                    onSubmit={handleAssignSubmit}
                >
                    <div className="flex flex-col gap-1">
                        <label className="text-sm font-medium text-text">
                            Opportunity
                        </label>

                        <p className="wrap-break-word text-text-secondary">
                            {selectedOpportunity?.name}
                        </p>
                    </div>

                    <div className="flex flex-col gap-2">
                        <label className="text-sm font-medium text-text">
                            Assign To
                        </label>

                        <Dropdown
                            label="Select Sales Representative"
                            value={selectedAssignedUserId}
                            onChange={(value) => {
                                setSelectedAssignedUserId(value);
                                setActionError("");
                            }}
                            options={userOptions}
                            className="w-full"
                        />
                    </div>

                    {actionError && (
                        <p className="text-sm text-red-500">
                            {actionError}
                        </p>
                    )}

                    <div className="flex justify-end gap-2 border-t border-border pt-4">
                        <Button
                            type="button"
                            variant="secondary"
                            onClick={closeAssignModal}
                            disabled={isActionSubmitting}
                        >
                            Cancel
                        </Button>

                        <Button
                            type="submit"
                            disabled={
                                isActionSubmitting ||
                                !selectedAssignedUserId ||
                                selectedAssignedUserId === selectedOpportunity?.assignedUser?.id
                            }
                        >
                            {isActionSubmitting ? "Saving..." : "Reassign"}
                        </Button>
                    </div>
                </form>
            </Modal>

            {/* Change Status Modal */}
            <Modal
                isOpen={isStatusModalOpen}
                onClose={closeStatusModal}
                title="Change Opportunity Status"
                className="w-full max-w-md"
            >
                <form
                    className="flex max-h-[75vh] flex-col gap-4 overflow-y-auto"
                    onSubmit={handleStatusSubmit}
                >
                    <div className="flex flex-col gap-1">
                        <label className="text-sm font-medium text-text">
                            Opportunity
                        </label>

                        <p className="wrap-break-word text-text-secondary">
                            {selectedOpportunity?.name}
                        </p>
                    </div>

                    <div className="flex flex-col gap-2">
                        <label className="text-sm font-medium text-text">
                            New Status
                        </label>

                        <Dropdown
                            label="Select status"
                            value={selectedStatus}
                            onChange={(value) => {
                                setSelectedStatus(value);
                                setActionError("");
                            }}
                            options={STATUS_OPTIONS}
                            className="w-full"
                        />
                    </div>

                    {actionError && (
                        <p className="text-sm text-red-500">
                            {actionError}
                        </p>
                    )}

                    <div className="flex justify-end gap-2 border-t border-border pt-4">
                        <Button
                            type="button"
                            variant="secondary"
                            onClick={closeStatusModal}
                            disabled={isActionSubmitting}
                        >
                            Cancel
                        </Button>

                        <Button
                            type="submit"
                            disabled={
                                isActionSubmitting ||
                                !selectedStatus ||
                                selectedStatus === normalizeStatus(selectedOpportunity?.status)
                            }
                        >
                            {isActionSubmitting ? "Saving..." : "Update Status"}
                        </Button>
                    </div>
                </form>
            </Modal>

            {/* Delete Opportunity Modal */}
            <Modal
                isOpen={isDeleteModalOpen}
                onClose={closeDeleteModal}
                title="Delete Opportunity"
                className="w-full max-w-md"
            >
                <div className="flex flex-col gap-4">
                    <p className="text-sm text-text-secondary">
                        Are you sure you want to delete{" "}
                        <span className="wrap-break-word font-semibold text-text">
                            {selectedOpportunity?.name}
                        </span>
                        ? This action cannot be undone.
                    </p>

                    {actionError && (
                        <p className="text-sm text-red-500">
                            {actionError}
                        </p>
                    )}

                    <div className="flex justify-end gap-2 border-t border-border pt-4">
                        <Button
                            type="button"
                            variant="secondary"
                            onClick={closeDeleteModal}
                            disabled={isActionSubmitting}
                        >
                            Cancel
                        </Button>

                        <Button
                            type="button"
                            variant="danger"
                            onClick={handleDeleteSubmit}
                            disabled={isActionSubmitting}
                        >
                            {isActionSubmitting ? "Deleting..." : "Delete"}
                        </Button>
                    </div>
                </div>
            </Modal>
        </div>
    );
};
