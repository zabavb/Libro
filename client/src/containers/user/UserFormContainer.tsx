import React, { useCallback, useEffect, useMemo } from "react"
import { useDispatch, useSelector } from "react-redux"
import { addUser, AppDispatch, editUser } from "../../state/redux/index"
import { RootState } from "../../state/redux/store"
import { useNavigate } from "react-router-dom"
import UserForm from "../../components/user/UserForm"
import { resetOperationStatus } from "../../state/redux/slices/userSlice"
import { addNotification } from "../../state/redux/slices/notificationSlice"

interface UserFormContainerProps {
	userId: string
}

const UserFormContainer: React.FC<UserFormContainerProps> = ({ userId }) => {
	const dispatch = useDispatch<AppDispatch>()
	const { data: users, operationStatus, error } = useSelector((state: RootState) => state.users)
	const navigate = useNavigate()

	const existingUser = useMemo(() => users.find((user) => user.id === userId), [users, userId])
	const handleAddUser = useCallback((user: FormData) => {dispatch(addUser(user))}, [dispatch])
	const handleEditUser = useCallback(
		(id: string, user: FormData) => dispatch(editUser({ id, user })),
		[dispatch]
	)

	useEffect(() => {
		if (operationStatus === "success") {
			dispatch(
				addNotification({
					message: existingUser ? "User updated successfully!" : "User created successfully!",
					type: "success",
				})
			)
			dispatch(resetOperationStatus())
			navigate("/admin/users")
		} else if (operationStatus === "error") {
			dispatch(
				addNotification({
					message: error,
					type: "error",
				})
			)
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
