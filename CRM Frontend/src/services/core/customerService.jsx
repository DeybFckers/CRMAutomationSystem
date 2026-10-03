import api from "../api"

export const getAllCustomer = async (params) => {
    const response = await api.get("/api/customer", {params});

    return response.data.data;
}

export const createCustomer = async (customerData) => {
    const response = await api.post("/api/customer", customerData);
    return response.data;
}

export const updateCustomer = async (customerId, customerData) => {
    const response = await api.put(`/api/customer/${customerId}`, customerData);
    return response.data;
}

export const deleteCustomer = async (customerId) => {
    const response = await api.delete(`/api/customer/${customerId}`);
    return response.data;
}
