import { Navigate, Outlet } from "react-router-dom"
import { useAuth } from "./state/context"

const DISABLE_AUTH = true;

const PrivateRoute = () => {
	const { token } = useAuth()
	if (DISABLE_AUTH) 
		{return <Outlet />;}
	return token ? <Outlet /> : <Navigate to="/login" />
}

export default PrivateRoute
