import React from "react"
import type { UserSort } from "../../types"

interface UserSortProps {
	onSortChange: (field: keyof UserSort) => void
	sort: UserSort
}

const UserSort: React.FC<UserSortProps> = ({ onSortChange, sort }) => {
	return (
		<div>
			<button onClick={() => onSortChange("firstName")}>
				First Name {sort.firstName === true ? "↑" : sort.firstName === false ? "↓" : ""}
			</button>
			<button onClick={() => onSortChange("lastName")}>
				Last Name {sort.lastName === true ? "↑" : sort.lastName === false ? "↓" : ""}
			</button>
			<button onClick={() => onSortChange("dateOfBirth")}>
				Date of Birth {sort.dateOfBirth === true ? "↑" : sort.dateOfBirth === false ? "↓" : ""}
			</button>
			<button onClick={() => onSortChange("email")}>
				Email {sort.email === true ? "↑" : sort.email === false ? "↓" : ""}
			</button>
			<button onClick={() => onSortChange("phoneNumber")}>
				Phone Number {sort.phoneNumber === true ? "↑" : sort.phoneNumber === false ? "↓" : ""}
			</button>
		</div>
	)
}

export default UserSort
