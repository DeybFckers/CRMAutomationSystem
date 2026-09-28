import api from "../api";

export const getAllLeads = async () => {
    const response = await api.get("/api/leads");

   return response.data.data;
}