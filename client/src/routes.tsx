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
import SubscriptionPage from './pages/main/user/SubscriptionPage';
import SubscriptionListPage from './pages/admin/userRelated/subscriptions/SubscriptionListPage';
import SubscriptionFormPage from './pages/admin/userRelated/subscriptions/SubscriptionFormPage';
import OrderCheckoutPage from './pages/Main/order/OrderCheckoutPage';
import UserOrdersPage from './pages/main/user/UserOrdersPage';
import BookDetailsPage from './pages/main/book/BookDetailsPage';
import BookCatalogPage from './pages/main/book/BooksCatalogPage';
import OrderDetailsPage from './pages/admin/orderRelated/orders/OrderDetailsPage';
import ProfilePage from './pages/common/ProfilePage';
import OrderConfirmationPage from './pages/Main/order/OrderConfirmationPage';
import OrderSuccessPage from './pages/Main/order/OrderSuccessPage';
import BookListPage from './pages/Admin/BookRelated/Books/BooksListPage';
import AuthorsListPage from './pages/Admin/BookRelated/Authors/AuthorsListPage';
import AuthorFormPage from './pages/Admin/BookRelated/Authors/AuthorFormPage';
import PublishersListPage from './pages/Admin/BookRelated/Publishers/PublishersListPage';
import PublisherFormPage from './pages/Admin/BookRelated/Publishers/PublisherFormPage';
import FeedbacklistPage from './pages/Admin/BookRelated/Feedbacks/FeedbacksListPage';

const AppRoutes = () => (
  <AuthProvider>
    <BrowserRouter>
      <Routes>
        {/* Main */}
        <Route path='/' element={<MainPage />} />
        <Route path='/profile' element={<ProfilePage />} />
        <Route path='/checkout' element={<OrderCheckoutPage />} />
        <Route path='/checkout/confirm' element={<OrderConfirmationPage />} />
        <Route path='/checkout/success' element={<OrderSuccessPage/>} />
        <Route path='/orders' element={<UserOrdersPage />} />
        <Route path='/catalog' element={<BookCatalogPage/>}/>
        <Route path='/books/:bookId' element={<BookDetailsPage/>}/>
        <Route path='/orders/:orderId' element={<OrderDetailsPage />} />
        <Route
          path='/subscriptions/:subscriptionId'
          element={<SubscriptionPage />}
        />
        <Route element={<PrivateRoute />}>
          {/* Admin */}
          <Route path='/admin' element={<AdminLayout />}>
            <Route index element={<AdminPage />} />
            {/* User */}
            <Route path='/admin/users' element={<UserListPage />} />
            <Route path='/admin/users/add' element={<UserFormPage />} />
            <Route path='/admin/users/:userId' element={<UserFormPage />} />
            {/* Subscription */}
            <Route
              path='/admin/subscriptions'
              element={<SubscriptionListPage />}
            />
            <Route
              path='/admin/subscriptions/add'
              element={<SubscriptionFormPage />}
            />
            <Route
              path='/admin/subscriptions/:subscriptionId'
              element={<SubscriptionFormPage />}
            />
            {/* Order */}
            <Route path='/admin/orders' element={<OrderListPage />} />
            <Route path='/admin/orders/:orderId' element={<OrderFormPage />} />
            {/* Delivery Types */}
            <Route path='/admin/delivery' element={<DeliveriesListPage />} />
            <Route
              path='/admin/delivery/add'
              element={<DeliveryTypeFormPage />}
            />
            <Route
              path='/admin/delivery/:deliveryTypeId'
              element={<DeliveryTypeFormPage />}
            />
            {/*BOOKS*/}
            <Route path='/admin/booksRelated/books' element={<BookListPage />} />
            {/* AUTHOR */}
            <Route path='/admin/booksRelated/authors' element={<AuthorsListPage />} />
            <Route path='/admin/booksRelated/author/add' element={<AuthorFormPage />} />
            <Route path='/admin/booksRelated/author/:authorId' element={<AuthorFormPage />} />
            {/* PUBLISHER */}
            <Route path='/admin/booksRelated/publishers' element={<PublishersListPage />} />
            <Route path='/admin/booksRelated/publisher/add' element={<PublisherFormPage />} />
            <Route path='/admin/booksRelated/publisher/:publisherId' element={<PublisherFormPage />} />
            {/* FEEDBACK */}
            <Route path='/admin/booksRelated/feedbacks' element={<FeedbacklistPage />} />
            {/* Other */}
            <Route path='*' element={<NotFoundPage />} />
          </Route>
        </Route>
        <Route path='*' element={<NotFoundPage />} />
      </Routes>
    </BrowserRouter>
  </AuthProvider>
);

export default AppRoutes;
