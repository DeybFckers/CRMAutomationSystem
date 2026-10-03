export const CustomerColumns = ({ onViewDetails }) => [
    {
        accessorKey:"firstName",
        header: "Name",
        cell: ({ row }) => {
            const lead = row.original;

            return (
                <button
                    type="button"
                    onClick={() => onViewDetails(lead)}
                    className="font-medium text-text hover:text-primary hover:underline cursor-pointer"
                >
                    {lead.firstName} {lead.lastName}
                </button>
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
    {
        id:"actions",
        header:"Actions"
    }
]