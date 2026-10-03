import { useCallback, useEffect, useState } from "react";
import {
    getAllLeads,
    getAllLeadStatuses,
    getAllLeadSources
} from "../services/core/leadsService"

export const useLeads = () => {
    const [leads, setLeads] = useState([]);

    const [leadStatuses, setLeadStatuses] = useState([]);

    const [leadSources, setLeadSources] = useState([]);

    const [pagination, setPagination] = useState({
        page: 1,
        pageSize: 15,
        totalCount:0,
        totalPages:0,
        hasPreviousPage: false,
        hasNextPage: false,
    });

    const [filters, setFilters] = useState({
        search: "",
        statusId: "",
        sourceId: "",
        assignedUserId: "",
        sortBy: "createdat",
        sortDirection: "desc"
    });

    const [debouncedSearch, setDebouncedSearch] = useState("");

    const [loading, setLoading] = useState(false);

    const [error, setError] = useState("");

    
    useEffect(() => {
        const handler = setTimeout(() => {
            setDebouncedSearch(filters.search);
        }, 400);

        return () => {
            clearTimeout(handler);
        };
    }, [filters.search]);

    const fetchLeads = useCallback(async () => {
        try{
            setLoading(true);
            setError("");

            const response = await getAllLeads({
                page: pagination.page,
                pageSize: pagination.pageSize,
                search: debouncedSearch || undefined,
                statusId: filters.statusId || undefined,
                sourceId: filters.sourceId || undefined,
                assignedUserId: filters.assignedUserId || undefined,
                sortBy: filters.sortBy,
                sortDirection: filters.sortDirection
            });

            setLeads(response.items);
            setPagination(response.pagination)
        }catch(error){
            setError("Failed to lead leads.")
        }finally{
            setLoading(false);
        }
    }, [
        pagination.page,
        pagination.pageSize,
        debouncedSearch,
        filters.statusId,
        filters.sourceId,
        filters.assignedUserId,
        filters.sortBy,
        filters.sortDirection
    ]);


    useEffect(() => {
        fetchLeads();
    },[fetchLeads]);

    const refetch = () =>{
        fetchLeads();
    };

    useEffect(() => {
        const fetchOptions = async () => {
            try{ 
                const [statuses, sources] = await Promise.all([
                    getAllLeadStatuses(),
                    getAllLeadSources()
                ]);

                setLeadStatuses(statuses);
                setLeadSources(sources);
            }catch(error){
                setError("Failed to load lead Options");
            }
        };

        fetchOptions();
    }, []);
    
    return{
        leads,
        leadStatuses,
        leadSources,
        pagination,
        setPagination,
        filters,
        setFilters,
        loading,
        error,
        refetch
    }
    
};