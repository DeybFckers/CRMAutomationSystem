import { NavLink } from "react-router-dom";

export const Sidebar = () => {

    const navLinkClass = ({ isActive }) =>
        `w-full px-4 py-2.5 rounded-lg text-sm font-medium text-left transition-colors ${
            isActive
                ? "bg-primary-light text-primary"
                : "text-text-secondary hover:bg-surface-secondary hover:text-text"
        }`;

    return (
        <aside className="h-screen w-64 flex flex-col justify-between bg-surface rounded-tr-2xl rounded-br-2xl border-r border-border shadow-xs p-4">

            <div>

                <div className="px-4 py-4.5">
                    <h1 className="text-xl font-bold text-text">
                        CRM System
                    </h1>
                </div>

                <nav className="flex flex-col gap-1">

                    <NavLink to="/dashboard"className={navLinkClass}>
                        Dashboard
                    </NavLink>

                    <div className="mt-2 mb-1 px-4">
                        <span className="text-xs font-semibold tracking-wide text-text-muted">
                            SALES
                        </span>
                    </div>

                    <NavLink to="/leads" className={navLinkClass} >
                        Leads
                    </NavLink>

                    <NavLink to="/customers"className={navLinkClass}>
                        Customers
                    </NavLink>

                    <NavLink to="/opportunities"className={navLinkClass}>
                        Opportunities
                    </NavLink>

                    <NavLink to="/pipeline" className={navLinkClass}>
                        Pipeline
                    </NavLink>

                    <div className="mt-2 mb-1 px-4">
                        <span className="text-xs font-semibold tracking-wide text-text-muted">
                            WORK
                        </span>
                    </div>

                    <NavLink to="/tasks"className={navLinkClass}>
                        Tasks
                    </NavLink>

                    <NavLink to="/activities"className={navLinkClass}>
                        Activities
                    </NavLink>

                    <NavLink to="/notes" className={navLinkClass}>
                        Notes
                    </NavLink>

                </nav>
            </div>

            <div>

                <button className="w-full px-4 py-2.5 rounded-lg text-sm font-medium text-left transition-colors text-text-secondary hover:bg-surface-secondary hover:text-text cursor-pointer">
                    Settings
                </button>

                <button className="w-full mt-1 px-4 py-2.5 rounded-lg text-sm font-medium text-danger text-left hover:bg-danger-light transition-colors">Logout</button>

            </div>

        </aside>
    );
};