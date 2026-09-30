import { useState } from "react";
import Button from "../../components/common/Button"
import { Dropdown } from "../../components/common/Dropdown";
import { LeadColumns } from "./LeadColumns";
import { DataTable } from "../../components/common/DataTable";
import { Modal } from "../../components/common/Modal";
import { useLeads } from "../../hooks/useLeads";
import { createLead } from "../../services/core/leadsService";
import { useUsers } from "../../hooks/useUsers";

export const Leads = () => {

    const {
        leads,
        leadStatuses,
        leadSources,
        pagination,
        setPagination,
        filters,
        setFilters,
        loading,
        error,
        refetch
    } = useLeads();

    const {
        users,
        loading: usersLoading,
        error: usersError
    } = useUsers();

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [validationErrors, setValidationErrors] = useState({});
    const [submitError, setSubmitError] = useState("");

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

    const validateForm = () => {
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
        setValidationErrors(errors);

        return Object.keys(errors).length === 0;
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

        const isValid = validateForm();

        if (!isValid) {
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

            await createLead(leadData);
            await refetch();

            setIsModalOpen(false);
            resetForm();
        }catch (error) {
            setSubmitError("Failed to create lead.");
        }
    }

    const columns = LeadColumns({
        onView: (lead) => {
            console.log("View lead notes:", lead);
        },
        onEdit: (lead) => {
            console.log("Edit lead:", lead);
        },
        onDelete: (lead) => {
            console.log("Delete lead:", lead);
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

                    <Button onClick={() => {resetForm(); setIsModalOpen(true)}}>
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
                    onClose={() => setIsModalOpen(false)}
                    title="Add Lead"
                >
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

                            <Button variant="outline" type="button" onClick={() => setIsModalOpen(false)}>
                                Cancel
                            </Button>

                            <Button variant="primary" type="submit">
                                Create
                            </Button>
                        </div>
                        
                        {submitError && (
                            <div className="mt-4 p-3 rounded-md bg-red-50 border border-red-200 text-red-600">
                                {submitError}
                            </div>
                        )}

                    </form>


                </Modal>
                
            </main>
                
           
        </div>
    );
};