import { API_ROUTES } from "./config/apiConfig"

export const USERS = API_ROUTES.USERS.BASE

export const USERS_PAGINATED = (pageNumber: number = 1, pageSize: number = 10) =>
	API_ROUTES.USERS.PAGINATED(pageNumber, pageSize)

export const USER_BY_ID = API_ROUTES.USERS.BY_ID
