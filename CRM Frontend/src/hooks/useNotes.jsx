
import { useCallback, useEffect, useState } from "react";
import { getNotesByCustomerId } from "../services/core/noteService";

export const useNotes = (customerId) => {
    const [notes, setNotes] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const fetchNotes = useCallback(async () => {
        if (!customerId) {
            setNotes([]);
            setError("");
            setLoading(false);
            return;
        }

        setLoading(true);
        setError("");

        try {
            const response = await getNotesByCustomerId(customerId);

            setNotes(Array.isArray(response) ? response : []);
        } catch (error) {
            console.error("Failed to fetch customer notes:", error);

            setError(
                error.response?.data?.message ||
                error.response?.data?.Message ||
                "Failed to load notes."
            );
        } finally {
            setLoading(false);
        }
    }, [customerId]);

    useEffect(() => {
        fetchNotes();
    }, [fetchNotes]);

    return {
        notes,
        loading,
        error,
        refetch: fetchNotes
    };
};
