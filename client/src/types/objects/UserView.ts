import { Role } from "../subObjects/Role"

export interface UserView {
	id: string
	firstName: string
	lastName: string
	dateOfBirth: string
	email: string
	phoneNumber: string
	role: Role
}
