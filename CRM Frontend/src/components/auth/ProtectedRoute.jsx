import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

export const ProtectedRoute = () =>{
    const {user , loading} = useAuth();

    //wait until authcontext finishes checking
    //whether the user is already authenticated
    if(loading){
        return (
            <div className="min-h-screen flex items-center justify-center bg-background">
                <div className="w-8 h-8 border-4 border-border border-t-primary rounded-full animate-spin"></div>
            </div>
        );
    }

    // If there is no authenticated user,
    // redirect them to the login page.
    if (!user) {
        return <Navigate to="/login" replace />;
    }

    // User is authenticated.
    // Render the child route.
    return <Outlet />;
}