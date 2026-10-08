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
        header: "Status"
    },
]