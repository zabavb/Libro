import { useDispatch, useSelector } from "react-redux"
import Register from "../../components/auth/Register"
import { AppDispatch, RootState } from "../../state/redux"
import { useNavigate } from "react-router-dom"
import { useCallback, useEffect } from "react"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { resetOperationStatus } from "../../state/redux/slices/userSlice"

const RegisterContainer: React.FC = () => {
	const dispatch = useDispatch<AppDispatch>()
	const { operationStatus, error } = useSelector((state: RootState) => state.users)
	const navigate = useNavigate()

	const handleSuccess = useCallback(() => {
		dispatch(
			addNotification({
				message: "Registration successful!",
				type: "success",
			})
		)
		dispatch(resetOperationStatus())
		navigate("/login")
	}, [dispatch, navigate])

	const handleError = useCallback(() => {
		dispatch(addNotification({ message: error, type: "error" }))
		dispatch(resetOperationStatus())
	}, [dispatch, error])

	useEffect(() => {
		if (operationStatus === "success") handleSuccess()
		else if (operationStatus === "error") handleError()
	}, [operationStatus, handleSuccess, handleError])

	return <Register />
}

export default RegisterContainer
