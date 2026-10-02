import { TableActions } from "../../components/common/TableActions";

export const LeadColumns = ({onView, onEdit, onDelete, onAssign}) => [
  {
        accessorKey: "firstName",
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
        accessorKey: "source",
        header: "Source",
        cell: ({row}) =>{
          return row.original.source?.name ?? "-";
        }
    },
    {
        accessorKey: "estimatedValue",
        header: "Value",
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
        header: "Notes",
        cell: ({ row }) => {
        const lead = row.original;

            if (!lead.notes) {
                return "-";
            }
            return (
                <button
                    type="button"
                    onClick={() => onView(lead)}
                    className="cursor-pointer inline-flex items-center justify-center rounded-full bg-blue-100 px-3 py-1.5 text-sm font-medium text-blue-700 transition-colors hover:bg-blue-200"
                    title="View notes"
                >
                    View
                </button>
            );
        }
    },
    {
        id: "actions",
        header: "Actions",
        cell: ({ row }) => {
            const lead = row.original;

            return (
            <TableActions
                options={[
                    {
                        label: "Edit",
                        onClick: () => onEdit(lead)
                    },
                    {
                        label: "Assign",
                        onClick: () => onAssign(lead)
                    },
                    {
                        label: "Delete",
                        variant: "danger",
                        onClick: () => onDelete(lead)
                    }
                ]}
            />
            );
        }
    }

]