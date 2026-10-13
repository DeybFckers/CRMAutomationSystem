import api from "../api";

export const getOpportunitiesByCustomerId = async (customerId) => {
    const response = await api.get(`/api/opportunities/customer/${customerId}`);
    return response.data.data;
};

export const createOpportunity = async (opportunityData) => {
    const response = await api.post("/api/opportunities", opportunityData);
    return response.data.data;
};

export const updateOpportunity = async (opportunityId, opportunityData) => {
    const response = await api.put(`/api/opportunities/${opportunityId}`, opportunityData);
    return response.data.data;
};

export const updateStageOpportunity = async (opportunityId, stageId) => {
    const response = await api.patch(`/api/opportunities/${opportunityId}/stage/${stageId}`);
    return response.data;
};

export const reassigneduserOpportunity = async (opportunityId, assignedUserId) => {
    const response = await api.patch(`/api/opportunities/${opportunityId}/assign/${assignedUserId}`);
    return response.data;
};

export const updateStatusOpportunity = async (opportunityId, status) => {
    const response = await api.patch(
        `/api/opportunities/${opportunityId}/status`,
        null,
        { params: { status } }
    );
    return response.data;
};
export const deleteOpportunity = async (opportunityId) => {
    const response = await api.delete(`/api/opportunities/${opportunityId}`);
    return response.data;
};