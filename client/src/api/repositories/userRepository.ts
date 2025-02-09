import axios from "axios"
import { USERS_PAGINATED, USERS, USER_BY_ID } from "../index"
import { User, PaginatedResponse } from "../../types"

interface UserQueryParams {
	role?: string
	dateOfBirthStart?: string
	dateOfBirthEnd?: string
	hasSubscription?: boolean
	searchTerm?: string
}

/**
 * Fetch a paginated list of users with optional filters.
 */
export const getAllUsers = async (
	pageNumber: number = 1,
	pageSize: number = 10,
	params: UserQueryParams = {}
): Promise<PaginatedResponse<User>> => {
	const url = USERS_PAGINATED(pageNumber, pageSize)
	const response = await axios.get<PaginatedResponse<User>>(url, { params })
	return response.data
}

/**
 * Fetch a single user by their ID.
 */
export const getUserById = async (id: string): Promise<User> => {
	const response = await axios.get<User>(USER_BY_ID(id))
	return response.data
}

/**
 * Create a new user.
 */
export const createUser = async (user: Partial<User>): Promise<User> => {
	const response = await axios.post<User>(USERS, user)
	return response.data
}

/**
 * Update an existing user by ID.
 */
export const updateUser = async (id: string, user: Partial<User>): Promise<User> => {
	const response = await axios.put<User>(USER_BY_ID(id), user)
	return response.data
}

/**
 * Delete a user by ID.
 */
export const deleteUser = async (id: string): Promise<void> => {
	await axios.delete(USER_BY_ID(id))
}
