import { Route, Routes } from "react-router-dom"
import { BrowserRouter } from "react-router-dom"

import MainPage from "./pages/Main/MainPage"

import AdminPage from "./pages/Admin/AdminPage"
import UserFormPage from "./pages/Admin/UserRelated/Users/UserFormPage"
import NotFoundPage from "./pages/Main/NotFoundPage"
import UserListContainer from "./containers/user/UserListContainer"
import AdminLayout from "./components/layouts/AdminLayout"
import OrderListContainer from "./containers/order/OrderListContainer"
import OrderFormPage from "./pages/Admin/OrderRelated/Orders/OrderFormPage"
import BookListContainer from "./containers/books/BookListContainer"

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
				{/* User */}
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
				{/* Order */}
				<Route
					path="/admin/orders"
					element={<OrderListContainer />}
				/>
				<Route
					path="/admin/orders/add"
					element={<OrderFormPage />}
				/>
				<Route
					path="/admin/orders/:orderId"
					element={<OrderFormPage />}
				/>
			</Route>
			{/*BOOKS*/}
			<Route
					path="/admin/books"
					element={<BookListContainer />}
				/>
			{/* Other */}
			<Route
				path="*"
				element={<NotFoundPage />}
			/>
		</Routes>
	</BrowserRouter>
)

export default AppRoutes
