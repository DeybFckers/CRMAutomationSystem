import { createContext, useContext, useEffect, useState } from "react";
import {
    login as loginApi,
    logout as logoutApi,
    getCurrentUser,
} from "../services/core/authService";

// AuthContext provides a shared authentication state
// that can be accessed by components inside AuthProvider.
export const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {

    // Stores the currently authenticated user.
    // Initially null because we have not checked authentication yet.
    const [user, setUser] = useState(null);

    // Tracks whether we are still checking the user's authentication state.
    const [loading, setLoading] = useState(true);

    const login = async (email, password) => {
        // Call the backend login API.
        // The backend creates the HttpOnly authentication cookies.
        await loginApi(email, password);

        alert("This is From Auth Context")

        // Get the currently authenticated user's information.
        const currentUser = await getCurrentUser();

        // Store the authenticated user in React state.
        setUser(currentUser);
    };

    const logout = async () => {
        // Call the backend logout endpoint.
        // The backend handles removing the authentication cookies.
        await logoutApi();

        // Clear the authenticated user from React state.
        setUser(null);
    };

    useEffect(() => {
        const loadUser = async () => {
            try {
                // Check if a user is already authenticated.
                // The browser automatically sends the HttpOnly cookie.
                const currentUser = await getCurrentUser();

                // Store the authenticated user in React state.
                setUser(currentUser);
            } catch (error) {
                // If authentication fails, treat the user as unauthenticated.
                setUser(null);
            } finally {
                // Authentication check is finished,
                // regardless of whether it succeeded or failed.
                setLoading(false);
            }
        };

        // Check authentication when AuthProvider mounts.
        loadUser();
    }, []);

    return (
        <AuthContext.Provider
            value={{
                user,
                loading,
                login,
                logout,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
};

// Custom hook for accessing the authentication context.
export const useAuth = () => {
    return useContext(AuthContext);
};