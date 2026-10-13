import { useEffect, useState } from "react";
import { getAllStagesOfPipeline } from "../services/core/stageService";

export const useStages = (pipelineId) => {
    const [stages, setStages] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    useEffect(() => {
        if (!pipelineId) {
            setStages([]);
            setError("");
            return;
        }

        const fetchStages = async () => {
            setLoading(true);
            setError("");

            try {
                const stagesData = await getAllStagesOfPipeline(pipelineId);
                setStages(stagesData);
            } catch (error) {
                setStages([]);
                setError("Failed to load Pipeline Stages");
            } finally {
                setLoading(false);
            }
        };

        fetchStages();
    }, [pipelineId]);

    return {
        stages,
        loading,
        error
    };
};