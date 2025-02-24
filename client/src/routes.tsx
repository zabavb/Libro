import { Route, Routes } from "react-router-dom"
import { BrowserRouter } from "react-router-dom"

import LoginContainer from "./containers/auth/LoginContainer"
import RegisterContainer from "./containers/auth/RegisterContainer"

import MainPage from "./pages/Main/MainPage"

import { AuthProvider } from "./state/context/AuthContext"
import PrivateRoute from "./privateRoute"

import AdminLayout from "./components/layouts/AdminLayout"
import AdminPage from "./pages/Admin/AdminPage"

import UserFormPage from "./pages/Admin/UserRelated/Users/UserFormPage"
import UserListContainer from "./containers/user/UserListContainer"

import OrderListContainer from "./containers/order/OrderListContainer"
import OrderFormPage from "./pages/Admin/OrderRelated/Orders/OrderFormPage"
import BookListContainer from "./containers/books/BookListContainer"

import NotFoundPage from "./pages/Main/NotFoundPage"

const AppRoutes = () => (
	<AuthProvider>
		<BrowserRouter>
			<Routes>
				{/* Authentication */}
				<Route
					path="/login"
					element={<LoginContainer />}
				/>
				{/* Subscription */}
				<Route 
				   	path="/subscription" 
					element={<Subscription />} 
				/> 
				<Route
					path="/register"
					element={<RegisterContainer />}
				/>
				{/* Main */}
				<Route
					path="/"
					element={<MainPage />}
				/>
				<Route element={<PrivateRoute />}>
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
				</Route>

				{/* Other */}
				<Route
					path="*"
					element={<NotFoundPage />}
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
	</AuthProvider>

)

export default AppRoutes
