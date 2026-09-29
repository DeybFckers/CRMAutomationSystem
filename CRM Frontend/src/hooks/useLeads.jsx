import { useEffect, useState } from "react";
import {
    getAllLeads,
    getAllLeadStatuses,
    getAllLeadSources
} from "../services/core/leadsService"

export const useLeads = () => {
    const [leads, setLeads] = useState([]);

    const [leadStatuses, setLeadStatuses] = useState([]);

    const [leadSources, setLeadSources] = useState([]);

    const [loading, setLoading] = useState(false);

    const [error, setError] = useState("");

    const fetchLeads = async () =>{
        const data = await getAllLeads();

        setLeads(data);
    }

    const fetchLeadOptions = async() =>{
        const status = await getAllLeadStatuses();
        const source = await getAllLeadSources();

        setLeadStatuses(status);
        setLeadSources(source);
    }
    
    const fetchData = async () =>{
        setLoading(true);
        setError("");

        try{
            await Promise.all([
                fetchLeads(),
                fetchLeadOptions(),
            ]);
        }catch(error){
            setError("Failed to load leads.")
        }finally{
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    return{
        leads,
        leadStatuses,
        leadSources,
        loading,
        error,
        refetch: fetchLeads
    };
};