import { FaFacebook, FaGoogle, FaLinkedin } from "react-icons/fa";
import { Globe, Handshake, Building2 } from "lucide-react";

const getInitials = (firstName, lastName) => {
    const first = firstName?.trim()?.[0] ?? "";
    const last = lastName?.trim()?.[0] ?? "";

    return `${first}${last}`.toUpperCase() || "?";
};

const getValueStyle = (value) => {
    if (value < 50000) {
        return "bg-slate-300 text-slate-700";
    }

    if (value < 100000) {
        return "bg-amber-100 text-amber-700";
    }

    if (value < 250000) {
        return "bg-blue-100 text-blue-700";
    }

    return "bg-green-100 text-green-700";
};

const getStatusStyle = (statusName) => {
    switch (statusName?.toLowerCase()) {
        case "new":
            return "bg-blue-100 text-blue-700";

        case "contacted":
            return "bg-amber-100 text-amber-700";

        case "qualified":
            return "bg-purple-100 text-purple-700";

        case "unqualified":
            return "bg-red-100 text-red-700";

        case "converted":
            return "bg-green-100 text-green-700";

        default:
            return "bg-slate-100 text-slate-700";
    }
};

const getSource = (sourceName) => {
    switch (sourceName?.toLowerCase()) {
        case "facebook":
            return {
                icon: FaFacebook,
                className: "bg-blue-100 text-blue-700"
            };

        case "google ads":
            return {
                icon: FaGoogle,
                className: "bg-red-100 text-red-600"
            };

        case "linkedin":
            return {
                icon: FaLinkedin,
                className: "bg-sky-100 text-sky-700"
            };

        case "referral":
            return {
                icon: Handshake,
                className: "bg-purple-100 text-purple-700"
            };

        case "website":
            return {
                icon: Globe,
                className: "bg-green-100 text-green-700"
            };

        default:
            return {
                icon: Globe,
                className: "bg-slate-100 text-slate-700"
            };
    }
};

const getLeadAge = (createdAt) => {
    const createdDate = new Date(createdAt);
    const now = new Date();

    const difference = now - createdDate;

    const minutes = Math.floor(difference / (1000 * 60));
    const hours = Math.floor(difference / (1000 * 60 * 60));
    const days = Math.floor(difference / (1000 * 60 * 60 * 24));
    const weeks = Math.floor(days / 7);
    const months = Math.floor(days / 30);
    const years = Math.floor(days / 365);

    if (minutes < 1) {
        return "Just now";
    }

    if (minutes < 60) {
        return `${minutes} min`;
    }

    if (hours < 24) {
        return `${hours} hrs`;
    }

    if (days < 7) {
        return `${days} days`;
    }

    if (days < 30) {
        return `${weeks} weeks`;
    }

    if (days < 365) {
        return `${months} months`;
    }

    return `${years} year${years > 1 ? "s" : ""}`;
};

export const LeadColumns = () => [
    {
        accessorKey: "firstName",
        header: "Lead",
        cell: ({ row }) => {
            const lead = row.original;

            return (
                <div className="flex items-center gap-3 min-w-45">
                    <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-primary/10 text-sm font-semibold text-primary">
                        {getInitials(
                            lead.firstName,
                            lead.lastName
                        )}
                    </div>

                    <span className="font-medium text-text">
                        {lead.firstName} {lead.lastName}
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
        accessorKey: "source",
        header: "Source",
        cell: ({ row }) => {
            const source = row.original.source;

            if (!source) {
                return "-";
            }

            const { icon: Icon, className } = getSource(source.name);

            return (
                <div className="flex items-center gap-2">
                    <span
                        className={`flex h-7 w-7 items-center justify-center rounded-full ${className}`}
                    >
                        <Icon size={14} />
                    </span>

                    <span>
                        {source.name}
                    </span>
                </div>
            );
        }
    },
    {
        accessorKey: "estimatedValue",
        header: "Value",
        cell: ({ getValue }) => {
            const value = Number(getValue());

            if (!value) {
                return "-";
            }

            return (
                <span
                    className={`inline-flex items-center rounded-full px-3 py-1 ${getValueStyle(value)}`}
                >
                    ₱{value.toLocaleString()}
                </span>
            );
        }
    },
    {
        accessorKey: "status",
        header: "Status",
        cell: ({ row }) => {
            const status = row.original.status;

            if (!status) {
                return "-";
            }

            return (
                <span
                    className={`inline-flex items-center rounded-full px-3 py-1 ${getStatusStyle(status.name)}`}
                >
                    {status.name}
                </span>
            );
        }
    },
    {
        accessorKey: "assignedUser",
        header: "Sales Representative",
        cell: ({ row }) => {
            const user = row.original.assignedUser;

            return user
                ? `${user.firstName} ${user.lastName}`
                : "-";
        }
    },
    {
        accessorKey: "createdAt",
        header: "Lead Age",
        cell: ({ getValue }) => {
            return getLeadAge(getValue());
        }
    }
];