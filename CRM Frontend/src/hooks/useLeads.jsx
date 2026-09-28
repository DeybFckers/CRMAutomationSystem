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

    useEffect(() => {
        const fetchData = async () =>{
            setLoading(true);
            setError("");

            try{
                const [
                    leadsData,
                    statusesData,
                    sourcesData
                ] = await Promise.all([
                    getAllLeads(),
                    getAllLeadStatuses(),
                    getAllLeadSources()
                ]);

                setLeads(leadsData);
                setLeadStatuses(statusesData);
                setLeadSources(sourcesData);
            }catch(error){
                setError("Failed to load Leads.")
            }finally {
                setLoading(false);
            }
        };

        fetchData();

    }, []);

    return{
        leads,
        leadStatuses,
        leadSources,
        loading,
        error
    };
};