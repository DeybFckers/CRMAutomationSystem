import { Button } from "../../../components/common/Button";
export const CustomerOpportunities = () => {
  return (
        <div className="flex flex-col gap-4">

            <div className="flex justify-between items-center">

                <h3 className="text-lg font-semibold text-text">
                    Opportunity
                </h3>

                <Button type="button">
                    + Add Opportunity
                </Button>

            </div>


            <div className="border border-border rounded-lg p-6 text-center">

                <p className="text-text-secondary">
                    No Opportunity available.
                </p>

            </div>

        </div>
    );
}
