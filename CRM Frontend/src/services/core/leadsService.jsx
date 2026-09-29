import api from "../api";

export const getAllLeads = async () => {
    const response = await api.get("/api/leads");

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