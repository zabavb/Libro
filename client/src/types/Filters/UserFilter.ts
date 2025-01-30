import { Role } from "../subObjects/Role"

export interface UserFilter {
	dateOdBirthStart?: Date
	dateOdBirthEnd?: Date
	role?: Role
	hasSubscription?: boolean
}
