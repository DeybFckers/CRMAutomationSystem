import api from "../api"

export const getAllUser = async () => {
    const response = await api.get("/api/users")

    return response.data.data;
}