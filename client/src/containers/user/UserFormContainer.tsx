import React, { useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { addUser, AppDispatch, editUser } from "../../state/redux/index"
import { RootState } from "../../state/redux/store"
import { useNavigate } from "react-router-dom"
import UserForm from "../../components/user/UserForm"
import { User } from "../../types"
import { resetOperationStatus } from "../../state/redux/slices/userSlice"

interface UserFormContainerProps {
	userId: string
}

const UserFormContainer: React.FC<UserFormContainerProps> = ({ userId }) => {
	const dispatch = useDispatch<AppDispatch>()
	const { data: users, operationStatus, error } = useSelector((state: RootState) => state.users)

	const existingUser = users.find((user) => user.id === userId) ?? undefined

	const navigate = useNavigate()

	const handleAddUser = (user: User) => {
		console.log("Adding user:", user)
		dispatch(addUser(user))
	}

	const handleEditUser = (id: string, user: User) => {
		console.log("Editing user:", id, user)
		dispatch(editUser({ id, user }))
	}

	useEffect(() => {
		if (operationStatus === "success") {
			alert(existingUser ? "User updated successfully!" : "User created successfully!")
			dispatch(resetOperationStatus())
			navigate("/admin/users")
		} else if (operationStatus === "error") {
			alert(error)
			dispatch(resetOperationStatus())
		}
	}, [operationStatus, existingUser, error, dispatch, navigate])

	return (
		<UserForm
			existingUser={existingUser}
			onAddUser={handleAddUser}
			onEditUser={handleEditUser}
		/>
	)
}

export default UserFormContainer
