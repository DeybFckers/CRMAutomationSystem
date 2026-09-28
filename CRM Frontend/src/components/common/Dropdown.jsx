import { useState } from "react";
import { ChevronDown } from "lucide-react";

export const Dropdown = ({ label, options = [],
    value,
    onChange,
    className }) => {

    // Controls whether the dropdown is open or closed.
    //
    // false = closed
    // true = open
    const [open, setOpen] = useState(false);

    const selectedOption = options.find(
        (option) => option.value === value
    );


    return (
        <div className={`relative ${className}`}>

            {/* Dropdown button */}
            <button
                type="button"
                onClick={() => setOpen(!open)}
                className="flex items-center justify-between w-full h-10 px-3 rounded-xl border border-border bg-surface text-sm font-medium text-text cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary"
            >
                <span>
                    {selectedOption?.label || label}
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
                                onChange?.(option.value);
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