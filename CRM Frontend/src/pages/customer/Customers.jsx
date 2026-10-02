import Button from "../../components/common/Button"

export const Customers = () => {
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
            </main>
        </div>
    )
}