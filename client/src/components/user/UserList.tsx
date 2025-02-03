import React from "react"
import UserCardContainer from "../../containers/user/UserCardContainer"
import { User } from "../../types"

interface UserListProps {
	users?: User[]
	loading: boolean
	error: string | null | undefined
	pagination: { pageNumber: number; pageSize: number; totalCount: number }
	onPageChange: (pageNumber: number) => void
	onNavigate: (path: string) => void
}

const UserList: React.FC<UserListProps> = ({
	users = [],
	loading,
	error,
	pagination,
	onPageChange,
	onNavigate,
}) => {
	if (loading) return <p>Loading...</p>
	if (error) return <p>Error: {error}</p>

	const totalPages = Math.ceil(pagination.totalCount / pagination.pageSize)

	return (
		<div>
			<p onClick={() => onNavigate("/admin")}>Back to Admin Dashboard</p>
			<p onClick={() => onNavigate("/admin/users/add")}>Add User</p>
			<h1>User List</h1>
			<div>
				{users.map((user) => (
					<UserCardContainer
						key={user.id}
						user={user}
					/>
				))}
			</div>
			<hr />
			<div>
				<span>Pages: </span>
				{pagination.pageNumber > 1 && (
					<button onClick={() => onPageChange(pagination.pageNumber - 1)}>Previous</button>
				)}
				<span>{pagination.pageNumber} </span>
				{pagination.pageNumber < totalPages && (
					<button onClick={() => onPageChange(pagination.pageNumber + 1)}>Next</button>
				)}
			</div>
		</div>
	)
}

export default UserList
