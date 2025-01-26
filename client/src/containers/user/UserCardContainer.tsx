import React, { useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState } from "../../state/redux/store"
import { useNavigate } from "react-router-dom"
import { AppDispatch, removeUser } from "../../state/redux/index"
import UserCard from "../../components/user/UserCard"
import { resetOperationStatus } from "../../state/redux/slices/userSlice"
import { User } from "../../types"

interface UserCardContainerProps {
	user: User
}

const UserCardContainer: React.FC<UserCardContainerProps> = ({ user }) => {
	const dispatch = useDispatch<AppDispatch>()
	const navigate = useNavigate()
	const { operationStatus, error } = useSelector((state: RootState) => state.users)
	const formattedDateOfBirth = user.dateOfBirth.split("T")[0]

	const handleDelete = (e: React.MouseEvent) => {
		e.stopPropagation()
		dispatch(removeUser(user.id))
	}

	const handleNavigate = () => {
		navigate(`/admin/users/${user.id}`)
	}

	useEffect(() => {
		if (operationStatus === "success") {
			alert("User removed successfully!")
			dispatch(resetOperationStatus())
		} else if (operationStatus === "error") {
			alert(error)
			dispatch(resetOperationStatus())
		}
	}, [operationStatus, error, dispatch])

	return (
		<UserCard
			user={{ ...user, dateOfBirth: formattedDateOfBirth }}
			onDelete={handleDelete}
			onNavigate={handleNavigate}
		/>
	)
}

export default UserCardContainer
