const Button = ({
    children,
    variant = "primary",
    size = "md",
    type = "button",
    disabled = false,
    onClick,
    className = "",
}) => {

    const baseStyles =
        "inline-flex items-center justify-center rounded-lg font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-primary/30 disabled:cursor-not-allowed disabled:opacity-50";

    const variants = {
        primary:
            "bg-primary text-white hover:bg-primary-hover",

        secondary:
            "bg-surface-secondary text-text hover:bg-border",

        outline:
            "border border-border bg-surface text-text hover:bg-surface-secondary",

        danger:
            "bg-danger text-white hover:bg-red-700",

        ghost:
            "text-text-secondary hover:bg-surface-secondary hover:text-text",
    };

    const sizes = {
        sm: "px-3 py-1.5 text-sm",
        md: "px-4 py-2.5 text-sm",
        lg: "px-5 py-3 text-base",
    };

    return (
        <button
            type={type}
            disabled={disabled}
            onClick={onClick}
            className={`${baseStyles} ${variants[variant]} ${sizes[size]} ${className}`}
        >
            {children}
        </button>
    );
};

export default Button;