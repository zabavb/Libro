import { API_ROUTES } from './config/apiConfig';

// ================= AUTHENTICATION =================

export const LOGIN = API_ROUTES.AUTH.LOGIN;
export const REGISTER = API_ROUTES.AUTH.REGISTER;
export const OAUTH = API_ROUTES.AUTH.OAUTH;

// ================= USER =================

export const USERS = API_ROUTES.USERS.BASE;

export const USERS_PAGINATED = (
  pageNumber: number = 1,
  pageSize: number = 10,
) => API_ROUTES.USERS.PAGINATED(pageNumber, pageSize);

export const USER_BY_ID = API_ROUTES.USERS.BY_ID;

// ================= SUBSCRIPTION =================

export const SUBSCRIPTIONS = API_ROUTES.SUBSCRIPTIONS.BASE;

export const SUBSCRIPTIONS_PAGINATED = (
  pageNumber: number = 1,
  pageSize: number = 10,
) => API_ROUTES.SUBSCRIPTIONS.PAGINATED(pageNumber, pageSize);

export const SUBSCRIPTION_BY_ID = API_ROUTES.SUBSCRIPTIONS.BY_ID;

export const SUBSCRIBE = API_ROUTES.SUBSCRIPTIONS.SUBSCRIBE;
export const UNSUBSCRIBE = API_ROUTES.SUBSCRIPTIONS.UNSUBSCRIBE;

export const FOR_FILTERING = API_ROUTES.SUBSCRIPTIONS.FOR_FILTERING;

// ================= ORDER API  =================
//================= ORDER =================
export const ORDERS = API_ROUTES.ORDERS.BASE;

export const ORDERS_PAGINATED = (
  pageNumber: number = 1,
  pageSize: number = 10,
) => API_ROUTES.ORDERS.PAGINATED(pageNumber, pageSize);

export const ORDER_BY_ID = API_ROUTES.ORDERS.BY_ID;

// ================= DELIVERY TYPE =================

export const DELIVERYTYPES = API_ROUTES.DELIVERY.BASE;

export const DELIVERYTYPES_PAGINATED = (
  pageNumber: number = 1,
  pageSize: number = 10,
) => API_ROUTES.DELIVERY.PAGINATED(pageNumber, pageSize);

export const DELIVERYTYPE_BY_ID = API_ROUTES.DELIVERY.BY_ID;

// ================= BOOK =================
export const BOOKS = API_ROUTES.BOOKS.BASE;

export const BOOKS_PAGINATED = (
  pageNumber: number = 1,
  pageSize: number = 10,
) => API_ROUTES.BOOKS.PAGINATED(pageNumber, pageSize);

export const BOOK_BY_ID = API_ROUTES.BOOKS.BY_ID;
