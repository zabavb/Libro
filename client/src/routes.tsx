import { Route, Routes } from 'react-router-dom';
import { BrowserRouter } from 'react-router-dom';

import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';

import MainPage from './pages/main/MainPage';

import { AuthProvider } from './state/context/AuthContext';
import PrivateRoute from './privateRoute';

import AdminPage from './pages/admin/AdminPage';

import AdminLayout from './components/layouts/AdminLayout';

import UserListPage from './pages/admin/userRelated/users/UsersListPage';
import UserFormPage from './pages/admin/userRelated/users/UserFormPage';

import OrderFormPage from './pages/admin/orderRelated/orders/OrderFormPage';

import DeliveryTypeFormPage from './pages/admin/orderRelated/deliveries/DeliveryFormPage';

import BookListContainer from './containers/books/BookListContainer';

import NotFoundPage from './pages/common/NotFoundPage'; 
import OrderListPage from './pages/admin/orderRelated/orders/OrdersListPage';
import DeliveriesListPage from './pages/admin/orderRelated/deliveries/DeliveriesListPage';
import UserCartPage from './pages/main/UserCartPage';
import UserCheckoutPage from './pages/main/UserCheckoutPage';
import UserOrdersPage from './pages/main/UserOrdersPage';


const AppRoutes = () => (
  <AuthProvider>
    <BrowserRouter>
      <Routes>
        {/* Authentication */}
        <Route path='/login' element={<LoginPage />} />
        {/* Subscription */}
        {/* <Route 
				   	path="/subscription" 
					element={<Subscription />} 
				/>  */}
        <Route path='/register' element={<RegisterPage />} />
        {/* Main */}
        <Route path='/' element={<MainPage />} />
        <Route path='/cart' element={<UserCartPage />} />
        <Route path='/cart/checkout' element={<UserCheckoutPage />} />
        <Route path='/orders' element={<UserOrdersPage />} />
        <Route element={<PrivateRoute />}>
          {/* Admin */}
          <Route path='/admin' element={<AdminLayout />}>
            <Route index element={<AdminPage />} />
            {/* User */}
            <Route path='/admin/users' element={<UserListPage />} />
            <Route path='/admin/users/add' element={<UserFormPage />} />
            <Route path='/admin/users/:userId' element={<UserFormPage />} />
            {/* Order */}
            <Route path='/admin/orders' element={<OrderListPage />} />
            <Route path='/admin/orders/:orderId' element={<OrderFormPage />} />
            {/* Delivery Types */}
            <Route
              path='/admin/deliverytypes'
              element={<DeliveriesListPage />}
            />
            <Route
              path='/admin/deliverytypes/add'
              element={<DeliveryTypeFormPage />}
            />
            <Route
              path='/admin/deliverytypes/:deliveryTypeId'
              element={<DeliveryTypeFormPage />}
            />
            {/*BOOKS*/}
            <Route path='/admin/books' element={<BookListContainer />} />
            {/* Other */}
            <Route path='*' element={<NotFoundPage />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  </AuthProvider>
);

export default AppRoutes;
