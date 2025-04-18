import React from "react"
import type { UserSort } from "../../types"

interface UserSortProps {
	onSortChange: (field: keyof UserSort) => void
	sort: UserSort
}

const UserSort: React.FC<UserSortProps> = ({ onSortChange, sort }) => {
	return (
		<div>
			<button onClick={() => onSortChange("alphabetical")}>
				Alphabetical {sort.alphabetical === true ? "↑" : sort.alphabetical === false ? "↓" : ""}
			</button>
			<button onClick={() => onSortChange("youngest")}>
				Age {sort.youngest === true ? "↑" : sort.youngest === false ? "↓" : ""}
			</button>
			<button onClick={() => onSortChange("role")}>
				Role {sort.role === true ? "↑" : sort.role === false ? "↓" : ""}
			</button>
		</div>
	)
}

export default UserSort
