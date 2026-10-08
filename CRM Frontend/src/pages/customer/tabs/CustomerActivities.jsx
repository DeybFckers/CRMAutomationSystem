import { Button } from "../../../components/common/Button";

export const CustomerActivities = ({ customer }) => {

    return (
        <div className="flex flex-col gap-4">

            <div className="flex justify-between items-center">

                <h3 className="text-lg font-semibold text-text">
                    Activity
                </h3>

                <Button type="button">
                    + Add Activity
                </Button>

            </div>


            <div className="border border-border rounded-lg p-6 text-center">

                <p className="text-text-secondary">
                    No Activity available.
                </p>

            </div>

        </div>
    );
};