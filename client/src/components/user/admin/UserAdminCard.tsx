import React from "react"
import { UserView } from "../../../types"

interface UserAdminCardProps {
	user: UserView
	onDelete: (e: React.MouseEvent) => void
	onNavigate: () => void
}

const UserAdminCard: React.FC<UserAdminCardProps> = ({ user, onDelete, onNavigate }) => {
	return (
		<>
			<hr />
			<li
				onClick={(e) => {
					e.stopPropagation()
					onNavigate()
				}}>
				<div>
					<p>
						<strong>First Name:</strong> {user.firstName}
					</p>
					<p>
						<strong>Last Name:</strong> {user.lastName}
					</p>
					<p>
						<strong>Date of Birth:</strong> {user.dateOfBirth}
					</p>
					<p>
						<strong>Email:</strong> {user.email}
					</p>
					<p>
						<strong>Phone Number:</strong> {user.phoneNumber}
					</p>
					<p>
						<strong>Role:</strong> {user.role}
					</p>
				</div>
				<button
					onClick={(e) => {
						e.stopPropagation()
						onDelete(e)
					}}>
					Delete
				</button>
			</li>
		</>
	)
}

export default UserAdminCard
