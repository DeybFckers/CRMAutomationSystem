import { TableActions } from "../../../components/common/TableActions";

export const CustomerOverview = ({ customer }) => {

    return (
        <div className="flex flex-col gap-4">

            <div className="flex justify-between items-center">

                <h3 className="text-lg font-semibold text-text">
                    Customer Information
                </h3>

                <TableActions 
                    options = {[
                         {
                            label: "Edit",
                            onClick: () => onEdit(lead)
                        },
                        {
                            label: "Delete",
                            variant: "danger",
                            onClick: () => onDelete(lead)
                        }
                    ]}
                />

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

                        <span className="inline-flex rounded-full bg-green-100 px-3 py-1 text-sm font-medium text-green-700">
                            {customer.status || "-"}
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