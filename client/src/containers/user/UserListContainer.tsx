import { useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState, AppDispatch, fetchUsers } from "../../state/redux/index"
import { useNavigate } from "react-router-dom"
import UserList from "../../components/user/UserList"

const UserListContainer = () => {
	const dispatch = useDispatch<AppDispatch>()
	const { data: users, loading, error, pagination } = useSelector((state: RootState) => state.users)
	const navigate = useNavigate()

	useEffect(() => {
		dispatch(fetchUsers({ pageNumber: pagination.pageNumber, pageSize: pagination.pageSize }))
	}, [dispatch, pagination.pageNumber, pagination.pageSize])

	const handleNavigate = (path: string) => {
		navigate(path)
	}

	const handlePageChange = (pageNumber: number) => {
		dispatch(fetchUsers({ pageNumber, pageSize: pagination.pageSize }))
	}

	return (
		<UserList
			users={users}
			loading={loading}
			error={error}
			pagination={pagination}
			onPageChange={handlePageChange}
			onNavigate={handleNavigate}
		/>
	)
}

export default UserListContainer
