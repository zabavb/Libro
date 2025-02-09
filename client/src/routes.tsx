import { Route, Routes } from "react-router-dom";
import { BrowserRouter } from "react-router-dom"

import MainPage from "./pages/Main/MainPage";

import AdminPage from "./pages/Admin/AdminPage";
import UserFormPage from "./pages/Admin/UserRelated/Users/UserFormPage";
import NotFoundPage from "./pages/Main/NotFoundPage";
import UserListContainer from "./containers/user/UserListContainer";
import OrderListContainer from "./containers/order/OrderListContainer";
import OrderFormPage from "./pages/Admin/OrderRelated/Orders/OrderFormPage";


const AppRoutes = () => (
	<BrowserRouter>
		<Routes>
			<Route path="/" element={<MainPage />} />
			<Route path="/admin" element={<AdminPage />} />
				{/* User */}
			<Route path="/admin/users" element={<UserListContainer />} />
			<Route path="/admin/users/add" element={<UserFormPage />} />
			<Route path="/admin/users/:userId" element={<UserFormPage />} />
				{/* Order */}
			<Route path="/admin/orders" element={<OrderListContainer />}/>
			<Route path="/admin/orders/add" element={<OrderFormPage />} />
			<Route path="/admin/orders/:orderId" element={<OrderFormPage />} />
			<Route path="*" element={<NotFoundPage />} />
		</Routes>
	</BrowserRouter>
);

export default AppRoutes;
