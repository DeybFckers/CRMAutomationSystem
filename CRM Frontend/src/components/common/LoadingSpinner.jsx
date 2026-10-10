
export const LoadingSpinner = ({ message = "Searching..." }) => {
    return (
        <div
            className="absolute inset-0 z-10 flex items-center justify-center bg-surface/70"
            role="status"
            aria-live="polite"
        >
            <div className="flex flex-col items-center gap-3">
                <div className="h-10 w-10 animate-spin rounded-full border-4 border-border border-t-primary" />

                <p className="text-sm font-medium text-text-secondary">
                    {message}
                </p>
            </div>
        </div>
    );
};
