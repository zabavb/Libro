import axios from "axios"
import { USERS, USER_BY_ID } from "../index"
import { User } from "../../types"

export interface PaginatedResponse<T> {
	items: T[]
	pageNumber: 1
	pageSize: 10
	totalPages: 1
	hasPreviousPage: false
	hasNextPage: false
}

export const getAllUsers = async (): Promise<PaginatedResponse<User>> => {
	try {
		const response = await axios.get(USERS)
		return response.data
	} catch (error) {
		throw new Error(`Failed to fetch list of users: ${error}`)
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
