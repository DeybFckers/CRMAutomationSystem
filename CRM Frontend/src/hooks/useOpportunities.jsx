import { useCallback, useEffect, useState } from "react";
import { getOpportunitiesByCustomerId } from "../services/core/opportunityService";

export const useOpportunities = (customerId) => {
    const [opportunities, setOpportunities] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const fetchOpportunities = useCallback(async () => {
        if (!customerId) {
            setOpportunities([]);
            setError("");
            return;
        }

        setLoading(true);
        setError("");

        try {
            const response = await getOpportunitiesByCustomerId(customerId);

            setOpportunities(Array.isArray(response) ? response : []);
        } catch (error) {
            console.error("Failed to fetch customer opportunities:", error);

            setError(
                error.response?.data?.Message ||
                error.response?.data?.message ||
                "Failed to load opportunities."
            );
        } finally {
            setLoading(false);
        }
    }, [customerId]);

    useEffect(() => {
        fetchOpportunities();
    }, [fetchOpportunities]);

    return {
        opportunities,
        loading,
        error,
        refetch: fetchOpportunities
    };
};
