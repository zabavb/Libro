import { API_ROUTES } from "./config/apiConfig"

// ================= AUTHENTICATION =================

export const LOGIN = API_ROUTES.AUTH.LOGIN
export const REGISTER = API_ROUTES.AUTH.REGISTER
export const ME = API_ROUTES.AUTH.ME

// ================= USER =================

export const USERS = API_ROUTES.USERS.BASE

export const USERS_PAGINATED = (pageNumber: number = 1, pageSize: number = 10) =>
	API_ROUTES.USERS.PAGINATED(pageNumber, pageSize)

export const USER_BY_ID = API_ROUTES.USERS.BY_ID

// ================= ORDER API  =================
	//================= ORDER =================
	export const ORDERS = API_ROUTES.ORDERS.BASE

	export const ORDERS_PAGINATED = (pageNumber: number = 1, pageSize: number = 10) =>
		API_ROUTES.ORDERS.PAGINATED(pageNumber,pageSize)

	export const ORDER_BY_ID = API_ROUTES.ORDERS.BY_ID

	// ================= DELIVERY TYPE =================

	export const DELIVERYTYPES = API_ROUTES.DELIVERY.BASE

	export const DELIVERYTYPES_PAGINATED = (pageNumber: number = 1, pageSize: number = 10) =>
		API_ROUTES.DELIVERY.PAGINATED(pageNumber,pageSize)

	export const DELIVERYTYPE_BY_ID = API_ROUTES.DELIVERY.BY_ID

