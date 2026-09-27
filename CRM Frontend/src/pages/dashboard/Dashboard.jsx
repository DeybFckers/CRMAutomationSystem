import { useAuth } from "../../context/AuthContext";

export const Dashboard = () => {

    const { user } = useAuth();

    return (
        <div className="min-h-screen bg-background flex">

            <main className="flex-1 p-8">

                <h2 className="text-2xl font-bold text-text">
                    Welcome, {user?.firstName}
                </h2>

                <p className="mt-1 text-sm text-text-secondary">
                    Here's what's happening in your CRM today.
                </p>

            </main>

        </div>
    );
};