import { Button } from "../../../components/common/Button";

export const CustomerContacts = ({ customer }) => {

    return (
        <div className="flex flex-col gap-4">

            <div className="flex justify-between items-center">

                <h3 className="text-lg font-semibold text-text">
                    Contacts
                </h3>

                <Button
                variant="primary"
                type="button"

                >
                    + Add Contact
                </Button>

            </div>

            {customer.contacts?.length > 0 ? (

                <div className="flex flex-col gap-3">

                    {customer.contacts.map((contact) => (

                        <div
                            key={contact.id}
                            className="border border-border rounded-lg p-4"
                        >

                            {/* Contact Header */}
                            <div className="flex justify-between items-start">

                                <div>

                                    <p className="font-medium text-text">
                                        {contact.firstName}{" "}
                                        {contact.lastName}
                                    </p>

                                    <p className="text-sm text-text-secondary">
                                        {contact.position || "-"}
                                    </p>

                                </div>


                                {contact.isPrimary && (

                                    <span className="rounded-full bg-green-100 px-3 py-1 text-xs font-medium text-green-700">
                                        Primary
                                    </span>

                                )}

                            </div>


                            {/* Contact Details */}
                            <div className="grid grid-cols-2 gap-4 mt-4">

                                <div>

                                    <p className="text-sm text-text-secondary">
                                        Email
                                    </p>

                                    <p className="text-text wrap-break-words font-medium">
                                        {contact.email || "-"}
                                    </p>

                                </div>


                                <div>

                                    <p className="text-sm text-text-secondary">
                                        Phone
                                    </p>

                                    <p className="text-text font-medium">
                                        {contact.phone || "-"}
                                    </p>

                                </div>

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
                        No contacts available.
                    </p>

                </div>

            )}   

        </div>
    );
};