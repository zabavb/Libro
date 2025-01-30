import { UserView, User, Role } from "../../types"
import { dateToString } from "./commonAdapters"

const roleNumberToEnum = (roleNumber: number): Role => {
	const roleMap: { [key: number]: Role } = {
		0: Role.ADMIN,
		1: Role.MODERATOR,
		2: Role.USER,
		3: Role.GUEST,
	}

	return roleMap[roleNumber] ?? Role.GUEST
}

export const UserToUserView = (response: User): UserView => {
	return {
		id: response.id,
		firstName: response.firstName,
		lastName: response.lastName,
		dateOfBirth: dateToString(response.dateOfBirth),
		email: response.email,
		phoneNumber: response.phoneNumber,
		role: roleNumberToEnum(response.role),
	}
}
