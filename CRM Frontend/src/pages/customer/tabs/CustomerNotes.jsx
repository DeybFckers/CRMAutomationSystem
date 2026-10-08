import { Button } from "../../../components/common/Button";

export const CustomerNotes = ({ customer }) => {

    return (
        <div className="flex flex-col gap-4">

            <div className="flex justify-between items-center">

                <h3 className="text-lg font-semibold text-text">
                    Notes
                </h3>

                <Button type="button">
                    + Add Note
                </Button>

            </div>


            <div className="border border-border rounded-lg p-6 text-center">

                <p className="text-text-secondary">
                    No notes available.
                </p>

            </div>

        </div>
    );
};