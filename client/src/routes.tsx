import { Route, Routes } from 'react-router-dom';
import { BrowserRouter } from 'react-router-dom';

import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';

import MainPage from './pages/main/MainPage';

import { AuthProvider } from './state/context/AuthContext';
import PrivateRoute from './privateRoute';

import AdminPage from './pages/admin/AdminPage';
// import AdminDashboard from './pages/admin/AdminDashboard';

import AdminLayout from './components/layouts/AdminLayout';

import UserListPage from './pages/admin/userRelated/users/UsersListPage';
import UserFormPage from './pages/admin/userRelated/users/UserFormPage';

import OrderFormPage from './pages/admin/orderRelated/orders/OrderFormPage';

import DeliveryTypeFormPage from './pages/admin/orderRelated/deliveries/DeliveryFormPage';

// import BookListContainer from './containers/books/BookListContainer';

import NotFoundPage from './pages/common/NotFoundPage';
import OrderListPage from './pages/admin/orderRelated/orders/OrdersListPage';
import DeliveriesListPage from './pages/admin/orderRelated/deliveries/DeliveriesListPage';
import SubscriptionPage from './pages/main/user/SubscriptionPage';
import SubscriptionListPage from './pages/admin/userRelated/subscriptions/SubscriptionListPage';
import SubscriptionFormPage from './pages/admin/userRelated/subscriptions/SubscriptionFormPage';
import OrderCheckoutPage from './pages/main/order/OrderCheckoutPage';
import UserOrdersPage from './pages/main/user/UserOrdersPage';
import LikedBooksPage from './pages/main/likedBooksPage';
import BookDetailsPage from './pages/main/book/BookDetailsPage';
import BookCatalogPage from './pages/main/book/BooksCatalogPage';
import OrderDetailsPage from './pages/admin/orderRelated/orders/OrderDetailsPage';
import ProfilePage from './pages/common/ProfilePage';
import OrderConfirmationPage from './pages/main/order/OrderConfirmationPage';
import OrderSuccessPage from './pages/main/order/OrderSuccessPage';
import BookListPage from './pages/admin/bookRelated/books/BooksListPage';
import AuthorsListPage from './pages/admin/bookRelated/authors/AuthorsListPage';
import AuthorFormPage from './pages/admin/bookRelated/authors/AuthorFormPage';
import PublishersListPage from './pages/admin/bookRelated/publishers/PublishersListPage';
import PublisherFormPage from './pages/admin/bookRelated/publishers/PublisherFormPage';
import FeedbacklistPage from './pages/admin/bookRelated/feedbacks/FeedbacksListPage';
import CategoriesListPage from './pages/admin/bookRelated/categories/CategoriesListPage';
import CategoryFormPage from './pages/admin/bookRelated/categories/CategoryFormPage';
import SubCategoryFormPage from './pages/admin/bookRelated/categories/SubCategoryFormPage';
import { CartProvider } from './state/context/CartContext';
import BookFormPage from './pages/admin/bookRelated/books/BookFormPage';
import LibraryPage from './pages/main/book/LibraryPage';
const AppRoutes = () => (
  <AuthProvider>
    <CartProvider>
      <BrowserRouter>
        <Routes>
          {/* Main */}
          <Route path='/' element={<MainPage />} />
          <Route path='/login' element={<LoginPage />} />
          <Route path='/register' element={<RegisterPage />} />
          <Route path='/profile' element={<ProfilePage />} />
          <Route path='/checkout' element={<OrderCheckoutPage />} />
          <Route path='/checkout/confirm' element={<OrderConfirmationPage />} />
          <Route path='/checkout/success' element={<OrderSuccessPage />} />
          <Route path='/orders' element={<UserOrdersPage />} />
          <Route path='/liked' element={<LikedBooksPage />} />
          <Route path='/library' element={<LibraryPage/>} />
          <Route path='/catalog' element={<BookCatalogPage />} />
          <Route path='/books/:bookId' element={<BookDetailsPage />} />
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
              <Route path='/admin/booksRelated/book/add' element={<BookFormPage />} />
              <Route path='/admin/booksRelated/book/:bookId' element={<BookFormPage />} />
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
              {/* CATEGORY */}
              <Route path='/admin/booksRelated/categories' element={<CategoriesListPage/>}/>
              <Route path='/admin/booksRelated/category/add' element={<CategoryFormPage/>}/>
              {/* SUBCATEGORY */}
              <Route path='/admin/booksRelated/subcategory/add/:categoryId' element={<SubCategoryFormPage/>}/>
              {/* Other */}
              <Route path='*' element={<NotFoundPage />} />
            </Route>
          </Route>
          <Route path='*' element={<NotFoundPage />} />
        </Routes>
      </BrowserRouter>
    </CartProvider>
  </AuthProvider>
);

export default AppRoutes;
