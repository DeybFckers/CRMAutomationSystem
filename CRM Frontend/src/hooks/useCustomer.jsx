import { useCallback, useEffect, useState } from "react";
import { getAllCustomer } from "../services/core/customerService";

export const useCustomers = () => {
    const [customers, setCustomers] = useState([]);
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
      assignedUserId: "",
      sortBy: "createdat",
      sortDirection: "desc"
    });

    const [debouncedSearch, setDebouncedSearch] = useState("");

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    
    useEffect(() => {
        const handler = setTimeout(() => {
            setDebouncedSearch(filters.search);
        }, 400);

        return () => {
            clearTimeout(handler);
        };
    }, [filters.search]);

    const fetchCustomers = useCallback(async () =>{
        try{
          setLoading(true);
          setError("");

          const response = await getAllCustomer({
            page: pagination.page,
            pageSize: pagination.pageSize,
            search: debouncedSearch || undefined,
            assignedUserId: filters.assignedUserId || undefined,
            sortBy: filters.sortBy,
            sortDirection: filters.sortDirection
          });

          setCustomers(response.items);
          setPagination(response.pagination)
        }catch(error){
          setError("Failed to fetch customers.")
        }finally{
          setLoading(false);
        }
    },[
      pagination.page,
      pagination.pageSize,
      debouncedSearch,
      filters.assignedUserId,
      filters.sortBy,
      filters.sortDirection
    ]);

    useEffect(() => {
      fetchCustomers();
    },[fetchCustomers]);

    const refetch = () =>{
      fetchCustomers();
    }

  return{
    customers,
    filters,
    setFilters,
    pagination,
    setPagination,
    refetch,
    loading,
    error
  }
}