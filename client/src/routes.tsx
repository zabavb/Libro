import { Route, Routes } from "react-router-dom"
import { BrowserRouter } from "react-router-dom"

import MainPage from "./pages/Main/MainPage"

import AdminPage from "./pages/Admin/AdminPage"
import UserFormPage from "./pages/Admin/UserRelated/Users/UserFormPage"
import NotFoundPage from "./pages/Main/NotFoundPage"
import UserListContainer from "./containers/user/UserListContainer"
import AdminLayout from "./components/layouts/AdminLayout"

const AppRoutes = () => (
	<BrowserRouter>
		<Routes>
			{/* Main */}
			<Route
				path="/"
				element={<MainPage />}
			/>

			{/* Admin */}
			<Route
				path="/admin"
				element={<AdminLayout />}>
				<Route
					index
					element={<AdminPage />}
				/>
				<Route
					path="/admin/users"
					element={<UserListContainer />}
				/>
				<Route
					path="/admin/users/add"
					element={<UserFormPage />}
				/>
				<Route
					path="/admin/users/:userId"
					element={<UserFormPage />}
				/>
			</Route>
			{/* Other */}
			<Route
				path="*"
				element={<NotFoundPage />}
			/>
		</Routes>
	</BrowserRouter>
)

export default AppRoutes
