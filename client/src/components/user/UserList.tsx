import React from "react"
import { User } from "../../types"
import UserFilter from "./UserFilter"
import UserSort from "./UserSort"
import UserCardContainer from "../../containers/user/UserCardContainer"
import Pagination from "../common/Pagination"
import Search from "../common/Search"

interface UserListProps {
	users?: User[]
	loading: boolean
	error: string | null | undefined
	pagination: { pageNumber: number; pageSize: number; totalCount: number }
	onPageChange: (pageNumber: number) => void
	onNavigate: (path: string) => void
	onSearchTermChange: (searchTerm: string) => void
	searchTerm: string
	onFilterChange: (filters: UserFilter) => void
	filters: UserFilter
	onSortChange: (field: keyof UserSort) => void
	sort: UserSort
}

const UserList: React.FC<UserListProps> = ({
	users = [],
	loading,
	error,
	pagination,
	onPageChange,
	searchTerm,
	onSearchTermChange,
	filters,
	onFilterChange,
	sort,
	onSortChange,
	onNavigate,
}) => {
	if (loading) return <p>Loading...</p>
	if (error) return <p>Error: {error}</p>

	return (
		<div>
			<p onClick={() => onNavigate("/admin")}>Back to Admin Dashboard</p>
			<p onClick={() => onNavigate("/admin/users/add")}>Add User</p>
			<h1>User List</h1>

			<Search
				searchTerm={searchTerm}
				onSearchTermChange={onSearchTermChange}
			/>

			<UserFilter
				filters={filters}
				onFilterChange={onFilterChange}
			/>

			<UserSort
				sort={sort}
				onSortChange={onSortChange}
			/>

			<div>
				{users.map((user) => (
					<UserCardContainer
						key={user.id}
						user={user}
					/>
				))}
			</div>

			<hr />
			<Pagination
				pagination={pagination}
				onPageChange={onPageChange}
			/>
		</div>
	)
}

export default UserList
