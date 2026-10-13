import { useEffect, useState } from "react";
import {getAllPipelines} from "../services/core/pipelineService";

export const usePipelines = () => {
    const [pipelines, setPipelines] = useState([]);
    const [loading, setLoading] = useState([]);
    const [error, setError] = useState([]);

    useEffect(() => {
        const fetchPipelines = async () => {
            setLoading(true);
            setError("");

            try{
                const pipelinesData = await getAllPipelines();

                setPipelines(pipelinesData);
            }catch(error){
                setError("Failed to load Pipelines")
            }finally{
                setLoading(false);
            }
        };
        fetchPipelines();
    }, []);

    return{
        pipelines,
    }
}