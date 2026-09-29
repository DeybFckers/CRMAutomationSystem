import { useEffect, useState } from "react";
import { getAllUser } from "../services/core/userService"; 

export const useUsers = () => {
    const [users, setUsers] = useState ([]);

    const [loading, setLoading] = useState(false);

    const [error, setError] = useState("");

    useEffect(() => {
        const fetchUsers = async () =>{
            setLoading(true);
            setError("");

            try{
                const usersData = await getAllUser()

                setUsers(usersData);
            }catch(error){
                setError("Failed to load Users")
            }finally{
                setLoading(false);
            }
        };
        fetchUsers();
    }, []);

    return{
        users,
    }
}