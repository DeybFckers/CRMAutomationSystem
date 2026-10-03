import { useState } from "react";
import { Button } from "../../components/common/Button"
import { Dropdown } from "../../components/common/Dropdown";
import { LeadColumns } from "./LeadColumns";
import { DataTable } from "../../components/common/DataTable";
import { Modal } from "../../components/common/Modal";
import { useLeads } from "../../hooks/useLeads";
import { createLead, updateLead, deleteLead, convertLead } from "../../services/core/leadsService";
import { useUsers } from "../../hooks/useUsers";
import { LeadValidator } from "./LeadValidator";
import { Pagination } from "../../components/common/Pagination";


export const Leads = () => {

    // ====================
    // Hooks / Data
    // ====================

    const {leads, leadStatuses, leadSources, pagination, setPagination, filters, setFilters, loading, error, refetch } = useLeads();

    const { users } = useUsers();


    // ====================
    // Modal / Selection State
    // ====================

    const [selectedLead, setSelectedLead] = useState(null);
    const [isEditMode, setIsEditMode] = useState(false);

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [isConvertModalOpen, setIsConvertModalOpen] = useState(false);
    const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);


    // ====================
    // Form State or form from the modal
    // ====================

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
    });

    const [validationErrors, setValidationErrors] = useState({});
    const [submitError, setSubmitError] = useState("");
    const [deleteError, setDeleteError] = useState("");
    const [convertError, setConvertError] = useState("");


    // ====================
    // Form Helpers
    // ====================

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

    const closeLeadModal = () => {
        setIsModalOpen(false);
        setIsEditMode(false);
        setSelectedLead(null);
        setSubmitError("");
        setValidationErrors({});
        resetForm();
    };


    // ====================
    // Form Input
    // ====================

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));
    };


    // ====================
    // Create / Update Lead
    // ====================

    const handleSubmit = async (e) => {
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
            //if the modal is in edit mode
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

        } catch (error) {
            setSubmitError(
                isEditMode
                    ? "Failed to update lead."
                    : "Failed to create lead."
            );
        }
    };


    // ====================
    // Edit Lead
    // ====================

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


    // ====================
    // Delete Lead
    // ====================

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

    //Convert Lead to customer
    const handleConvert = async () => {
        setConvertError("");

        try{
            await convertLead(selectedLead.id);

            await refetch();

            setIsConvertModalOpen(false);
            setSelectedLead(null)
        }catch(error){
            setConvertError("Failed to Convert Lead")
        }
    }


    // ====================
    // Table Columns / Actions
    // ====================

    const columns = LeadColumns({
        onViewDetails: (lead) => {
            setSelectedLead(lead);
            setIsDetailsModalOpen(true);
        },
    });

    const inputClass = "h-10 p-2 w-xs border border-border rounded-md text-text placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary";

    return (
        <div className="min-h-screen bg-background flex p-6">

            <main className="bg-surface border border-border w-full rounded-xl shadow-md p-8">
                <div className="flex justify-between mb-5">
                    <div className="flex items-center gap-3">
                    <p className="text-2xl font-bold text-text">
                        Leads
                    </p>

                    <span className="px-3 py-1 text-sm font-medium rounded-full bg-slate-100 text-text-secondary">
                        {pagination.totalCount}
                    </span>
                     </div>


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
                ) : leads.length === 0 ? (
                    <div className="flex flex-col items-center justify-center min-h-165 text-center">
                        <p className="text-lg font-semibold text-text">
                            {filters.search || filters.statusId || filters.sourceId || filters.assignedUserId
                                ? "No leads found"
                                : "No leads yet"}
                        </p>

                        <p className="mt-1 text-sm text-text-secondary">
                            {filters.search || filters.statusId || filters.sourceId || filters.assignedUserId
                                ? "Try adjusting your search or filters."
                                : "Create your first lead to get started."}
                        </p>
                    </div>
                ) : (
                    <DataTable
                        data={leads}
                        columns={columns}
                    />
                )}

                <Pagination
                    pagination={pagination}
                    onPageChange={(page) => {
                        setPagination(prev => ({
                            ...prev,
                            page,
                        }));
                    }}
                />

                {/* MODAL */}
                 <Modal
                    isOpen={isModalOpen}
                    onClose={() => {closeLeadModal()}}
                    title={isEditMode ? "Edit Lead" : "Add Lead"}
                    >
                    <form onSubmit={handleSubmit} className="flex flex-col ">
                        {/* Lead Name */}
                        <div className="flex justify-between gap-3 mb-4">

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
                        {/* Lead Email and Phone */}
                        <div className="flex justify-between mb-4">
                            
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
                        {/* Lead Company and Estimated Value */}
                        <div className="flex justify-between mb-4">

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
                        {/* Lead Status, Source, and Sales Representative */}
                        <div className="flex justify-between mb-4">

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
                        {/* NOTES */}
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
                
                {/* Delete Modal */}
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

                {/* Convert Modal */}
                <Modal
                    isOpen={isConvertModalOpen} // open the modal
                    onClose={() => { //X BUTTON
                        setIsConvertModalOpen(false);// close the modal
                        setConvertError("");
                        setSelectedLead(null);// remove the selected lead
                    }}
                    title={`Convert Lead - ${selectedLead?.firstName} ${selectedLead?.lastName}`}
                    >
                    {convertError && (
                        <div className="mt-4 p-3 rounded-md bg-red-50 border border-red-200 text-red-600">
                            {convertError}
                        </div>
                    )}
                    <p className="text-text text-base">Are you sure you want to Convert this lead?</p>
                    <div className="flex justify-end gap-2 mt-4">
                        <Button variant="outline" type="button" onClick={() => {
                            setIsConvertModalOpen(false);
                            setConvertError("");
                            setSelectedLead(null);
                        }}>
                            Cancel
                        </Button>
                        <Button variant="primary" type="button" onClick={handleConvert}>
                            Convert
                        </Button>
                    </div>
                </Modal>
                {/* Details Modal */}
                <Modal
                    isOpen={isDetailsModalOpen}
                    onClose={() => {
                        setIsDetailsModalOpen(false);
                        setSelectedLead(null);
                    }}
                    title={`${selectedLead?.firstName} ${selectedLead?.lastName}`}
                    >
                    {selectedLead && (
                        <div className="space-y-6 w-125">

                            {/* Contact Information */}
                            <div>
                                <h3 className="mb-3 font-semibold text-text">
                                    Contact Information
                                </h3>

                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <p className="text-sm text-text-secondary">Email</p>
                                        <p className="text-text wrap-break-word">
                                            {selectedLead.email || "-"}
                                        </p>
                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">Phone</p>
                                        <p className="text-text">
                                            {selectedLead.phone || "-"}
                                        </p>
                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">Company</p>
                                        <p className="text-text">
                                            {selectedLead.companyName || "-"}
                                        </p>
                                    </div>
                                </div>
                            </div>

                            {/* Lead Information */}
                            <div>
                                <h3 className="mb-3 font-semibold text-text">
                                    Lead Information
                                </h3>

                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <p className="text-sm text-text-secondary">Status</p>
                                        <p className="text-text">
                                            {selectedLead.status?.name || "-"}
                                        </p>
                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">Source</p>
                                        <p className="text-text">
                                            {selectedLead.source?.name || "-"}
                                        </p>
                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Estimated Value
                                        </p>
                                        <p className="text-text">
                                            {selectedLead.estimatedValue
                                                ? `₱${Number(selectedLead.estimatedValue).toLocaleString()}`
                                                : "-"}
                                        </p>
                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Sales Representative
                                        </p>
                                        <p className="text-text">
                                            {selectedLead.assignedUser
                                                ? `${selectedLead.assignedUser.firstName} ${selectedLead.assignedUser.lastName}`
                                                : "-"}
                                        </p>
                                    </div>
                                </div>
                            </div>

                            {/* Notes */}
                            <div>
                                <h3 className="mb-3 font-semibold text-text">
                                    Notes
                                </h3>

                                <p className="rounded-md border border-border bg-surface-secondary p-3 text-text">
                                    {selectedLead.notes || "No notes available."}
                                </p>
                            </div>

                            {/* Dates */}
                            <div>
                                <h3 className="mb-3 font-semibold text-text">
                                    Record Information
                                </h3>

                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Created
                                        </p>
                                        <p className="text-text">
                                            {new Date(
                                                selectedLead.createdAt
                                            ).toLocaleString()}
                                        </p>
                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Updated
                                        </p>
                                        <p className="text-text">
                                            {new Date(
                                                selectedLead.updatedAt
                                            ).toLocaleString()}
                                        </p>
                                    </div>
                                </div>
                            </div>

                            {/* Actions */}
                            <div className="flex justify-end gap-2 border-t border-border pt-4">
                                <Button
                                    variant="outline"
                                    type="button"
                                    onClick={() => {
                                        setIsDetailsModalOpen(false);
                                        handleEdit(selectedLead);
                                    }}
                                >
                                    Edit
                                </Button>

                                <Button
                                    variant="primary"
                                    type="button"
                                    onClick={() => {
                                        setIsDetailsModalOpen(false);
                                        setIsConvertModalOpen(true);
                                    }}
                                >
                                    Convert
                                </Button>

                                <Button
                                    variant="danger"
                                    type="button"
                                    onClick={() => {
                                        setIsDetailsModalOpen(false);
                                        setIsDeleteModalOpen(true);
                                    }}
                                >
                                    Delete
                                </Button>
                            </div>

                        </div>
                    )}
                </Modal>
                
            </main>
                
           
        </div>
    );
};