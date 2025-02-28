import { UserView, User, Role } from "../../types"
import { dateToString } from "./commonAdapters"

export const roleNumberToEnum = (roleNumber: number): Role => {
	const roleMap: { [key: number]: Role } = {
		0: Role.ADMIN,
		1: Role.MODERATOR,
		2: Role.USER,
		3: Role.GUEST,
	}

	return roleMap[roleNumber] ?? Role.GUEST
}

export const roleEnumToNumber = (role: Role): number => {
	const roleMap: { [key in Role]: number } = {
		[Role.ADMIN]: 0,
		[Role.MODERATOR]: 1,
		[Role.USER]: 2,
		[Role.GUEST]: 3,
	}

	return roleMap[role] ?? 2
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
		imageUrl: response.imageUrl,
	}
}
