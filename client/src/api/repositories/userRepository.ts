import axios from "axios"
import { API_ROUTES } from "../config/apiConfig"

export interface User {
	id: string
	firstName: string
	lastName: string
	dateOfBirth: string
	email: string
	phoneNumber: string
	role: string
}

export const getAllUsers = async (): Promise<User[]> => {
	const response = await axios.get(API_ROUTES.USERS.BASE)
	return response.data
}

export const getUserById = async (id: string): Promise<User> => {
	const response = await axios.get(API_ROUTES.USERS.BY_ID(id))
	return response.data
}

export const createUser = async (user: Partial<User>): Promise<User> => {
	const response = await axios.post(API_ROUTES.USERS.BASE, user)
	return response.data
}

export const updateUser = async (id: string, user: Partial<User>): Promise<User> => {
	const response = await axios.put(API_ROUTES.USERS.BY_ID(id), user)
	return response.data
}

export const deleteUser = async (id: string): Promise<void> => {
	await axios.delete(API_ROUTES.USERS.BY_ID(id))
}
