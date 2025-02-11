import { API_ROUTES } from "./config/apiConfig"

// ================= USER =================

export const USERS = API_ROUTES.USERS.BASE

export const USERS_PAGINATED = (pageNumber: number = 1, pageSize: number = 10) =>
	API_ROUTES.USERS.PAGINATED(pageNumber, pageSize)

export const USER_BY_ID = API_ROUTES.USERS.BY_ID

// ================= ORDER =================

export const ORDERS = API_ROUTES.ORDERS.BASE

export const ORDERS_PAGINATED = (pageNumber: number = 1, pageSize: number = 10) =>
	API_ROUTES.ORDERS.PAGINATED(pageNumber,pageSize)

export const ORDER_BY_ID = API_ROUTES.ORDERS.BY_ID