import { Route, Routes } from "react-router-dom";
import { BrowserRouter } from "react-router-dom"

import MainPage from "./pages/Main/MainPage";

import AdminPage from "./pages/Admin/AdminPage";
import UserListPage from "./pages/Admin/UserRelated/Users/UsersListPage";
import UserFormPage from "./pages/Admin/UserRelated/Users/UserFormPage";
import NotFoundPage from "./pages/Main/NotFoundPage";

const AppRoutes = () => (
	<BrowserRouter>
		<Routes>
			<Route path="/" element={<MainPage />} />
			<Route path="/admin" element={<AdminPage />} />
			<Route path="/admin/users" element={<UserListPage />} />
			<Route path="/admin/users/add" element={<UserFormPage />} />
			<Route path="/admin/users/:userId" element={<UserFormPage />} />
			<Route path="*" element={<NotFoundPage />} />
		</Routes>
	</BrowserRouter>
);

export default AppRoutes;
