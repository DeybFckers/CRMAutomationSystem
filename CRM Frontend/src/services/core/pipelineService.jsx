import api from "../api"

export const getAllPipelines = async () => {
    const response = await api.get("/api/pipelines")

    return response.data.data;
}