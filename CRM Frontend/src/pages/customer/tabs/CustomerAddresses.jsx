import { useState } from "react";
import { Button } from "../../../components/common/Button";
import { Modal } from "../../../components/common/Modal";
import { Dropdown } from "../../../components/common/Dropdown";
import { TableActions } from "../../../components/common/TableActions";
import {addCustomerAddress, updateCustomerAddress,deleteCustomerAddress } from "../../../services/core/customerService";
import { CustomerAddressValidator } from "./validator/CustomerAddressValidator";

export const CustomerAddresses = ({ customer, onAddressesChanged }) => {
    const [selectedAddress, setSelectedAddress] = useState(null);
    const [isAddressModalOpen, setIsAddressModalOpen] = useState(false);
    const [isEditMode, setIsEditMode] = useState(false);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [validationErrors, setValidationErrors] = useState({});

    const [submitError, setSubmitError] = useState("");
    const [deleteError, setDeleteError] = useState("");

    const [formData, setFormData] = useState({
        type: "",
        addressLine: "",
        city: "",
        province: "",
        postalCode: "",
        country: "",
        isPrimary: false,
    });

    const resetForm = () => {
        setFormData({
            type: "",
            addressLine: "",
            city: "",
            province: "",
            postalCode: "",
            country: "",
            isPrimary: false,
        });
    };

    const closeAddressModal = () => {
        if (isSubmitting) return;

        setIsAddressModalOpen(false);
        setIsEditMode(false);
        setSelectedAddress(null);
        setSubmitError("");
        resetForm();
        setValidationErrors({});
    };

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));

        setValidationErrors((prev) => ({
            ...prev,
            [name]: "",
        }));
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        const errors = CustomerAddressValidator(formData);
        
        setValidationErrors(errors);

        if (Object.keys(errors).length > 0) {
            return;
        }

        setSubmitError("");
        setIsSubmitting(true);

        try {
            const addressData = {
                type: formData.type,
                addressLine: formData.addressLine,
                city: formData.city,
                province: formData.province,
                postalCode: formData.postalCode,
                country: formData.country,
                isPrimary: formData.isPrimary,
            };

            if (isEditMode && selectedAddress) {
                await updateCustomerAddress(
                    customer.id,
                    selectedAddress.id,
                    addressData
                );
            } else {
                await addCustomerAddress(
                    customer.id,
                    addressData
                );
            }

            await onAddressesChanged?.();
            closeAddressModal();
        } catch (error) {
            setSubmitError(
                error.response?.data?.message ||
                error.response?.data?.title ||
                (isEditMode
                    ? "Failed to update customer address."
                    : "Failed to create customer address.")
            );
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleEdit = (address) => {
        setSelectedAddress(address);
        setIsEditMode(true);

        setFormData({
            type: address.type ?? "",
            addressLine: address.addressLine ?? "",
            city: address.city ?? "",
            province: address.province ?? "",
            postalCode: address.postalCode ?? "",
            country: address.country ?? "",
            isPrimary: address.isPrimary ?? false,
        });

        setSubmitError("");
        setIsAddressModalOpen(true);
    };

    const openDeleteModal = (address) => {
        setSelectedAddress(address);
        setDeleteError("");
        setIsDeleteModalOpen(true);
    };

    const closeDeleteModal = () => {
        if (isSubmitting) return;

        setIsDeleteModalOpen(false);
        setDeleteError("");
        setSelectedAddress(null);
    };

    const handleDelete = async () => {
        if (!selectedAddress) return;

        setDeleteError("");
        setIsSubmitting(true);

        try {
            await deleteCustomerAddress(
                customer.id,
                selectedAddress.id
            );

            await onAddressesChanged?.();

            setIsDeleteModalOpen(false);
            setSelectedAddress(null);
        } catch (error) {
            setDeleteError(
                error.response?.data?.message ||
                error.response?.data?.title ||
                "Failed to delete customer address."
            );
        } finally {
            setIsSubmitting(false);
        }
    };

    const inputClass =
        "h-10 p-2 w-full border border-border rounded-md text-text placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary";

    const addresses = customer?.addresses ?? [];

    return (
        <div className="flex flex-col gap-4">
            {/* Header */}
            <div className="flex justify-between items-center">
                <h3 className="text-lg font-semibold text-text">
                    Addresses
                </h3>

                <Button
                    variant="primary"
                    type="button"
                    onClick={() => {
                        setSelectedAddress(null);
                        setIsEditMode(false);
                        resetForm();
                        setSubmitError("");
                        setIsAddressModalOpen(true);
                    }}
                >
                    +
                </Button>
            </div>

            {/* Address List */}
            {addresses.length > 0 ? (
                <div className="flex flex-col gap-3">
                    {addresses.map((address) => (
                        <div
                            key={address.id}
                            className="border border-border rounded-lg p-4"
                        >
                            {/* Address Header and Actions */}
                            <div className="flex justify-between items-start gap-4">
                                <div className="min-w-0">
                                    <p className="font-medium text-text">
                                        {address.type || "Address"}
                                    </p>

                                    <p className="text-sm text-text-secondary">
                                        {address.addressLine || "-"}
                                    </p>
                                </div>

                                <div className="flex items-center gap-2 shrink-0">
                                    {address.isPrimary && (
                                        <span className="rounded-full bg-green-100 px-3 py-1 text-xs font-medium text-green-700">
                                            Primary
                                        </span>
                                    )}

                                    <TableActions
                                        options={[
                                            {
                                                label: "Edit",
                                                onClick: () => handleEdit(address),
                                            },
                                            {
                                                label: "Remove",
                                                variant: "danger",
                                                onClick: () => openDeleteModal(address),
                                            },
                                        ]}
                                    />
                                </div>
                            </div>

                            {/* Address Details */}
                            <div className="mt-4 grid grid-cols-2 gap-x-6 gap-y-3">
                                <div>
                                    <p className="text-xs font-medium text-text-secondary">
                                        City
                                    </p>
                                    <p className="mt-1 text-sm font-medium text-text">
                                        {address.city || "-"}
                                    </p>
                                </div>

                                <div>
                                    <p className="text-xs font-medium text-text-secondary">
                                        Province
                                    </p>
                                    <p className="mt-1 text-sm font-medium text-text">
                                        {address.province || "-"}
                                    </p>
                                </div>

                                <div>
                                    <p className="text-xs font-medium text-text-secondary">
                                        Postal Code
                                    </p>
                                    <p className="mt-1 text-sm font-medium text-text">
                                        {address.postalCode || "-"}
                                    </p>
                                </div>

                                <div>
                                    <p className="text-xs font-medium text-text-secondary">
                                        Country
                                    </p>
                                    <p className="mt-1 text-sm font-medium text-text">
                                        {address.country || "-"}
                                    </p>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <div className="border border-border rounded-lg p-6 text-center">
                    <p className="text-text-secondary">
                        No addresses available.
                    </p>
                </div>
            )}

            {/* Add or Edit Address Modal */}
            <Modal
                isOpen={isAddressModalOpen}
                onClose={closeAddressModal}
                title={
                    isEditMode
                        ? `Edit Address - ${customer?.firstName ?? ""} ${customer?.lastName ?? ""}`
                        : `Add Address - ${customer?.firstName ?? ""} ${customer?.lastName ?? ""}`
                }
                className="max-w-fit"
            >
                <form
                    onSubmit={handleSubmit}
                    className="flex flex-col gap-4"
                    noValidate
                >
                    {/* Address Type */}
                    <div className="flex flex-col">
                        <label className="mb-2 text-base font-medium text-text">
                            Address Type
                        </label>

                        <Dropdown
                            label="Select address type"
                            value={formData.type}
                            onChange={(value) =>
                                setFormData((prev) => ({
                                    ...prev,
                                    type: value,
                                }))
                            }
                            options={[
                                { label: "Home", value: "Home" },
                                { label: "Work", value: "Work" },
                                { label: "Billing", value: "Billing" },
                                { label: "Shipping", value: "Shipping" },
                                { label: "Other", value: "Other" },
                            ]}
                            className="w-full"
                        />

                        {!formData.type && (
                            <input
                                type="text"
                                tabIndex={-1}
                                value=""
                                required
                                onChange={() => {}}
                                className="absolute h-0 w-0 opacity-0 pointer-events-none"
                                aria-hidden="true"
                            />
                        )}

                        {validationErrors.type && (
                            <p className="mt-1 text-sm text-red-600">
                                {validationErrors.type}
                            </p>
                        )}
                    </div>

                    {/* Address Line */}
                    <div className="flex flex-col">
                        <label
                            htmlFor="addressLine"
                            className="mb-2 text-base font-medium text-text"
                        >
                            Address Line
                        </label>

                        <input
                            id="addressLine"
                            type="text"
                            name="addressLine"
                            placeholder="House number, street, building"
                            value={formData.addressLine}
                            onChange={handleChange}
                            className={inputClass}
                            required
                        />

                        {validationErrors.addressLine && (
                            <p className="mt-1 text-sm text-red-600">
                                {validationErrors.addressLine}
                            </p>
                        )}
                    </div>

                    {/* Country and City */}
                    <div className="grid grid-cols-2 gap-3">
                        <div className="flex flex-col">
                            <label
                                htmlFor="country"
                                className="mb-2 text-base font-medium text-text"
                            >
                                Country
                            </label>

                            <input
                                id="country"
                                type="text"
                                name="country"
                                placeholder="Enter country"
                                value={formData.country}
                                onChange={handleChange}
                                className={inputClass}
                                required
                            />

                            {validationErrors.country && (
                                <p className="mt-1 text-sm text-red-600">
                                    {validationErrors.country}
                                </p>
                            )}
                        </div>

                        <div className="flex flex-col">
                            <label
                                htmlFor="city"
                                className="mb-2 text-base font-medium text-text"
                            >
                                City
                            </label>

                            <input
                                id="city"
                                type="text"
                                name="city"
                                placeholder="Enter city"
                                value={formData.city}
                                onChange={handleChange}
                                className={inputClass}
                                required
                            />

                            {validationErrors.city && (
                                <p className="mt-1 text-sm text-red-600">
                                    {validationErrors.city}
                                </p>
                            )}
                        </div>
                    </div>

                    {/* Province and Postal Code */}
                    <div className="grid grid-cols-2 gap-3">
                        <div className="flex flex-col">
                            <label
                                htmlFor="province"
                                className="mb-2 text-base font-medium text-text"
                            >
                                Province
                            </label>

                            <input
                                id="province"
                                type="text"
                                name="province"
                                placeholder="Enter province"
                                value={formData.province}
                                onChange={handleChange}
                                className={inputClass}
                                required
                            />

                            {validationErrors.province && (
                                <p className="mt-1 text-sm text-red-600">
                                    {validationErrors.province}
                                </p>
                            )}
                        </div>

                        <div className="flex flex-col">
                            <label
                                htmlFor="postalCode"
                                className="mb-2 text-base font-medium text-text"
                            >
                                Postal Code
                            </label>

                            <input
                                id="postalCode"
                                type="text"
                                name="postalCode"
                                placeholder="Enter postal code"
                                value={formData.postalCode}
                                onChange={handleChange}
                                className={inputClass}
                                required
                            />

                            {validationErrors.postalCode && (
                                <p className="mt-1 text-sm text-red-600">
                                    {validationErrors.postalCode}
                                </p>
                            )}
                        </div>
                    </div>

                    {/* Primary Address */}
                    <div className="flex flex-col">
                        <label className="mb-2 text-base font-medium text-text">
                            Primary Address
                        </label>

                        <label className="flex items-center gap-3 min-h-10 px-3 rounded-md border border-border cursor-pointer transition-colors hover:border-primary focus-within:ring-2 focus-within:ring-primary">
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
                                Set as primary address
                            </span>
                        </label>
                    </div>

                    {/* Submit Error */}
                    {submitError && (
                        <div className="p-3 rounded-md bg-red-50 border border-red-200 text-red-600">
                            {submitError}
                        </div>
                    )}

                    {/* Form Actions */}
                    <div className="flex justify-end gap-2">
                        <Button
                            variant="outline"
                            type="button"
                            onClick={closeAddressModal}
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
                                    ? "Update Address"
                                    : "Add Address"}
                        </Button>
                    </div>
                </form>
            </Modal>

            {/* Delete Address Confirmation Modal */}
            <Modal
                isOpen={isDeleteModalOpen}
                onClose={closeDeleteModal}
                title={`Remove Address - ${customer?.firstName ?? ""} ${customer?.lastName ?? ""}`}
                className="max-w-fit"
            >
                <p className="text-text text-base">
                    Are you sure you want to remove this customer address?
                </p>

                {selectedAddress && (
                    <div className="mt-3 rounded-lg border border-border p-3">
                        <p className="font-medium text-text">
                            {selectedAddress.addressLine || "-"}
                        </p>

                        <p className="text-sm text-text-secondary">
                            {[
                                selectedAddress.city,
                                selectedAddress.province,
                                selectedAddress.postalCode,
                            ]
                                .filter(Boolean)
                                .join(", ")}
                        </p>

                        <p className="text-sm text-text-secondary">
                            {selectedAddress.country || "-"}
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