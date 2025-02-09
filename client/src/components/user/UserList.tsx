import React from "react"
import { User } from "../../types"
import UserFilter from "./UserFilter"
import UserSort from "./UserSort"
import Pagination from "../common/Pagination"
import Search from "../common/Search"
import Loading from "../common/Loading"
import UserAdminCardContainer from "../../containers/user/UserAdminCardContainer"

interface UserListProps {
	users?: User[]
	loading: boolean
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
			{loading ? (
				<Loading />
			) : users.length > 0 ? (
				users.map((user) => (
					<UserAdminCardContainer
						key={user.id}
						user={user}
					/>
				))
			) : (
				<p>No users found.</p>
			)}

			<hr />

			<Pagination
				pagination={pagination}
				onPageChange={onPageChange}
			/>
		</div>
	)
}

export default UserList
