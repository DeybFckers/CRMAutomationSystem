import Button from "../../components/common/Button"

export const Leads = () => {

    return (
        <div className="min-h-screen bg-background flex p-6">

            <main className="bg-surface border border-border w-full rounded-xl shadow-md p-8">
                <div className="flex justify-between">
                    <p className="text-2xl font-bold text-text">
                    Leads
                    </p>

                    <Button>
                        + Add Lead
                    </Button>
                    
                </div>
            </main>
                
           
        </div>
    );
};