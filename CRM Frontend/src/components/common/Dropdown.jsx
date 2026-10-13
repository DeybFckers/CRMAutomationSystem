import { useState, useEffect } from "react";
import { ChevronDown } from "lucide-react";

export const Dropdown = ({
    label,
    options = [],
    value,
    onChange,
    className = "",
    disabled = false,
}) => {
    // Controls whether the dropdown is open or closed.
    const [open, setOpen] = useState(false);

    // Close the dropdown if it becomes disabled.
    useEffect(() => {
        if (disabled) {
            setOpen(false);
        }
    }, [disabled]);

    const selectedOption = options.find(
        (option) => option.value === value
    );

    const handleToggle = () => {
        if (disabled) return;
        setOpen((prev) => !prev);
    };

    return (
        <div className={`relative ${className}`}>
            {/* Dropdown button */}
            <button
                type="button"
                disabled={disabled}
                onClick={handleToggle}
                aria-haspopup="listbox"
                aria-expanded={!disabled && open}
                className={`flex items-center justify-between w-full h-10 px-3 rounded-xl border border-border bg-surface text-sm font-medium text-text focus:outline-none focus:ring-2 focus:ring-primary ${
                    disabled
                        ? "cursor-not-allowed opacity-50"
                        : "cursor-pointer"
                }`}
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
            {open && !disabled && (
                <div
                    role="listbox"
                    className="absolute z-50 mt-2 w-full rounded-xl border border-border bg-surface p-1 shadow-lg max-h-48 overflow-y-auto"
                >
                    {options.map((option) => (
                        <button
                            key={option.value}
                            type="button"
                            role="option"
                            aria-selected={option.value === value}
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
