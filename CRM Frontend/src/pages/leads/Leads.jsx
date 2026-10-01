import { useState } from "react";
import Button from "../../components/common/Button"
import { Dropdown } from "../../components/common/Dropdown";
import { LeadColumns } from "./LeadColumns";
import { DataTable } from "../../components/common/DataTable";
import { Modal } from "../../components/common/Modal";
import { useLeads } from "../../hooks/useLeads";
import { createLead, updateLead, deleteLead } from "../../services/core/leadsService";
import { useUsers } from "../../hooks/useUsers";
import { LeadValidator } from "./LeadValidator";

export const Leads = () => {

    const {leads, leadStatuses, leadSources, pagination, setPagination,filters, setFilters, loading, error, refetch} = useLeads();

    const {users} = useUsers();

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [validationErrors, setValidationErrors] = useState({});
    const [submitError, setSubmitError] = useState("");

    const [selectedLead, setSelectedLead] = useState(null);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [isNotesModalOpen, setIsNotesModalOpen] = useState(false);
    const [isEditMode, setIsEditMode] = useState(false);
    const [deleteError, setDeleteError] = useState("");

    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        email: "",
        phone: "",
        companyName: "",
        estimatedValue: "",
        statusId: "",
        sourceId: "",
        assignedUserId: "",
        notes: ""
    })

    const resetForm = () => {
        setFormData({
            firstName: "",
            lastName: "",
            email: "",
            phone: "",
            companyName: "",
            estimatedValue: "",
            statusId: "",
            sourceId: "",
            assignedUserId: "",
            notes: ""
        });

        setValidationErrors({});
    };


    const handleChange = (e) => {
        const { name, value} = e.target;

        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));

    };

    const handleSubmit = async (e) =>{
        e.preventDefault();

        setSubmitError("");

        const errors = LeadValidator(formData);

        setValidationErrors(errors);

        if (Object.keys(errors).length > 0) {
            return;
        }

        try {
            const leadData = {
                assignedUserId: formData.assignedUserId,
                sourceId: formData.sourceId,
                statusId: formData.statusId,
                firstName: formData.firstName,
                lastName: formData.lastName,
                companyName: formData.companyName,
                email: formData.email,
                phone: formData.phone,
                estimatedValue: Number(formData.estimatedValue),
                notes: formData.notes
            };

            if (isEditMode) {
                await updateLead(selectedLead.id, leadData);
            } else {
                await createLead(leadData);
            }
            await refetch();

            setIsModalOpen(false);
            setIsEditMode(false);
            setSelectedLead(null);
            resetForm();
        }catch (error) {
            setSubmitError(
            isEditMode
                ? "Failed to update lead."
                : "Failed to create lead."
            );
        }
    }

    const handleEdit = (lead) => {

    setSelectedLead(lead);
    setIsEditMode(true);

    setFormData({
        firstName: lead.firstName ?? "",
        lastName: lead.lastName ?? "",
        email: lead.email ?? "",
        phone: lead.phone ?? "",
        companyName: lead.companyName ?? "",
        estimatedValue: lead.estimatedValue ?? "",
        statusId: lead.status?.id ?? "",
        sourceId: lead.source?.id ?? "",
        assignedUserId: lead.assignedUser?.id ?? "",
        notes: lead.notes ?? ""
    });

        setValidationErrors({});
        setSubmitError("");
        setIsModalOpen(true);
    };

    const handleDelete = async () => {
        setDeleteError("");

        try {
            await deleteLead(selectedLead.id);

            await refetch();
            setIsDeleteModalOpen(false);
            setSelectedLead(null);
        } catch (error) {
            setDeleteError("Failed to delete lead.");
        }
    };

    const closeLeadModal = () => {
        setIsModalOpen(false);
        setIsEditMode(false);
        setSelectedLead(null);
        setSubmitError("");
        setValidationErrors({});
        resetForm();
    };


    const columns = LeadColumns({
        onView: (lead) => {
            setSelectedLead(lead);
            setIsNotesModalOpen(true);
        },
        onEdit: handleEdit,
        onDelete: (lead) => {
            setSelectedLead(lead);
            setDeleteError("");
            setIsDeleteModalOpen(true);
        }
    });

    const inputClass = "h-10 p-2 w-xs border border-border rounded-md text-text placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary";

    return (
        <div className="min-h-screen bg-background flex p-6">

            <main className="bg-surface border border-border w-full rounded-xl shadow-md p-8">
                <div className="flex justify-between mb-5">
                    <p className="text-2xl font-bold text-text">
                    Leads
                    </p>

                    <Button onClick={() => {
                        resetForm();
                        setSubmitError("");
                        setIsEditMode(false);
                        setSelectedLead(null);
                        setIsModalOpen(true);
                    }}>
                        + Add Lead
                    </Button>
                    
                </div>

                <div className="flex justify-end items-center gap-3 mb-5">
                    <input type="text"
                    placeholder="Search leads..."
                    className="border border-border rounded-3xl p-4 h-10 w-xs placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary" 
                    value={filters.search}
                    onChange={(e) => {
                        setFilters(prev => ({
                            ...prev,
                            search: e.target.value
                        }));

                        setPagination(prev => ({
                            ...prev,
                            page: 1
                        }));
                    }}
                    />

                    <div className="flex gap-3">

                        <Dropdown label="Status"    
                            className="w-40" 
                            value={filters.statusId} 
                            onChange={(value) => {
                                setFilters(prev => ({
                                    ...prev,
                                    statusId: value
                                }));

                                setPagination(prev =>({
                                    ...prev,
                                    page: 1
                                }))
                            }}
                            options={[
                                {
                                    value: "",
                                    label: "All Statuses"
                                },
                                ...leadStatuses.map((status) => ({
                                    value: status.id,
                                    label: status.name
                                }))
                            ]} 
                        />

                        <Dropdown label="Source" 
                        className="w-40" 
                        value={filters.sourceId}
                        onChange={(value) => {
                            setFilters(prev =>({
                                ...prev,
                                sourceId: value
                            }));
                            setPagination(prev =>({
                                ...prev,
                                page: 1
                            }))
                        }}
                        options={[
                            {
                                value: "",
                                label: "All Sources"
                            },
                            ...leadSources.map((source) => ({
                                value: source.id,
                                label: source.name
                            }))
                        ]}
                        />

                        <Dropdown label="Assigned" 
                        className="w-40" 
                        value={filters.assignedUserId}
                        onChange= {(value) => {
                            setFilters(prev => ({
                                ...prev,
                                assignedUserId: value
                            }));
                            setPagination(prev =>({
                                ...prev,
                                page: 1
                            }))
                        }}
                        options={[
                            {
                                value: "",
                                label: "All Assigned"
                            },
                            ...users.map((user) => ({
                                value: user.id,
                                label: `${user.firstName} ${user.lastName}`
                            }))
                        ]}
                        />

                    </div>
                </div>

                {error && (
                    <div className="mb-4 p-3 rounded-md bg-red-50 border border-red-200 text-red-600">
                        {error}
                    </div>
                )}


                {loading ? (
                    <div className="py-10 text-center text-text-secondary">
                        Loading leads...
                    </div>
                ) : (
                    <DataTable
                        data={leads}
                        columns={columns}
                    />
                )}

                <div className="flex items-center justify-between mt-4">

                    <div className="text-sm text-text-secondary">
                        Page {pagination.page} of {pagination.totalPages}
                    </div>

                    <div className="flex gap-2">

                        <Button
                            variant="outline"
                            disabled={!pagination.hasPreviousPage}
                            onClick={() => {
                                setPagination(prev => ({
                                    ...prev,
                                    page: prev.page - 1
                                }));
                            }}
                        >
                            Previous
                        </Button>

                        <Button
                            variant="outline"
                            disabled={!pagination.hasNextPage}
                            onClick={() => {
                                setPagination(prev => ({
                                    ...prev,
                                    page: prev.page + 1
                                }));
                            }}
                        >
                            Next
                        </Button>

                    </div>

                </div>

                {/* MODAL */}
                 <Modal
                    isOpen={isModalOpen}
                    onClose={() => {closeLeadModal()}}
                    title={isEditMode ? "Edit Lead" : "Add Lead"}>
                    <form onSubmit={handleSubmit} className="flex flex-col ">
                        
                        <div className="flex justify-between gap-3 mb-2">
                            <div className="flex flex-col">
                                
                                <label className="mb-2 text-base font-medium text-text">First Name</label>
                                
                                <input type="text"
                                name="firstName" 
                                placeholder="First Name" 
                                value={formData.firstName}
                                onChange={handleChange}
                                className={inputClass} />
                                
                                {validationErrors.firstName && (
                                    <span className="mt-1 text-sm text-red-500">
                                        {validationErrors.firstName}
                                    </span>
                                )}

                            </div>

                            <div className="flex flex-col">
                                
                                <label className="mb-2 text-base font-medium text-text">Last Name</label>

                                <input type="text" 
                                name="lastName"
                                value={formData.lastName}
                                onChange={handleChange}
                                placeholder="Last Name" 
                                className={inputClass} />

                                {validationErrors.lastName && (
                                    <span className="mt-1 text-sm text-red-500">
                                        {validationErrors.lastName}
                                    </span>
                                )}
                                
                            </div>
                            
                        </div>

                        <div className="flex justify-between mb-2">
                            
                            <div className="flex flex-col">
                                
                                <label className="mb-2 text-base font-medium text-text">Email</label>
                                
                                <input type="email"
                                name="email"
                                value={formData.email}
                                onChange={handleChange}
                                placeholder="john@example.com" 
                                className={inputClass} />

                                {validationErrors.email && (
                                    <span className="mt-1 text-sm text-red-500">
                                        {validationErrors.email}
                                    </span>
                                )}

                            </div>

                            <div className="flex flex-col">
                                
                                <label className="mb-2 text-base font-medium text-text">Phone</label>

                                <input type="text"
                                name="phone"
                                value={formData.phone}
                                onChange={handleChange} 
                                placeholder="09XX-XXXX-XXX" 
                                className={inputClass}/>

                                {validationErrors.phone && (
                                    <span className="mt-1 text-sm text-red-500">
                                        {validationErrors.phone}
                                    </span>
                                )}
                                
                            </div>
                        </div>

                        <div className="flex justify-between mb-2">

                            <div className="flex flex-col">

                                <label className="mb-2 text-base font-medium text-text">Company Name</label>
                                
                                <input type="text"
                                name="companyName"
                                value={formData.companyName}
                                onChange={handleChange}
                                placeholder="Company" 
                                className={inputClass} />

                                {validationErrors.companyName && (
                                    <span className="mt-1 text-sm text-red-500">
                                        {validationErrors.companyName}
                                    </span>
                                )}

                            </div>

                            <div className="flex flex-col">

                                <label className="mb-2 text-base font-medium text-text">Estimated Value</label>
                                
                                <input type="number"
                                name="estimatedValue"
                                min="0"
                                value={formData.estimatedValue}
                                onChange={handleChange}
                                placeholder="₱" 
                                className={inputClass} />

                                {validationErrors.estimatedValue && (
                                    <span className="mt-1 text-sm text-red-500">
                                        {validationErrors.estimatedValue}
                                    </span>
                                )}

                            </div>
                            
                        </div>

                        <div className="flex justify-between mb-2">

                            <div className="flex flex-col">

                                <label className="mb-2 text-base font-medium text-text">Status</label>
                                
                                <Dropdown label="Status" className="w-50" 
                                value={formData.statusId}
                                onChange={(value) =>
                                    setFormData((prev) =>({
                                        ...prev,
                                        statusId:value
                                    }))
                                }
                                 options={leadStatuses.map((status) => ({
                                    value: status.id,
                                    label: status.name
                                }))}   
                                />

                                {validationErrors.statusId && (
                                    <span className="mt-1 text-sm text-red-500">
                                        {validationErrors.statusId}
                                    </span>
                                )}

                            </div>

                            <div className="flex flex-col">

                                <label className="mb-2 text-base font-medium text-text">Source</label>
                                
                                <Dropdown label="Source" className="w-50" value={formData.sourceId}
                                onChange={(value) =>
                                    setFormData((prev) =>({
                                        ...prev,
                                        sourceId:value
                                    }))
                                }
                                 options={leadSources.map((source) => ({
                                    value: source.id,
                                    label: source.name
                                }))}
                                />

                                {validationErrors.sourceId && (
                                    <span className="mt-1 text-sm text-red-500">
                                        {validationErrors.sourceId}
                                    </span>
                                )}

                            </div>

                             <div className="flex flex-col">

                                <label className="mb-2 text-base font-medium text-text">Sales Representative</label>
                                
                                <Dropdown label="Sales Representative" className="w-full"
                                value={formData.assignedUserId}
                                    onChange={(value) =>
                                        setFormData((prev) => ({
                                            ...prev,
                                            assignedUserId: value
                                        }))
                                    }
                                    options={users.map((user) => ({
                                        value: user.id,
                                        label: `${user.firstName} ${user.lastName}`
                                    }))}
                                />

                                {validationErrors.assignedUserId && (
                                    <span className="mt-1 text-sm text-red-500">
                                        {validationErrors.assignedUserId}
                                    </span>
                                )}

                            </div>

                        </div>

                       

                        <div className="flex flex-col mb-4">

                            <label className="mb-2 text-base font-medium text-text">Notes</label>
                                
                            <textarea
                                placeholder="Add notes about this lead..."
                                name="notes"
                                value={formData.notes}
                                onChange={handleChange}
                                rows={4}
                                className="w-full resize-none border border-border rounded-md px-3 py-2 text-text placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary"
                            />
                        
                        </div>

                        <div className="flex justify-end gap-2">

                            <Button variant="outline"
                                type="button"
                                onClick={() => {closeLeadModal()}}
                            >
                                Cancel
                            </Button>

                            <Button variant="primary" type="submit">
                                {isEditMode ? "Update Lead" : "Add Lead"}
                            </Button>
                        </div>
                        
                        {submitError && (
                            <div className="mt-4 p-3 rounded-md bg-red-50 border border-red-200 text-red-600">
                                {submitError}
                            </div>
                        )}

                    </form>


                </Modal>

                <Modal
                    isOpen={isNotesModalOpen}
                    onClose={() => setIsNotesModalOpen(false)}
                    title={`${selectedLead?.firstName} ${selectedLead?.lastName} - Notes`}
                >
                    <p className="text-text text-base border border-border p-3 rounded-md bg-surface-secondary">{selectedLead?.notes || "No notes available."}</p>
                </Modal>

                <Modal
                    isOpen={isDeleteModalOpen}
                    onClose={() => {
                        setIsDeleteModalOpen(false);
                        setDeleteError("");
                        setSelectedLead(null);
                    }}
                    title={`Remove Lead - ${selectedLead?.firstName} ${selectedLead?.lastName}`}
                >
                    {deleteError && (
                        <div className="mt-4 p-3 rounded-md bg-red-50 border border-red-200 text-red-600">
                            {deleteError}
                        </div>
                    )}
                    <p className="text-text text-base">Are you sure you want to remove this lead?</p>
                    <div className="flex justify-end gap-2 mt-4">
                        <Button variant="outline" type="button" onClick={() => {
                            setIsDeleteModalOpen(false);
                            setDeleteError("");
                            setSelectedLead(null);
                        }}>
                            Cancel
                        </Button>
                        <Button variant="danger" type="button" onClick={handleDelete}>
                            Remove
                        </Button>
                    </div>
                </Modal>
                
            </main>
                
           
        </div>
    );
};