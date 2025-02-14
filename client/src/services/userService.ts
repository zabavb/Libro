import { User, PaginatedResponse, UserFilter, UserSort } from "../types"
import {
	getAllUsers,
	getUserById,
	createUser,
	updateUser,
	deleteUser,
} from "../api/repositories/userRepository"
import { roleEnumToNumber } from "../api/adapters/userAdapter"

/**
 * Fetch a paginated list of users with optional search term, filters, and sorting.
 */
export const fetchUsersService = async (
	pageNumber: number = 1,
	pageSize: number = 10,
	searchTerm?: string,
	filters?: UserFilter,
	sort?: UserSort
): Promise<PaginatedResponse<User>> => {
	try {
		const formattedSort = Object.fromEntries(
			Object.entries(sort ?? {}).map(([key, value]) => [key, value ? 1 : 2])
		)

		const params = {
			searchTerm,
			...filters,
			role: filters?.role !== undefined ? roleEnumToNumber(filters.role).toString() : undefined,
			...formattedSort,
		}

		return await getAllUsers(pageNumber, pageSize, params)
	} catch (error) {
		console.error(`fetchUsersService: Failed to fetch users`, error)
		throw new Error("An error occurred while fetching users. Please try again later.")
	}
}

/**
 * Fetch a single user by their ID.
 */
export const fetchUserByIdService = async (id: string): Promise<User> => {
	try {
		return await getUserById(id)
	} catch (error) {
		console.error(`fetchUserByIdService: Failed to fetch user ID [${id}]`, error)
		throw new Error(`An error occurred while fetching the user. Please try again later.`)
	}
}

/**
 * Create a new user.
 */
export const addUserService = async (user: Partial<FormData>): Promise<FormData> => {
	try {
		return await createUser(user)
	} catch (error) {
		console.error(`addUserService: Failed to create user`, error)
		throw new Error("An error occurred while adding the user. Please try again later.")
	}
}

/**
 * Update an existing user by ID.
 */
export const editUserService = async (id: string, user: Partial<FormData>): Promise<FormData> => {
	try {
		return await updateUser(id, user)
	} catch (error) {
		console.error(`editUserService: Failed to update user ID [${id}]`, error)
		throw new Error("An error occurred while updating the user. Please try again later.")
	}
}

/**
 * Delete a user by ID.
 */
export const removeUserService = async (id: string, imageUrl: string): Promise<void> => {
	try {
		await deleteUser(id, imageUrl)
	} catch (error) {
		console.error(`removeUserService: Failed to delete user ID [${id}]`, error)
		throw new Error("An error occurred while deleting the user. Please try again later.")
	}
}
