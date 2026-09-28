import { useState } from "react";
import { ChevronDown } from "lucide-react";

export const Dropdown = ({ label }) => {

    // Controls whether the dropdown is open or closed.
    //
    // false = closed
    // true = open
    const [open, setOpen] = useState(false);

    // Stores the currently selected option.
    const [selected, setSelected] = useState(null);

    // Temporary option.
    // Later, this can come from your API.
    const options = [
        {
            value: "new",
            label: "New"
        }
    ];

    return (
        <div className="relative w-40">

            {/* Dropdown button */}
            <button
                type="button"
                onClick={() => setOpen(!open)}
                className="flex items-center justify-between w-full h-10 px-3 rounded-xl border border-border bg-surface text-sm font-medium text-text cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary"
            >
                <span>
                    {selected?.label || label}
                </span>

                <ChevronDown
                    size={16}
                    className={`text-text-muted transition-transform ${
                        open ? "rotate-180" : ""
                    }`}
                />
            </button>

            {/* Dropdown options */}
            {open && (
                <div className="absolute z-50 mt-2 w-full rounded-xl border border-border bg-surface p-1 shadow-lg">

                    {options.map((option) => (
                        <button
                            key={option.value}
                            type="button"
                            onClick={() => {
                                setSelected(option);
                                setOpen(false);
                            }}
                            className="w-full text-left px-3 py-2 rounded-lg text-sm text-text hover:bg-surface-secondary cursor-pointer"
                        >
                            {option.label}
                        </button>
                    ))}

                </div>
            )}

        </div>
    );
};