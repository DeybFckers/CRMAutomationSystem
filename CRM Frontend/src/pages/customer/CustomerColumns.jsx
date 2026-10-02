export const CustomerColumns = ({onView, onEdit, onDelete, onAssign}) => [
    {
        accessorKey:"firstName",
        header: "Name",
        cell: ({ row }) => {
            const { firstName, lastName } = row.original;

            return `${firstName} ${lastName}`;
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