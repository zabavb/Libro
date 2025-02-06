import React, { useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState } from "../../state/redux/store"
import { useNavigate } from "react-router-dom"
import { AppDispatch, removeUser } from "../../state/redux/index"
import { resetOperationStatus } from "../../state/redux/slices/userSlice"
import { User } from "../../types"
import { UserToUserView } from "../../api/adapters/userAdapter"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import UserRow from "../../components/user/admin/UserAdminCard"

interface UserAdminCardContainerProps {
	user: User
}

const UserAdminCardContainer: React.FC<UserAdminCardContainerProps> = ({ user }) => {
	const dispatch = useDispatch<AppDispatch>()
	const { operationStatus, error } = useSelector((state: RootState) => state.users)
	const userView = UserToUserView(user)
	const navigate = useNavigate()

	const handleDelete = (e: React.MouseEvent) => {
		e.stopPropagation()
		dispatch(removeUser(user.id))
	}

	const handleNavigate = () => {
		navigate(`/admin/users/${user.id}`)
	}

	useEffect(() => {
		if (operationStatus === "success") {
			dispatch(
				addNotification({
					message: "User successfully deleted.",
					type: "success",
				})
			)
			dispatch(resetOperationStatus())
		} else if (operationStatus === "error") {
			dispatch(
				addNotification({
					message: error,
					type: "error",
				})
			)
			dispatch(resetOperationStatus())
		}
	}, [operationStatus, error, dispatch])

	return (
		<UserRow
			user={userView}
			onDelete={handleDelete}
			onNavigate={handleNavigate}
		/>
	)
}

export default UserAdminCardContainer
