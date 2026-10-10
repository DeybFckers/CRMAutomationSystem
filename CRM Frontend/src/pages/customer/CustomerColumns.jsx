import { Building2, UserRound } from "lucide-react";

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

const getStatus = (statusName) => {
    const status = statusName
        ?.trim()
        .toUpperCase()
        .replace(/\s+/g, "_");

    return {
        label: statusLabels[status] ?? statusName ?? "-",
        className: statusStyles[status] ?? "bg-slate-100 text-slate-700"
    };
};

const getInitials = (firstName, lastName) => {
    const first = firstName?.trim()?.[0] ?? "";
    const last = lastName?.trim()?.[0] ?? "";

    return `${first}${last}`.toUpperCase() || "?";
};

export const CustomerColumns = () => [
    {
        accessorKey: "customerCode",
        header: "Code",
        cell: ({ row }) => {
            const customer = row.original;

            return (
                <span className="font-medium text-text">
                    {customer.customerCode || "-"}
                </span>
            );
        }
    },
    {
        accessorKey: "firstName",
        header: "Customer",
        cell: ({ row }) => {
            const customer = row.original;
            const fullName = [
                customer.firstName,
                customer.lastName
            ].filter(Boolean).join(" ");

            return (
                <div className="flex items-center gap-3 min-w-45">
                    <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-primary/10 text-sm font-semibold text-primary">
                        {getInitials(
                            customer.firstName,
                            customer.lastName
                        )}
                    </div>

                    <span className="font-medium text-text">
                        {fullName || "-"}
                    </span>
                </div>
            );
        }
    },
    {
        accessorKey: "companyName",
        header: "Company",
        cell: ({ row }) => {
            const companyName = row.original.companyName;

            return companyName ? (
                <div className="flex items-center gap-2">
                    <Building2
                        size={16}
                        className="shrink-0 text-text-secondary"
                    />

                    <span className="text-text">
                        {companyName}
                    </span>
                </div>
            ) : (
                <span className="text-text-secondary">-</span>
            );
        }
    },
    {
        accessorKey: "email",
        header: "Email",
        cell: ({ row }) => {
            return (
                <span className="text-text-secondary">
                    {row.original.email || "-"}
                </span>
            );
        }
    },
    {
        accessorKey: "phone",
        header: "Phone",
        cell: ({ row }) => {
            return (
                <span className="text-text-secondary">
                    {row.original.phone || "-"}
                </span>
            );
        }
    },
    {
        accessorKey: "assignedUser",
        header: "Assigned",
        cell: ({ row }) => {
            const user = row.original.assignedUser;

            if (!user) {
                return (
                    <span className="text-text-secondary">
                        Unassigned
                    </span>
                );
            }

            const fullName = [
                user.firstName,
                user.lastName
            ].filter(Boolean).join(" ");

            return (
                <div className="flex items-center gap-2 min-w-37.5">
                    <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-slate-100 text-slate-600">
                        <UserRound size={15} />
                    </div>

                    <span className="text-text">
                        {fullName || "-"}
                    </span>
                </div>
            );
        }
    },
    {
        accessorKey: "status",
        header: "Status",
        cell: ({ row }) => {
            const { label, className } = getStatus(
                row.original.status
            );

            return (
                <span
                    className={`inline-flex items-center rounded-full px-3 py-1 text-xs font-medium ${className}`}
                >
                    {label}
                </span>
            );
        },
    },
];