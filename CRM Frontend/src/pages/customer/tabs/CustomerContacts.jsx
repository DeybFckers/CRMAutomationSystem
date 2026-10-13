import { useState } from "react";
import { Button } from "../../../components/common/Button";
import { Modal } from "../../../components/common/Modal";
import { TableActions } from "../../../components/common/TableActions";
import { addCustomerContact,updateCustomerContact, deleteCustomerContact} from "../../../services/core/customerService";

export const CustomerContacts = ({ customer, onContactsChanged }) => {
    const [selectedContact, setSelectedContact] = useState(null);
    const [isContactModalOpen, setIsContactModalOpen] = useState(false);
    const [isEditMode, setIsEditMode] = useState(false);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [deleteError, setDeleteError] = useState("");
    const [submitError, setSubmitError] = useState("");

    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        position: "",
        email: "",
        phone: "",
        isPrimary: false,
    });

    const resetForm = () => {
        setFormData({
            firstName: "",
            lastName: "",
            position: "",
            email: "",
            phone: "",
            isPrimary: false,
        });
    };

    const closeContactModal = () => {
        setIsContactModalOpen(false);
        setIsEditMode(false);
        setSelectedContact(null);
        setSubmitError("");
        resetForm();
    };

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        setSubmitError("");
        setIsSubmitting(true);

        try {
            const contactData = {
                firstName: formData.firstName,
                lastName: formData.lastName,
                position: formData.position,
                email: formData.email,
                phone: formData.phone,
                isPrimary: formData.isPrimary,
            };

            if (isEditMode && selectedContact) {
                await updateCustomerContact( customer.id, selectedContact.id, contactData );
            } else {
                await addCustomerContact(customer.id, contactData);
            }

            await onContactsChanged?.();
            closeContactModal();
        } catch (error) {
            setSubmitError(
                error.response?.data?.message ||
                error.response?.data?.title ||
                (isEditMode
                    ? "Failed to update contact."
                    : "Failed to create contact.")
            );
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleEdit = (contact) => {
        setSelectedContact(contact);
        setIsEditMode(true);

        setFormData({
            firstName: contact.firstName ?? "",
            lastName: contact.lastName ?? "",
            position: contact.position ?? "",
            email: contact.email ?? "",
            phone: contact.phone ?? "",
            isPrimary: contact.isPrimary ?? false,
        });

        setSubmitError("");
        setIsContactModalOpen(true);
    };

    const openDeleteModal = (contact) => {
        setSelectedContact(contact);
        setDeleteError("");
        setIsDeleteModalOpen(true);
    };

    const closeDeleteModal = () => {
        if (isSubmitting) return;

        setIsDeleteModalOpen(false);
        setDeleteError("");
        setSelectedContact(null);
    };

    const handleDelete = async () => {
        if (!selectedContact) return;

        setDeleteError("");
        setIsSubmitting(true);

        try {
            await deleteCustomerContact(customer.id,selectedContact.id);

            await onContactsChanged?.();

            setIsDeleteModalOpen(false);
            setSelectedContact(null);
        } catch (error) {
            setDeleteError(
                error.response?.data?.message ||
                error.response?.data?.title ||
                "Failed to delete customer contact."
            );
        } finally {
            setIsSubmitting(false);
        }
    };

    const inputClass =
        "h-10 p-2 w-xs border border-border rounded-md text-text placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary";

    const contacts = customer?.contacts ?? [];

    return (
        <div className="flex flex-col gap-4">
            {/* Header */}
            <div className="flex justify-between items-center">
                <h3 className="text-lg font-semibold text-text">
                    Contacts
                </h3>

                <Button
                    variant="primary"
                    type="button"
                    onClick={() => {
                        setIsEditMode(false);
                        setSelectedContact(null);
                        resetForm();
                        setSubmitError("");
                        setIsContactModalOpen(true);
                    }}
                >
                    +
                </Button>
            </div>

            {/* Contact List */}
            {contacts.length > 0 ? (
                <div className="flex flex-col gap-3">
                    {contacts.map((contact) => (
                        <div
                            key={contact.id}
                            className="border border-border rounded-lg p-4"
                        >
                            {/* Contact Header and Actions */}
                            <div className="flex justify-between items-start gap-4">
                                <div className="min-w-0">
                                    <p className="font-medium text-text">
                                        {contact.firstName} {contact.lastName}
                                    </p>

                                    <p className="text-sm text-text-secondary">
                                        {contact.position || "-"}
                                    </p>
                                </div>

                                <div className="flex items-center gap-2 shrink-0">
                                    {contact.isPrimary && (
                                        <span className="rounded-full bg-green-100 px-3 py-1 text-xs font-medium text-green-700">
                                            Primary
                                        </span>
                                    )}

                                    <TableActions
                                        options={[
                                            {
                                                label: "Edit",
                                                onClick: () => handleEdit(contact),
                                            },
                                            {
                                                label: "Remove",
                                                variant: "danger",
                                                onClick: () => openDeleteModal(contact),
                                            },
                                        ]}
                                    />
                                </div>
                            </div>

                            {/* Contact Details */}
                            <div className="grid grid-cols-2 gap-4 mt-4">
                                <div>
                                    <p className="text-sm text-text-secondary">
                                        Email
                                    </p>

                                    <p className="text-text wrap-break-words font-medium">
                                        {contact.email || "-"}
                                    </p>
                                </div>

                                <div>
                                    <p className="text-sm text-text-secondary">
                                        Phone
                                    </p>

                                    <p className="text-text font-medium">
                                        {contact.phone || "-"}
                                    </p>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <div className="border border-border rounded-lg p-6 text-center">
                    <p className="text-text-secondary">
                        No contacts available.
                    </p>
                </div>
            )}

            {/* Add or Edit Contact Modal */}
            <Modal
                isOpen={isContactModalOpen}
                onClose={closeContactModal}
                title={
                    isEditMode
                        ? `Edit Contact - ${customer.firstName} ${customer.lastName}`
                        : `Add Contact - ${customer.firstName} ${customer.lastName}`
                }
                className="max-w-fit"
            >
                <form onSubmit={handleSubmit} className="flex flex-col">
                    {/* First and Last Name */}
                    <div className="flex justify-between gap-3 mb-4">
                        <div className="flex flex-col">
                            <label className="mb-2 text-base font-medium text-text">
                                First Name
                            </label>

                            <input
                                type="text"
                                name="firstName"
                                placeholder="First Name"
                                value={formData.firstName}
                                onChange={handleChange}
                                className={inputClass}
                                required
                            />
                        </div>

                        <div className="flex flex-col">
                            <label className="mb-2 text-base font-medium text-text">
                                Last Name
                            </label>

                            <input
                                type="text"
                                name="lastName"
                                placeholder="Last Name"
                                value={formData.lastName}
                                onChange={handleChange}
                                className={inputClass}
                                required
                            />
                        </div>
                    </div>

                    {/* Email and Phone */}
                    <div className="flex justify-between gap-3 mb-4">
                        <div className="flex flex-col">
                            <label className="mb-2 text-base font-medium text-text">
                                Email
                            </label>

                            <input
                                type="email"
                                name="email"
                                placeholder="john@example.com"
                                value={formData.email}
                                onChange={handleChange}
                                className={inputClass}
                            />
                        </div>

                        <div className="flex flex-col">
                            <label className="mb-2 text-base font-medium text-text">
                                Phone
                            </label>

                            <input
                                type="text"
                                name="phone"
                                placeholder="09XX-XXXX-XXX"
                                value={formData.phone}
                                onChange={handleChange}
                                className={inputClass}
                            />
                        </div>
                    </div>

                    {/* Position and Primary Contact */}
                    <div className="flex justify-between gap-3 mb-4">
                        <div className="flex flex-col">
                            <label className="mb-2 text-base font-medium text-text">
                                Position
                            </label>

                            <input
                                type="text"
                                name="position"
                                placeholder="e.g. Manager"
                                value={formData.position}
                                onChange={handleChange}
                                className={inputClass}
                            />
                        </div>

                        <div className="flex flex-col">
                            <label className="mb-2 text-base font-medium text-text">
                                Primary Contact
                            </label>

                            <label className="flex items-center gap-3 h-10 w-xs px-3 rounded-md border border-border cursor-pointer transition-colors hover:border-primary focus-within:ring-2 focus-within:ring-primary">
                                <input
                                    type="checkbox"
                                    name="isPrimary"
                                    checked={formData.isPrimary}
                                    onChange={(e) =>
                                        setFormData((prev) => ({
                                            ...prev,
                                            isPrimary: e.target.checked,
                                        }))
                                    }
                                    className="h-4 w-4 accent-primary"
                                />

                                <span className="text-sm text-text">
                                    Set as primary contact
                                </span>
                            </label>
                        </div>
                    </div>

                    {/* Submit Error */}
                    {submitError && (
                        <div className="mb-4 p-3 rounded-md bg-red-50 border border-red-200 text-red-600">
                            {submitError}
                        </div>
                    )}

                    {/* Form Actions */}
                    <div className="flex justify-end gap-2">
                        <Button
                            variant="outline"
                            type="button"
                            onClick={closeContactModal}
                            disabled={isSubmitting}
                        >
                            Cancel
                        </Button>

                        <Button
                            variant="primary"
                            type="submit"
                            disabled={isSubmitting}
                        >
                            {isSubmitting
                                ? "Saving..."
                                : isEditMode
                                    ? "Update Contact"
                                    : "Add Contact"}
                        </Button>
                    </div>
                </form>
            </Modal>

            {/* Delete Contact Confirmation Modal */}
            <Modal
                isOpen={isDeleteModalOpen}
                onClose={closeDeleteModal}
                title={`Remove Contact - ${customer?.firstName ?? ""} ${customer?.lastName ?? ""}`}
                className="max-w-fit"
            >
                <p className="text-text text-base">
                    Are you sure you want to remove this customer contact?
                </p>

                {selectedContact && (
                    <div className="mt-3 rounded-lg border border-border p-3">
                        <p className="font-medium text-text">
                            {selectedContact.firstName} {selectedContact.lastName}
                        </p>

                        <p className="text-sm text-text-secondary">
                            {selectedContact.email || "-"}
                        </p>
                    </div>
                )}

                {deleteError && (
                    <div className="mt-4 p-3 rounded-md bg-red-50 border border-red-200 text-red-600">
                        {deleteError}
                    </div>
                )}

                <div className="flex justify-end gap-2 mt-4">
                    <Button
                        variant="outline"
                        type="button"
                        onClick={closeDeleteModal}
                        disabled={isSubmitting}
                    >
                        Cancel
                    </Button>

                    <Button
                        variant="danger"
                        type="button"
                        onClick={handleDelete}
                        disabled={isSubmitting}
                    >
                        {isSubmitting ? "Removing..." : "Remove"}
                    </Button>
                </div>
            </Modal>
        </div>
    );
};