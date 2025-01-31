import axios from "axios"
import { USERS_PAGINATED, USERS, USER_BY_ID } from "../index"
import { User, PaginatedResponse } from "../../types"

export const getAllUsers = async (
	pageNumber: number = 1,
	pageSize: number = 10
): Promise<PaginatedResponse<User>> => {
	try {
		const url = USERS_PAGINATED(pageNumber, pageSize)
		const response = await axios.get<PaginatedResponse<User>>(url)
		return response.data
	} catch (error: unknown) {
		if (axios.isAxiosError(error)) {
			throw new Error(
				`Failed to fetch list of users: ${error.response?.data?.message || error.message}`
			)
		}
		throw new Error(`Failed to fetch list of users: ${String(error)}`)
	}
}

export const getUserById = async (id: string): Promise<User> => {
	try {
		const response = await axios.get(USER_BY_ID(id))
		return response.data
	} catch (error) {
		throw new Error(`Failed to fetch user: ${error}`)
	}
}

export const createUser = async (user: Partial<User>): Promise<User> => {
	try {
		const response = await axios.post(USERS, user)
		console.log(`userRepository.ts: create user:[${user}$] response:[${response.data}$]`)
		return response.data
	} catch (error) {
		throw new Error(`Failed to add user: ${error}`)
	}
}

export const updateUser = async (id: string, user: Partial<User>): Promise<User> => {
	try {
		const response = await axios.put(USER_BY_ID(id), user)
		console.log(`userRepository.ts: update id:[${id}] user:[${user}$] response:[${response.data}$]`)
		return response.data
	} catch (error) {
		throw new Error(`Failed to update user: ${error}`)
	}
}

export const deleteUser = async (id: string): Promise<void> => {
	try {
		console.log(`userRepository.ts: delete id:[${id}$]`)
		await axios.delete(USER_BY_ID(id))
	} catch (error) {
		throw new Error(`Failed to delete user: ${error}`)
	}
}
