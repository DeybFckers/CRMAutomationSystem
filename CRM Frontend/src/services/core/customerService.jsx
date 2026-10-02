import api from "../api"

export const getAllCustomer = async (params) => {
    const response = await api.get("/api/customer", {params});

    return response.data.data;
}