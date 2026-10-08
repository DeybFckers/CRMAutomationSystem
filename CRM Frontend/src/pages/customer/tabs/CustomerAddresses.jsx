import { Button } from "../../../components/common/Button";

export const CustomerAddresses = ({ customer }) => {

    return (
        <div className="flex flex-col gap-4">

            <div className="flex justify-between items-center">

                <h3 className="text-lg font-semibold text-text">
                    Addresses
                </h3>

                <Button type="button">
                    + Add Address
                </Button>

            </div>


            {customer.addresses?.length > 0 ? (

                <div className="flex flex-col gap-3">

                    {customer.addresses.map((address) => (

                        <div
                            key={address.id}
                            className="border border-border rounded-lg p-4"
                        >

                            <div className="flex justify-between items-start">

                                <p className="font-medium text-text">
                                    {address.type || "Address"}
                                </p>


                                {address.isPrimary && (

                                    <span className="rounded-full bg-green-100 px-3 py-1 text-xs font-medium text-green-700">
                                        Primary
                                    </span>

                                )}

                            </div>


                            <div className="mt-3">

                                <p className="text-text font-medium">
                                    {address.addressLine || "-"}
                                </p>

                                <p className="text-sm text-text-secondary">
                                    {address.city || "-"},{" "}
                                    {address.province || "-"}{" "}
                                    {address.postalCode || ""}
                                </p>

                                <p className="text-sm text-text-secondary">
                                    {address.country || "-"}
                                </p>

                            </div>

                            <div className="flex justify-end gap-2">
                                <Button
                                variant="outline"
                                type="button"
                                
                                >
                                    Edit
                                </Button>

                                <Button
                                variant="danger"
                                type="button"
                                
                                >
                                    Delete
                                </Button>
                            </div>

                        </div>

                    ))}

                </div>

            ) : (

                <div className="border border-border rounded-lg p-6 text-center">

                    <p className="text-text-secondary">
                        No addresses available.
                    </p>

                </div>

            )}

        </div>
    );
};