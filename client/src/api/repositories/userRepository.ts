import axios from "axios"
import { USERS_PAGINATED, USERS, USER_BY_ID } from "../index"
import { User, PaginatedResponse } from "../../types"

export const getAllUsers = async (
	pageNumber: number = 1,
	pageSize: number = 10,
	params: {
		role: string | undefined
		dateOfBirthStart?: string
		dateOfBirthEnd?: string
		hasSubscription?: boolean
		searchTerm: string | undefined
	}
): Promise<PaginatedResponse<User>> => {
	const url = USERS_PAGINATED(pageNumber, pageSize)
	const response = await axios.get<PaginatedResponse<User>>(url, { params })
	return response.data
}

export const getUserById = async (id: string): Promise<User> => {
	const response = await axios.get(USER_BY_ID(id))
	return response.data
}

export const createUser = async (user: Partial<User>): Promise<User> => {
	const response = await axios.post(USERS, user)
	return response.data
}

export const updateUser = async (id: string, user: Partial<User>): Promise<User> => {
	const response = await axios.put(USER_BY_ID(id), user)
	return response.data
}

export const deleteUser = async (id: string): Promise<void> => {
	await axios.delete(USER_BY_ID(id))
}
