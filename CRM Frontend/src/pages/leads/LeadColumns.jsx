

export const LeadColumns = [
  {
        accessorKey: "firstName",
        header: "Name",
        cell: ({ row }) => {
            const { firstName, lastName } = row.original;

            return `${firstName} ${lastName}`;
        }
    },
    {
        accessorKey: "email",
        header: "Email"
    },
    {
        accessorKey: "companyName",
        header: "Company"
    },
    {
        accessorKey: "source",
        header: "Source",
        cell: ({row}) =>{
          return row.original.source?.name ?? "-";
        }
    },
    {
        accessorKey: "estimatedValue",
        header: "Estimated Value",
        cell: ({ getValue }) => {
            const value = getValue();

            return value
                ? `₱${value.toLocaleString()}`
                : "-";
        }
    },
    {
        accessorKey: "status",
        header: "Status",
        cell: ({ row }) => {
            return row.original.status?.name ?? "-";
        }
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
        accessorKey: "notes",
        header: "Notes"
    },
]