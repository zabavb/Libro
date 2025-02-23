import React from "react"
import type { UserFilter } from "../../types"
import { Role } from "../../types"

interface UserFilterProps {
	filters: UserFilter
	onFilterChange: (filters: UserFilter) => void
}

const UserFilter: React.FC<UserFilterProps> = ({ onFilterChange, filters }) => {
	return (
		<div>
			<h3>Filters</h3>
			<label>
				Date of Birth (Start):
				<input
					type="date"
					value={filters.dateOfBirthStart ? filters.dateOfBirthStart : ""}
					onChange={(e) =>
						onFilterChange({
							...filters,
							dateOfBirthStart: e.target.value,
						})
					}
				/>
			</label>
			<label>
				Date of Birth (End):
				<input
					type="date"
					value={filters.dateOfBirthEnd ? filters.dateOfBirthEnd : ""}
					onChange={(e) =>
						onFilterChange({
							...filters,
							dateOfBirthEnd: e.target.value,
						})
					}
				/>
			</label>
			<label>
				Email:
				<select
					value={filters.email || ""}
					onChange={(e) => onFilterChange({ ...filters, email: e.target.value })}>
					<option value="">All</option>
					<option value="@gmail.com">Gmail</option>
					<option value="@outlook.com">Outlook</option>
					<option value="@hotmail.com">Hotmail</option>
					<option value="@icloud.com">iCloud</option>
					<option value="@yahoo.com">Yahoo</option>
					<option value="@msn.com">MSN</option>
					<option value="@aol.com">AOL</option>
					<option value="@comcast.com">Comcast</option>
				</select>
			</label>
			<label>
				Role:
				<select
					value={filters.role || ""}
					onChange={(e) => onFilterChange({ ...filters, role: e.target.value as Role })}>
					<option value="">All</option>
					{Object.values(Role).map((role) => (
						<option
							key={role}
							value={role}>
							{role}
						</option>
					))}
				</select>
			</label>
			<label>
				Has Subscription:
				<input
					type="checkbox"
					checked={filters.hasSubscription ?? false}
					onChange={(e) => onFilterChange({ ...filters, hasSubscription: e.target.checked })}
				/>
			</label>
		</div>
	)
}

export default UserFilter
