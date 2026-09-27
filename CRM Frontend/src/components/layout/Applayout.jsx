import { Sidebar } from "./Sidebar";
import { Outlet } from "react-router-dom";

export const Applayout = () =>{
    return(
        <div className="min-h-screen bg-background flex">

            <Sidebar />

            <main className="flex-1">
                <Outlet />
            </main>

        </div>
    )
}