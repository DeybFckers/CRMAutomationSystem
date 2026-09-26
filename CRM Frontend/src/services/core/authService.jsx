import api from "../api";

export const login = async (email, password) => {
    const response = await api.post("/api/auth/login", { email, password });
    return response.data;
}

export const getCurrentUser = async () => {
    const response = await api.get("/api/auth/me");

    return response.data;
};

export const refreshToken = async () => {
    const response = await api.post("/api/auth/refresh");

    return response.data;
};

export const logout = async () => {
    const response = await api.post("/api/auth/logout");

    return response.data;
};