import { Role } from "../subTypes/Role"

export interface UserFilter {
	dateOfBirthStart?: string
	dateOfBirthEnd?: string
	email?: string
	role?: Role
	hasSubscription?: boolean
}
