import api from "../api"

export const getAllStagesOfPipeline = async (pipelineId) => {
    const response = await api.get(`/api/pipelines/${pipelineId}/stages`);
    return response.data.data;
}