import { Route, Routes } from "react-router-dom"
import { BrowserRouter } from "react-router-dom"

import LoginContainer from "./containers/auth/LoginContainer"
import RegisterContainer from "./containers/auth/RegisterContainer"

import MainPage from "./pages/Main/MainPage"
import SubscriptionPage from "./pages/Main/SubscriptionPage"
import SubscriptionDetailsPage from "./pages/Admin/UserRelated/Subscriptions/SubscriptionDetailsPage"

import SubscriptionFormPage from "./pages/Admin/UserRelated/Subscriptions/SubscriptionFormPage"
import { AuthProvider } from "./state/context/AuthContext"
import PrivateRoute from "./privateRoute"

import AdminLayout from "./components/layouts/AdminLayout"
import AdminPage from "./pages/Admin/AdminPage"

import UserFormPage from "./pages/Admin/UserRelated/Users/UserFormPage"
import UserListContainer from "./containers/user/UserListContainer"

import OrderListContainer from "./containers/order/OrderListContainer"
import OrderFormPage from "./pages/Admin/OrderRelated/Orders/OrderFormPage"
import DeliveryTypeListContainer from "./containers/order/DeliveryTypeListContainer"
import DeliveryTypeFormPage from "./pages/Admin/OrderRelated/Deliveries/DeliveryFormPage"

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
				<Route
					path="/register"
					element={<RegisterContainer />}
				/>
				{/* Subscriptions */}
				<Route 
				    path="/" 
					element={<SubscriptionPage />} 
				/>
                <Route 
					path="/subscriptions/:id" 
					element={<SubscriptionDetailsPage />} 
				/>
                <Route 
					path="/subscriptions/new" 
					element={<SubscriptionFormPage />} 
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
						{/* Delivery Types */}
						<Route
							path="/admin/deliverytypes"
							element={<DeliveryTypeListContainer/>}
						/>
						<Route
							path="/admin/deliverytypes/add"
							element={<DeliveryTypeFormPage />}
						/>
						<Route
							path="/admin/deliverytypes/:deliveryTypeId"
							element={<DeliveryTypeFormPage />}
						/>
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
					</Route>
				</Route>
			</Routes>
		</BrowserRouter>
	</AuthProvider>

)

export default AppRoutes
