const BASE_URL = (port: number) => `https://localhost:${port}/api`

const USER_API_BASE_URL = BASE_URL(7007)
const ORDER_API_BASE_URL = BASE_URL(7051)
const BOOK_API_BASE_URL = BASE_URL(7084)

export const API_ROUTES = {
	AUTH: {
		ME: `${USER_API_BASE_URL}/auth/me`,
		LOGIN: `${USER_API_BASE_URL}/auth/login`,
		REGISTER: `${USER_API_BASE_URL}/auth/register`,
	},
	USERS: {
		BASE: `${USER_API_BASE_URL}/users`,
		PAGINATED: (pageNumber: number, pageSize: number) =>
			`${USER_API_BASE_URL}/users?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${USER_API_BASE_URL}/users/${id}`,
	},
	ORDERS: {
		BASE: `${ORDER_API_BASE_URL}/orders`,
		PAGINATED: (pageNumber: number, pageSize: number) =>
			`${ORDER_API_BASE_URL}/orders?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${ORDER_API_BASE_URL}/orders/${id}`,
	},
	BOOKS: {
		BASE: `${BOOK_API_BASE_URL}/books`,
		PAGINATED: (pageNumber: number, pageSize: number) =>
			`${BOOK_API_BASE_URL}/books?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${BOOK_API_BASE_URL}/books/${id}`,
	},
}
