import { useDispatch, useSelector } from "react-redux"
import { AppDispatch, RootState } from "../../state/redux"
import { useNavigate } from "react-router-dom"
import { useEffect, useCallback } from "react"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { resetOperationStatus } from "../../state/redux/slices/userSlice"
import Login from "../../components/auth/Login"

const LoginContainer: React.FC = () => {
	const dispatch = useDispatch<AppDispatch>()
	const { operationStatus, error } = useSelector((state: RootState) => state.users)
	const navigate = useNavigate()

	const handleSuccess = useCallback(() => {
		dispatch(
			addNotification({
				message: "Login successful!",
				type: "success",
			})
		)
		dispatch(resetOperationStatus())
		navigate("/")
	}, [dispatch, navigate])

	const handleError = useCallback(() => {
		dispatch(addNotification({ message: error, type: "error" }))
		dispatch(resetOperationStatus())
	}, [dispatch, error])

	useEffect(() => {
		if (operationStatus === "success") handleSuccess()
		else if (operationStatus === "error") handleError()
	}, [operationStatus, handleSuccess, handleError])

	return <Login />
}

export default LoginContainer
