import { Route, Routes } from 'react-router-dom';
import { BrowserRouter } from 'react-router-dom';

import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';

import MainPage from './pages/main/MainPage';

import { AuthProvider } from './state/context/AuthContext';
import PrivateRoute from './privateRoute';

import AdminLayout from './components/layouts/AdminLayout';
import AdminPage from './pages/admin/AdminPage';

import UserListPage from './pages/admin/UserRelated/users/UsersListPage';
import UserFormPage from './pages/admin/UserRelated/users/UserFormPage';

import OrderListContainer from './containers/order/OrderListContainer';
import OrderFormPage from './pages/admin/OrderRelated/Orders/OrderFormPage';

import DeliveryTypeListContainer from './containers/order/DeliveryTypeListContainer';
import DeliveryTypeFormPage from './pages/admin/OrderRelated/Deliveries/DeliveryFormPage';

import BookListContainer from './containers/books/BookListContainer';

import NotFoundPage from './pages/common/NotFoundPage';
import UserOrdersPage from './pages/Admin/UserRelated/Users/UserOrdersPage';

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
            <Route path='/admin/orders' element={<OrderListContainer />} />
            <Route path='/admin/orders/add' element={<OrderFormPage />} />
            <Route path='/admin/orders/:orderId' element={<OrderFormPage />} />
            {/* Delivery Types */}
            <Route
              path='/admin/deliverytypes'
              element={<DeliveryTypeListContainer />}
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
