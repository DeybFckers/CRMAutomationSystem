import { useState } from "react";

export const TableActions = ({ options = [] }) => {
    const [open, setOpen] = useState(false);

    return (
        <div className="relative">
            <button
                type="button"
                onClick={() => setOpen(prev => !prev)}
                className="flex cursor-pointer list-none items-center justify-center rounded-md p-1.5 text-lg text-text-secondary hover:bg-slate-100 hover:text-text"
                title="Actions"
            >
                ⋮
            </button>

            {open && (
                <div className="absolute right-0 z-50 mt-1 w-fit min-w-28 rounded-lg border border-border bg-surface py-1 shadow-lg">
                    {options.map((option) => (
                        <button
                            key={option.label}
                            type="button"
                            onClick={() => {
                                option.onClick();
                                setOpen(false);
                            }}
                            className={`block w-full px-4 py-2 text-left text-sm ${
                                option.variant === "danger"
                                    ? "text-red-600 hover:bg-red-50"
                                    : "text-text hover:bg-slate-100"
                            }`}
                        >
                            {option.label}
                        </button>
                    ))}
                </div>
            )}
        </div>
    );
};