import { Role } from "../subObjects/Role"

export interface UserFilter {
	dateOfBirthStart?: string
	dateOfBirthEnd?: string
	role?: Role
	hasSubscription?: boolean
}
