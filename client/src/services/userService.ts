import { User, PaginatedResponse, UserFilter, UserSort } from "../types"
import {
	getAllUsers,
	getUserById,
	createUser,
	updateUser,
	deleteUser,
} from "../api/repositories/userRepository"
import { roleEnumToNumber } from "../api/adapters/userAdapter"

export const fetchUsersService = async (
	pageNumber: number = 1,
	pageSize: number = 10,
	searchTerm?: string,
	filters?: UserFilter,
	sort?: UserSort
): Promise<PaginatedResponse<User>> => {
	const formattedSort = Object.fromEntries(
		Object.entries(sort || {}).map(([key, value]) => [key, value ? 1 : 2])
	)

	const params = {
		searchTerm,
		...filters,
		role: filters?.role !== undefined ? roleEnumToNumber(filters.role).toString() : undefined,
		...formattedSort,
	}

	try {
		return await getAllUsers(pageNumber, pageSize, params)
	} catch (error) {
		throw new Error(`Error fetching users: ${error}`)
	}
}

export const fetchUserByIdService = async (id: string): Promise<User> => {
	try {
		return await getUserById(id)
	} catch (error) {
		throw new Error(`Error fetching user by ID: ${error}`)
	}
}

export const addUserService = async (user: Partial<User>): Promise<User> => {
	try {
		return await createUser(user)
	} catch (error) {
		throw new Error(`Error adding user: ${error}`)
	}
}

export const editUserService = async (id: string, user: Partial<User>): Promise<User> => {
	try {
		return await updateUser(id, user)
	} catch (error) {
		throw new Error(`Error updating user: ${error}`)
	}
}

export const removeUserService = async (id: string): Promise<void> => {
	try {
		await deleteUser(id)
	} catch (error) {
		throw new Error(`Error deleting user: ${error}`)
	}
}
