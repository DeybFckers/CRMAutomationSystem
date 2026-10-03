import { Button }from "../../components/common/Button"
import { Dropdown } from "../../components/common/Dropdown"
import { CustomerColumns } from "./CustomerColumns"
import { useCustomers } from "../../hooks/useCustomer"
import { DataTable } from "../../components/common/DataTable"
import { Pagination } from "../../components/common/Pagination"
import { useState } from "react"
import { Modal } from "../../components/common/Modal"
import { updateCustomer, createCustomer } from "../../services/core/customerService"
import { useUsers } from "../../hooks/useUsers"
import { Pencil } from "lucide-react";

export const Customers = () => {
    
    //hooks
    const { customers, filters, setFilters,pagination, setPagination,refetch, loading, error  } = useCustomers();

    const {users} = useUsers();

    //modal / selection state
    const [selectedCustomer, setSelectedCustomer] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isEditMode, setIsEditMode] = useState(false);
    const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);

    //form field
    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        assignedUserId: "",
        companyName: "",
        email: "",
        phone: "",
    })

    const [submitError, setSubmitError] = useState("");

    //form helpers
    const resetForm = () => {
        //from formdata
        setFormData({
            firstName: "",
            lastName: "",
            companyName: "",
            email: "",
            phone: "",
            assignedUserId: "",
        })
    }

    const closeCustomerModal = () => {
        setIsModalOpen(false);// close
        setIsEditMode(false);// close
        setSelectedCustomer(null);// remove the selected cusomter
        setSubmitError("");// no error
        resetForm();// reset the form
    }

    //form input or controller
    const handleChange = (e) => {
            //field name
        const {name, value} = e.target;
                    //value of the field
        setFormData((prev) => ({
            ...prev,
            [name]: value
        //firstName: "Value"; and so on
        }))

    }

    //create/update user
    const handleSubmit = async (e) => {
        e.preventDefault();
        
        setSubmitError("");
        
        try{
            const customerData = {
                assignedUserId: formData.assignedUserId,
                firstName: formData.firstName,
                lastName: formData.lastName,
                companyName: formData.companyName,
                email: formData.email,
                phone: formData.phone
            }

            if(isEditMode){
                await updateCustomer(selectedCustomer.id, customerData);
            }else{
                await createCustomer(customerData);
            }

            await refetch();

            setIsModalOpen(false);
            setIsEditMode(false);
            setSelectedCustomer(null);
            resetForm();
        }catch(error){
            setSubmitError(
                isEditMode
                    ? "Failed to update Customer"
                    : "Failed to create Customer"
            );
        }
    };

    const columns = CustomerColumns({
        onViewDetails: (customer) => {
            setSelectedCustomer(customer);
            setIsDetailsModalOpen(true);
        }
    })

    const inputClass = "h-10 p-2 w-xs border border-border rounded-md text-text placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary";

    return (
        <div className="min-h-screen bg-background flex p-6">
            <main className="bg-surface border border-border w-full rounded-xl shadow-md p-8">
                <div className="flex justify-between mb-5">
                    
                    <p className="text-2xl font-bold text-text">
                        Customer
                    </p>
c
                    <Button onClick={() => {setIsModalOpen(true)}}>
                        + Add Customer
                    </Button>
                    
                </div>

                <div className="flex justify-end items-center gap-3 mb-5">
                    <input type="text"
                    placeholder="Search Customers..."
                    className="border border-border rounded-3xl p-4 h-10 w-xs placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary" />

                    <Dropdown label="Status" className="w-40">

                    </Dropdown>

                    <Dropdown label="Assigned" className="w-40">

                    </Dropdown>


                </div>
                {loading ? (
                    <div className="py-10 text-center text-text-secondary">
                        Loading Customer...
                    </div>
                ) :(
                <DataTable
                    data={customers}
                    columns={columns}
                />
                )}

                <Pagination 
                    pagination={pagination}
                    onPageChange={(page) => {
                        setPagination(prev =>({
                            ...prev,
                            page
                        }));
                    }}
                />

                <Modal
                    isOpen={isModalOpen} //open the modal it means all the use state is true
                    onClose={() => {closeCustomerModal()}} // close the modal it means all the use state is false
                    title={isEditMode ? "Edit Customer" : "Add Customer"}
                    >
                        <form onSubmit={handleSubmit} className="flex flex-col">
                            {/* Customer Name */}
                            <div className="flex justify-between gap-3 mb-4">
                                <div className="flex flex-col">
                                    <label className="mb-2 text-base font-medium text-text">First Name</label>
                                
                                    <input type="text"
                                    name="firstName" //this is the form field name
                                    placeholder="First Name" 
                                    value={formData.firstName}// value of the form where it should be
                                    onChange={handleChange}
                                    className={inputClass} />
                                </div>

                                <div className="flex flex-col">
                                    <label className="mb-2 text-base font-medium text-text">Last Name</label>
                                
                                    <input type="text"
                                    name="lastName" 
                                    placeholder="Last Name" 
                                    value={formData.lastName}
                                    onChange={handleChange}
                                    className={inputClass} />
                                </div>
                            </div>
                            {/* Customer Company and Email */}
                            <div className="flex justify-between mb-4">
                                <div className="flex flex-col">
                                    <label className="mb-2 text-base font-medium text-text">Company Name</label>
                                
                                    <input type="text"
                                    name="companyName" 
                                    placeholder="Company" 
                                    value={formData.companyName}
                                    onChange={handleChange}
                                    className={inputClass} />
                                </div>

                                <div className="flex flex-col">
                                    <label className="mb-2 text-base font-medium text-text">Email</label>
                                
                                    <input type="email"
                                    name="email" 
                                    placeholder="john@example.com" 
                                    value={formData.email}
                                    onChange={handleChange}
                                    className={inputClass} />

                                </div>
                            </div>
                            {/* Sales Representative and Phone */}
                            <div className="flex justify-between mb-4">

                                <div className="flex flex-col">

                                    <label className="mb-2 text-base font-medium text-text">Phone</label>
                                
                                    <input type="text"
                                    name="phone" 
                                    placeholder="09XX-XXXX-XXX" 
                                    value={formData.phone}
                                    onChange={handleChange}
                                    className={inputClass} />
                                    

                                </div>

                                <div className="flex flex-col">

                                    <label className="mb-2 text-base font-medium text-text">Sales Representative</label>
                                
                                    <Dropdown label="Sales Representative" className="w-80"
                                    value={formData.assignedUserId}
                                        onChange={(value) =>
                                            setFormData((prev) => ({
                                                ...prev,
                                                assignedUserId: value
                                            }))
                                        }
                                        options={users.map((user) => ({
                                            value: user.id,
                                            label: `${user.firstName} ${user.lastName}`
                                        }))}
                                    />

                                </div>

                            </div>

                            <div className="flex justify-end gap-2">

                            <Button variant="outline"
                                type="button"
                                onClick={() => {closeCustomerModal()}}
                            >
                                Cancel
                            </Button>

                            <Button variant="primary" type="submit">
                                {isEditMode ? "Update Customer" : "Add Customer"}
                            </Button>
                        </div>

                    </form>
                </Modal>
                {/* Details Modal */}
                <Modal
                    isOpen={isDetailsModalOpen}
                    onClose={() => {
                        setIsDetailsModalOpen(false);
                        setSelectedCustomer(null);
                    }}
                    title={`Customer Details`}
                >
                    {selectedCustomer && (
                        <div className="flex justify-start gap-8 w-auto ">

                            {/* Customer Information */}
                            <div className="flex flex-col">

                                <div className="flex justify-between">
                                    <h3 className="text-text font-semibold text-lg">
                                    Customer Information
                                    </h3>
                                    <button
                                        type="button"
                                        className="text-text-secondary hover:text-text transition-colors"
                                    >
                                        <Pencil size={18} />
                                    </button>
                                    
                                </div>

                                <div className="grid grid-cols-2 gap-x-8 gap-y-2">
            
                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Name
                                        </p>

                                        <p className="text-text wrap-break-words">
                                            {selectedCustomer.firstName} {selectedCustomer.lastName}
                                        </p>
                                    </div>

                                    <div>

                                        <p className="text-sm text-text-secondary">
                                            Customer Code
                                        </p>

                                        <p className="text-text">
                                            {selectedCustomer.customerCode || "-"}
                                        </p>
                                    </div>

                                    <div>

                                        <p className="text-sm text-text-secondary">
                                            Company Name
                                        </p>

                                        <p className="text-text wrap-break-words">
                                            {selectedCustomer.companyName || "-"}
                                        </p>

                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Sales Representative
                                        </p>

                                        <p className="text-text wrap-break-words">
                                            {`${selectedCustomer.assignedUser.firstName} ${selectedCustomer.assignedUser.lastName}`  || "-"}
                                        </p>
                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Email
                                        </p>

                                        <p className="text-text wrap-break-words">
                                            {selectedCustomer.email}
                                        </p>
                                    </div>

                                    <div>

                                        <p className="text-sm text-text-secondary">
                                            Phone
                                        </p>

                                        <p className="text-text">
                                            {selectedCustomer.phone || "-"}
                                        </p>
                                    </div>

                                </div>
                                
                            </div>
                            {/* Contact Information */}
                            <div className="">
                                <h3 className="text-text font-semibold text-lg">
                                    Contacts 
                                </h3>
                            </div>
                            {/* Address */}
                            <div className="">
                                <h3 className="text-text font-semibold text-lg">
                                    Address
                                </h3>
                            </div>



                            
                            {/* <div>
                                <div className="flex justify-between mb-2 items-center">

                                    <h3 className="text-text font-semibold text-lg">
                                        Customer Information
                                    </h3>

                                  
                                </div>

                                <div className="grid grid-cols-2 gap-x-8 gap-y-2">
            
                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Name
                                        </p>

                                        <p className="text-text wrap-break-words">
                                            {selectedCustomer.firstName} {selectedCustomer.lastName}
                                        </p>
                                    </div>

                                    <div>

                                        <p className="text-sm text-text-secondary">
                                            Customer Code
                                        </p>

                                        <p className="text-text">
                                            {selectedCustomer.customerCode || "-"}
                                        </p>
                                    </div>

                                    <div>

                                        <p className="text-sm text-text-secondary">
                                            Company Name
                                        </p>

                                        <p className="text-text wrap-break-words">
                                            {selectedCustomer.companyName || "-"}
                                        </p>

                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Sales Representative
                                        </p>

                                        <p className="text-text wrap-break-words">
                                            {`${selectedCustomer.assignedUser.firstName} ${selectedCustomer.assignedUser.lastName}`  || "-"}
                                        </p>
                                    </div>

                                    <div>
                                        <p className="text-sm text-text-secondary">
                                            Email
                                        </p>

                                        <p className="text-text wrap-break-words">
                                            {selectedCustomer.email}
                                        </p>
                                    </div>

                                    <div>

                                        <p className="text-sm text-text-secondary">
                                            Phone
                                        </p>

                                        <p className="text-text">
                                            {selectedCustomer.phone || "-"}
                                        </p>
                                    </div>

                                </div>
                            </div> */}
                            
                            
                        </div>
                    )}
                </Modal>
            </main>
        </div>
    )
}

{/* <span className="h-fit rounded-full bg-green-100 px-3 py-1 text-sm font-medium text-green-700">
    {selectedCustomer.status}
</span> */}
