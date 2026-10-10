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

export const updateCustomerStatus = async (customerId, customerStatus) => {
    const response = await api.patch(`/api/customer/${customerId}`, customerStatus);
    return response.data;
}

export const deleteCustomer = async (customerId) => {
    const response = await api.delete(`/api/customer/${customerId}`);
    return response.data;
}

export const addCustomerContact = async (customerId, contactData) => {
    const response = await api.post(`/api/customers/${customerId}/contact`, contactData);
    return response.data;
}

export const updateCustomerContact = async (customerId, contactId, contactData) => {
    const response = await api.put(`/api/customers/${customerId}/contact/${contactId}`, contactData);
    return response.data;
}

export const deleteCustomerContact = async (customerId, contactId) => {
    const response = await api.delete(`/api/customers/${customerId}/contact/${contactId}`);
    return response.data;
}
