import { useEffect, useState } from "react";
import { getAllCustomer } from "../services/core/customerService";

export const useCustomers = () => {
    const [customer, setCustomer] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
    const fetchCustomer = async () => {
      try {
        const data = await getAllCustomer();
        setCustomer(data.items);
      } catch (error) {
        console.error("Failed to fetch customers:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchCustomer();
  }, []);

  return{
    customer,
    loading
  }
}