const GATEWAY = `https://localhost:7102/gateway`
const AUTH = `${GATEWAY}/auth`
const USERS = `${GATEWAY}/users`
const ORDERS = `${GATEWAY}/orders`
const DELIVERY = `${GATEWAY}/deliverytypes`
const BOOKS = `${GATEWAY}/books`

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
		BASE: `${ORDERS}`,
		PAGINATED: (pageNumber: number, pageSize: number) =>
			`${ORDERS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${ORDERS}/${id}`,
	},
	DELIVERY: {
		BASE: `${DELIVERY}`,
		PAGINATED: (pageNumber: number, pageSize: number) => 
			`${DELIVERY}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `$${DELIVERY}/${id}`,
	},
	BOOKS: {
		BASE: `${BOOKS}`,
		PAGINATED: (pageNumber: number, pageSize: number) =>
			`${BOOKS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
		BY_ID: (id: string) => `${BOOKS}/${id}`,
	},
}
