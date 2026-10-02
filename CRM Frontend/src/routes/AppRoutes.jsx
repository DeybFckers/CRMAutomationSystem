import { Routes, Route, Navigate } from "react-router-dom";

import { Login } from "../pages/auth/Login";
import { Dashboard } from "../pages/dashboard/Dashboard";

import { ProtectedRoute } from "../components/auth/ProtectedRoute";
import { Applayout } from "../components/layout/Applayout";
import { Leads } from "../pages/leads/Leads";
import { Customers } from "../pages/customer/Customers";

export const AppRoutes = () => {
    return (
        <Routes>

            <Route
                path="/login"
                element={<Login />}
            />

            <Route element={<ProtectedRoute />}>

                {/* Shared Application Layout */}
                <Route element={<Applayout />}>

                    <Route
                        path="/dashboard"
                        element={<Dashboard />}
                    />

                    <Route 
                        path="/leads" 
                        element={<Leads/>}
                    />

                    <Route 
                        path="/customers"
                        element={<Customers/>}
                    />

                </Route>

            </Route>

            <Route
                path="*"
                element={<Navigate to="/dashboard" replace />}
            />

        </Routes>
    );
};