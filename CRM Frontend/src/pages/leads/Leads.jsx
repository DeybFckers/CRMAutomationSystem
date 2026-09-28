import { useState } from "react";
import Button from "../../components/common/Button"
import { Dropdown } from "../../components/common/Dropdown";
import { LeadColumns } from "./LeadColumns";
import { DataTable } from "../../components/common/DataTable";
import { Modal } from "../../components/common/Modal";
import { useLeads } from "../../hooks/useLeads";

export const Leads = () => {

    const {
        leads,
        leadStatuses,
        leadSources,
        loading,
        error
    } = useLeads();

    const [isModalOpen, setIsModalOpen] = useState(false);

    const [selectedStatuses, setSelectedStatuses] = useState("");
    const [selectedSources, setSelectedSources] = useState("");

    const [selectedStatusesModal, setSelectedStatusesModal] = useState("");
    const [selectedSourcesModal, setSelectedSourcesModal] = useState("");

    const inputClass = "h-8 p-2 border border-border rounded-md text-text placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary";

    return (
        <div className="min-h-screen bg-background flex p-6">

            <main className="bg-surface border border-border w-full rounded-xl shadow-md p-8">
                <div className="flex justify-between mb-5">
                    <p className="text-2xl font-bold text-text">
                    Leads
                    </p>

                    <Button onClick={() => setIsModalOpen(true)}>
                        + Add Lead
                    </Button>
                    
                </div>

                <div className="flex justify-end items-center gap-3 mb-5">
                    <input type="text"
                    placeholder="Search leads..."
                    className="border border-border rounded-3xl p-4 h-10 w-xs placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary" 
                    />

                    <div className="flex gap-3">

                        <Dropdown label="Status" value={selectedStatuses} onChange={setSelectedStatuses}
                            className="w-40" 
                            options={leadStatuses.map((status) => ({
                                value: status.id,
                                label: status.name
                            }))} 
                        />

                        <Dropdown label="Source" value={selectedSources} onChange={setSelectedSources}
                            className="w-40" 
                            options={leadSources.map((sources) =>({
                                value: sources.id,
                                label: sources.name
                            }))}
                        />

                        <Dropdown label="Assigned To" className="w-40" />

                    </div>
                </div>

                <DataTable
                 data={leads}
                 columns={LeadColumns}
                 />

                 <Modal
                    isOpen={isModalOpen}
                    onClose={() => setIsModalOpen(false)}
                    title="Add Lead"
                >
                    <form action="" className="flex flex-col ">
                        
                        <div className="flex justify-between gap-3 mb-2">
                            <div className="flex flex-col">
                                
                                <label className="mb-2 text-base font-medium text-text">First Name</label>
                                
                                <input type="text" 
                                placeholder="First Name" 
                                className={inputClass} />

                            </div>

                            <div className="flex flex-col">
                                
                                <label className="mb-2 text-base font-medium text-text">Last Name</label>

                                <input type="text" 
                                placeholder="Last Name" 
                                className={inputClass} />
                                
                            </div>
                            
                        </div>

                        <div className="flex justify-between mb-2">
                            
                            <div className="flex flex-col">
                                
                                <label className="mb-2 text-base font-medium text-text">Email</label>
                                
                                <input type="text" 
                                placeholder="john@example.com" 
                                className={inputClass} />

                            </div>

                            <div className="flex flex-col">
                                
                                <label className="mb-2 text-base font-medium text-text">Password</label>

                                <input type="text" 
                                placeholder="+63 9XX-XXXX-XXX" 
                                className={inputClass}/>
                                
                            </div>
                        </div>

                        <div className="flex justify-between mb-2">

                            <div className="flex flex-col">

                                <label className="mb-2 text-base font-medium text-text">Company Name</label>
                                
                                <input type="text" 
                                placeholder="Company" 
                                className={inputClass} />

                            </div>

                            <div className="flex flex-col">

                                <label className="mb-2 text-base font-medium text-text">Estimated Value</label>
                                
                                <input type="text" 
                                placeholder="₱" 
                                className={inputClass} />

                            </div>
                            
                        </div>

                        <div className="flex justify-between mb-2">

                            <div className="flex flex-col">

                                <label className="mb-2 text-base font-medium text-text">Status</label>
                                
                                <Dropdown label="Status" className="w-50" value={selectedStatusesModal} onChange={setSelectedStatusesModal}
                                options={leadStatuses.map((status) => ({
                                    value: status.id,
                                    label: status.name
                                }))} 
                                    
                                />

                            </div>

                            <div className="flex flex-col">

                                <label className="mb-2 text-base font-medium text-text">Source</label>
                                
                                <Dropdown label="Source" className="w-50" value={selectedSourcesModal} onChange={setSelectedSourcesModal}
                                options={leadSources.map((sources) =>({
                                    value: sources.id,
                                    label: sources.name
                                }))}
                                />

                            </div>

                        </div>

                        <div className="flex flex-col mb-2">

                                <label className="mb-2 text-base font-medium text-text">Assigned Sales Representative</label>
                                
                                <Dropdown label="Sales Representative" className="w-full"/>

                            </div>

                        <div className="flex flex-col mb-4">

                            <label className="mb-2 text-base font-medium text-text">Notes</label>
                                
                            <textarea
                                placeholder="Add notes about this lead..."
                                rows={4}
                                className="w-full resize-none border border-border rounded-md px-3 py-2 text-text placeholder:text-text-muted bg-slate-100 focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary"
                            />
                        
                        </div>

                        <div className="flex justify-end gap-2">

                            <Button variant="outline" onClick={() => setIsModalOpen(false)}>
                                Cancel
                            </Button>

                            <Button variant="primary">
                                Create
                            </Button>
                        </div>
                        
                    </form>


                </Modal>
                
            </main>
                
           
        </div>
    );
};