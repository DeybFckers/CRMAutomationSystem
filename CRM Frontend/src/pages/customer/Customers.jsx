import Button from "../../components/common/Button"
import { Dropdown } from "../../components/common/Dropdown"
import { CustomerColumns } from "./CustomerColumns"
import { useCustomers } from "../../hooks/useCustomer"
import { DataTable } from "../../components/common/DataTable"

export const Customers = () => {
    
    const { customer, loading } = useCustomers();

    const columns = CustomerColumns({
        onView: () => {},
        onEdit: () => {},
        onDelete: () => {},
        onAssign: () => {}
    })

    return (
        <div className="min-h-screen bg-background flex p-6">
            <main className="bg-surface border border-border w-full rounded-xl shadow-md p-8">
                <div className="flex justify-between mb-5">
                    
                    <p className="text-2xl font-bold text-text">
                        Customer
                    </p>

                    <Button>
                        + Add Customer
                    </Button>
                    
                </div>

                <div className="flex justify-end items-center gap-3 mb-5">
                    <input type="text"
                    placeholder="Search Customers..."
                    className="border border-border rounded-3xl p-4 h-10 w-xs placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary" />

                    <div className="flex gap-3">
                        <Dropdown label="Status" className="w-40">

                        </Dropdown>

                        <Dropdown label="Assigned" className="w-40">

                        </Dropdown>
                    </div>
                </div>
                {loading ? (
                    <div className="py-10 text-center text-text-secondary">
                        Loading Customer...
                    </div>
                ) :(
                <DataTable
                    data={customer}
                    columns={columns}
                />
                )}
            </main>
        </div>
    )
}