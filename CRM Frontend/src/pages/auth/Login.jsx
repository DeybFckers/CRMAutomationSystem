import AuthImage from "../../assets/AuthImage.jpg";
import { useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";

export const Login = () => {

    const { login } = useAuth();
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");   

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();

        setError("");
        setLoading(true);

        try{
            await login(email, password);
            navigate("/dashboard", { replace: true });
        }catch(error){
            setError(
                error.response?.data?.message ||
                "Invalid email or password."
            );
        }finally{
            setLoading(false);
        }
    } 


    return (
        <div className="min-h-screen bg-background flex items-center justify-center">
            <div className="bg-surface rounded-lg shadow-xl flex w-auto">
                <img src={AuthImage} alt="" srcSet="" className=" h-180 w-auto rounded-tl-lg rounded-bl-lg" />
                
                <form onSubmit={handleSubmit} className="flex flex-col p-14 w-120 mx-auto justify-center">
                    <p className="text-2xl font-bold text-text">Customer Relationship Management</p>
                    <p className="mt-1 text-text-muted">Sign in to continue your account.</p>
                    
                    {error && (
                        <div className="mt-4 p-3 rounded-md bg-red-50 border border-red-200 text-red-600">
                            {error}
                        </div>
                    )}

                    <div className="mt-4">
                        <label className="block mb-2 text-base font-medium text-text">Email address</label>
                        <input 
                        type="username" 
                        placeholder="john@example.com"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                        className="border border-border h-10 w-full rounded-md p-4 placeholder:text-text-muted focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary " />
                    </div>
                    <div className="mt-2">
                        <label className="block mb-2 text-base font-medium text-text">Password</label>
                        <input 
                        type="password" 
                        placeholder="●●●●●●●●"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                        className="border border-border h-10  w-full rounded-md p-4 placeholder:text-text-muted focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary " />
                    </div>
                    <div className="mt-4">
                        <button type="submit" disabled={loading} className="w-full bg-primary text-white py-2 px-4 rounded-md hover:bg-primary-dark focus:outline-none focus:ring-2 focus:ring-primary focus:border-primary">
                            {loading ? "Signing in..." : "Sign in"}
                        </button>
                    </div>
                </form>
            </div>
        </div>
        
    )
}