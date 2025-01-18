import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import AdminPage from "./pages/Admin/AdminPage";
import UserListPage from "./pages/Admin/UserRelated/Users/UsersListPage";
import UserFormPage from "./pages/Admin/UserRelated/Users/UserFormPage";

const AppRoutes = () => (
	<Router>
		<Routes>
			<Route path="/admin" element={<AdminPage />} />
			<Route path="/admin/users" element={<UserListPage />} />
			<Route path="/admin/users/:userId" element={<UserFormPage />} />
		</Routes>
	</Router>
);

export default AppRoutes;
