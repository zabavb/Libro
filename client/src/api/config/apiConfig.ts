const GATEWAY = `https://localhost:7102/gateway`
const AUTH = `${GATEWAY}/auth`
const USERS = `${GATEWAY}/users`

export const API_ROUTES = {
	AUTH: {
		ME: `${AUTH}/me`,
		LOGIN: `${AUTH}/login`,
		REGISTER: `${AUTH}/register`,
	},
	USERS: {
		BASE: USERS,
		PAGINATED: (pageNumber: number, pageSize: number) =>
			`${USERS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${USERS}/${id}`,
	},
	ORDERS: {
		BASE: `${GATEWAY}/orders`,
		PAGINATED: (pageNumber: number, pageSize: number) =>
			`${GATEWAY}/orders?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${GATEWAY}/orders/${id}`,
	},
	DELIVERY: {
		BASE: `${GATEWAY}/deliverytypes`,
		PAGINATED: (pageNumber: number, pageSize: number) => 
			`${GATEWAY}/deliverytypes?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${GATEWAY}/deliverytypes/${id}`,
	},
	BOOKS: {
		BASE: `${GATEWAY}/books`,
		PAGINATED: (pageNumber: number, pageSize: number) =>
			`${GATEWAY}/books?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${GATEWAY}/books/${id}`,
	},
}
