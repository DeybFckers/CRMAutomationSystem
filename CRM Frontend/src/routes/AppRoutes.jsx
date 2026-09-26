import { Routes, Route, Navigate } from "react-router-dom";
import { Login } from "../pages/auth/Login";
import { Dashboard } from "../pages/dashboard/Dashboard";
import { ProtectedRoute } from "../components/auth/ProtectedRoute";

export const AppRoutes = () => {
    return (
        <Routes>

            {/* =========================
                Public Routes
            ========================= */}

            <Route
                path="/login"
                element={<Login />}
            />

            {/* =========================
                Protected Routes
            ========================= */}

            <Route element={<ProtectedRoute />}>

                <Route
                    path="/dashboard"
                    element={<Dashboard />}
                />

            </Route>

            {/* =========================
                Unknown Route
            ========================= */}

            <Route
                path="*"
                element={<Navigate to="/dashboard" replace />}
            />

        </Routes>
    );
};