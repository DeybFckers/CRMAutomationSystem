export const Modal = ({
    isOpen,
    onClose,
    title,
    children,
    className = "",
    contentClassName = "",
    height,
    maxHeight = "85vh",
}) => {
    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
            <div
                style={{
                    height: height ?? "auto",
                    maxHeight,
                }}
                className={`flex w-full flex-col overflow-hidden rounded-xl border border-border bg-surface shadow-2xl ${className}`}
            >
                {/* Fixed Header */}
                <div className="flex shrink-0 items-center justify-between border-b border-border px-6 py-3">
                    <h2 className="text-xl font-semibold text-text">
                        {title}
                    </h2>

                    <button
                        type="button"
                        onClick={onClose}
                        aria-label="Close modal"
                        className="flex h-8 w-8 shrink-0 cursor-pointer items-center justify-center rounded-full bg-red-100 text-text-muted transition hover:bg-red-400 hover:text-text"
                    >
                        <span className="text-xl leading-none">×</span>
                    </button>
                </div>

                {/* Scrollable Content */}
                <div
                    className={`min-h-0 flex-1 overflow-y-auto px-6 py-4 ${contentClassName}`}
                >
                    {children}
                </div>
            </div>
        </div>
    );
};