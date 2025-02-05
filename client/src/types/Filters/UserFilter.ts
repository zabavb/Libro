import { Role } from "../subTypes/Role"

export interface UserFilter {
	dateOfBirthStart?: string
	dateOfBirthEnd?: string
	role?: Role
	hasSubscription?: boolean
}
