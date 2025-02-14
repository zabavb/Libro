const USER_API_BASE_URL = "https://localhost:7007/api"
const ORDER_API_BASE_URL = "https://localhost:7051/api"

export const API_ROUTES = {
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
	DELIVERY: {
		BASE: `${ORDER_API_BASE_URL}/deliverytypes`,
		PAGINATED: (pageNumber: number, pageSize: number) => 
			`${ORDER_API_BASE_URL}/deliverytypes/pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${ORDER_API_BASE_URL}/deliverytypes/${id}`,
	},
}
