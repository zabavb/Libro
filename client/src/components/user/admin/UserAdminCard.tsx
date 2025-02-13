import React from "react"
import { UserView } from "../../../types"
import { LazyLoadImage } from "react-lazy-load-image-component"
import "react-lazy-load-image-component/src/effects/blur.css"

interface UserAdminCardProps {
	user: UserView
	onNavigate: () => void
	onDelete: (e: React.MouseEvent) => void
}

const UserAdminCard: React.FC<UserAdminCardProps> = ({ user, onNavigate, onDelete }) => {
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
						<LazyLoadImage
							src={user.imageUrl}
							alt={`${user.firstName} ${user.lastName}`}
							effect="opacity"
							height="100"
							width="100"
						/>
					</p>
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
