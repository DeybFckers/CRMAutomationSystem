import api from "../api";

export const getAllLeads = async (params) => {
    const response = await api.get("/api/leads", {params});

   return response.data.data;
}

export const getAllLeadStatuses = async () =>{
    const response = await api.get("/api/lead-statuses")

    return response.data.data;
}

export const getAllLeadSources = async () =>{
    const response = await api.get("/api/lead-sources")
    
    return response.data.data;
}

export const createLead = async (leadData) =>{
    const response = await api.post("/api/leads", leadData);

    return response.data;
}

export const updateLead = async (leadId, leadData) =>{
    const response = await api.put(`/api/leads/${leadId}`, leadData);

    return response.data;
}

export const deleteLead = async (leadId) =>{
    const response = await api.delete(`/api/leads/${leadId}`);
    return response.data;
}