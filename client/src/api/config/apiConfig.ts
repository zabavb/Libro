const USER_API_BASE_URL = "https://localhost:7007/api"

export const API_ROUTES = {
	USERS: {
		BASE: `${USER_API_BASE_URL}/users`,
		BY_ID: (id: string) => `${USER_API_BASE_URL}/users/${id}`,
	},
}
