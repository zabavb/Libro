import { Route, Routes } from 'react-router-dom';
import { BrowserRouter } from 'react-router-dom';

import LoginPage from './pages/Auth/LoginPage';
import RegisterPage from './pages/Auth/RegisterPage';

import MainPage from './pages/Main/MainPage';

import { AuthProvider } from './state/context/AuthContext';
import PrivateRoute from './privateRoute';

import AdminPage from './pages/Admin/AdminPage';

import AdminLayout from './components/layouts/AdminLayout';

import UserListPage from './pages/Admin/UserRelated/Users/UsersListPage';
import UserFormPage from './pages/Admin/UserRelated/Users/UserFormPage';

import OrderFormPage from './pages/Admin/OrderRelated/Orders/OrderFormPage';

import DeliveryTypeFormPage from './pages/Admin/OrderRelated/Deliveries/DeliveryFormPage';


import BookListContainer from './containers/books/BookListContainer';

import NotFoundPage from './pages/common/NotFoundPage';
import OrderListPage from './pages/Admin/OrderRelated/Orders/OrdersListPage';
import DeliveriesListPage from './pages/Admin/OrderRelated/Deliveries/DeliveriesListPage';
import UserBasketPage from './pages/Admin/UserRelated/Users/UserBasketPage';
import UserCheckoutPage from './pages/Main/UserCheckoutPage';


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
        <Route path='/Basket' element={<UserBasketPage />}/>
        <Route path='/Basket/Checkout' element={<UserCheckoutPage />}/>
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
            <Route path='/admin/orders/add' element={<OrderFormPage />} />
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
