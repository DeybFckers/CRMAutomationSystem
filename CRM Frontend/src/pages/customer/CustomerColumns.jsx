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

export const CustomerColumns = () => [
    {
        accessorKey:"customerCode",
        header: "Code",
        cell: ({ row }) => {
            const customer = row.original;

            return (
                <span className="font-medium text-text">
                    {customer.customerCode}
                </span>
            );
        }
    },
    {
        accessorKey:"firstName",
        header: "Name",
        cell: ({ row }) => {
            const customer = row.original;

            return (
                <span className="font-medium text-text">
                    {customer.firstName} {customer.lastName}
                </span>
            );
        }
    },
    {
        accessorKey: "companyName",
        header: "Company"
    },
    {
        accessorKey: "email",
        header: "Email"
    },
    {
        accessorKey: "phone",
        header: "Phone"
    },
    {
        accessorKey: "assignedUser",
        header: "Assigned",
        cell: ({ row }) => {
            const user = row.original.assignedUser;

            return user
                ? `${user.firstName} ${user.lastName}`
                : "-";
        }
    },
    {
        accessorKey: "status",
        header: "Status",
        cell: ({ row }) => {
            const status = row.original.status
                ?.trim()
                .toUpperCase()
                .replace(/\s+/g, "_");

            const label = statusLabels[status] ?? row.original.status ?? "-";

            const style = statusStyles[status] ?? "bg-gray-100 text-gray-700";

            return (
                <span
                    className={`inline-flex items-center rounded-full px-2.5 py-1 text-xs font-medium ${style}`}
                >
                    {label}
                </span>
            );
        },
    },
]