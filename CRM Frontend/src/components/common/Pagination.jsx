import { Button } from "./Button";

export const Pagination = ({ pagination, onPageChange }) => {
    const {
        page,
        totalPages,
        hasPreviousPage,
        hasNextPage,
    } = pagination;

    return (
        <div className="flex items-center justify-between mt-4">
            <div className="text-sm text-text-secondary">
                Page {page} of {totalPages}
            </div>

            <div className="flex gap-2">
                <Button
                    variant="outline"
                    disabled={!hasPreviousPage}
                    onClick={() => onPageChange(page - 1)}
                >
                    Previous
                </Button>

                <Button
                    variant="outline"
                    disabled={!hasNextPage}
                    onClick={() => onPageChange(page + 1)}
                >
                    Next
                </Button>
            </div>
        </div>
    );
};
