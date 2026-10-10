import { TableActions } from "../../../components/common/TableActions";

    const statusLabels = {
        ACTIVE: "Active",
        INACTIVE: "Inactive",
        ON_HOLD: "On Hold",
    };

    const statusStyles = {
        ACTIVE: "bg-green-100 text-green-700",
        INACTIVE: "bg-red-100 text-red-700",
        ON_HOLD: "bg-amber-100 text-amber-700",
    };

export const CustomerOverview = ({ customer, onEdit, onHold, onDeactivate, onActivate }) => {

    // Normalize status to match the standard backend values.
    const status = customer.status
        ?.trim()
        .toUpperCase()
        .replace(/\s+/g, "_");

    const statusLabel = statusLabels[status] ?? customer.status ?? "-";
    const statusStyle =
        statusStyles[status] ?? "bg-gray-100 text-gray-700";

    // Edit is available for every status.
    const actions = [
        {
            label: "Edit",
            onClick: onEdit,
        },
    ];

    // Show actions based on the customer's current status.
    if (status === "ACTIVE") {
        actions.push(
            {
                label: "Put on Hold",
                onClick: onHold,
            },
            {
                label: "Deactivate",
                variant: "danger",
                onClick: onDeactivate,
            }
        );
    } else {
        actions.push({
            label: "Activate",
            onClick: onActivate,
        });
    }

    return (
        <div className="flex flex-col gap-4">

            <div className="flex justify-between items-center">

                <h3 className="text-lg font-semibold text-text">
                    Customer Information
                </h3>

                <TableActions options={actions} />

            </div>


            <div className="border border-border rounded-lg p-4">

                <div className="grid grid-cols-2 gap-x-8 gap-y-5">

                    {/* Name */}
                    <div>
                        <p className="text-sm text-text-secondary">
                            Name
                        </p>

                        <p className="text-text font-medium">
                            {customer.firstName}{" "}
                            {customer.lastName}
                        </p>
                    </div>

                    {/* Status */}
                    <div>
                        <p className="text-sm text-text-secondary mb-1">
                            Status
                        </p>

                        <span
                            className={`inline-flex rounded-full px-3 py-1 text-sm font-medium ${statusStyle}`}
                        >
                            {statusLabel}
                        </span>
                    </div>

                    {/* Company */}
                    <div>
                        <p className="text-sm text-text-secondary">
                            Company Name
                        </p>

                        <p className="text-text font-medium">
                            {customer.companyName || "-"}
                        </p>
                    </div>


                    {/* Sales Representative */}
                    <div>
                        <p className="text-sm text-text-secondary">
                            Sales Representative
                        </p>

                        <p className="text-text font-medium">
                            {customer.assignedUser
                                ? `${customer.assignedUser.firstName} ${customer.assignedUser.lastName}`
                                : "-"
                            }
                        </p>
                    </div>


                    {/* Email */}
                    <div>
                        <p className="text-sm text-text-secondary">
                            Email
                        </p>

                        <p className="text-text wrap-break-words font-medium">
                            {customer.email || "-"}
                        </p>
                    </div>


                    {/* Phone */}
                    <div>
                        <p className="text-sm text-text-secondary">
                            Phone
                        </p>

                        <p className="text-text font-medium">
                            {customer.phone || "-"}
                        </p>
                    </div>


                    {/* Customer Code */}
                    <div>
                        <p className="text-sm text-text-secondary">
                            Customer Code
                        </p>

                        <p className="text-text font-medium">
                            {customer.customerCode || "-"}
                        </p>
                    </div>


                    {/* Created At */}
                    <div>
                        <p className="text-sm text-text-secondary">
                            Created At
                        </p>

                        <p className="text-text font-medium">
                            {customer.createdAt
                                ? new Date(
                                    customer.createdAt
                                ).toLocaleDateString()
                                : "-"
                            }
                        </p>
                    </div>

                </div>

            </div>
        </div>
    );
};