import { useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState, AppDispatch, fetchUsers } from "../../state/redux/index"
import { useNavigate } from "react-router-dom"
import UserList from "../../components/user/UserList"
import { UserFilter, UserSort } from "../../types"
import { setFilters, setSort } from "../../state/redux/slices/userSlice"

const UserListContainer = () => {
	const dispatch = useDispatch<AppDispatch>()
	const {
		data: users,
		loading,
		error,
		pagination,
		filters,
		sort,
	} = useSelector((state: RootState) => state.users)
	const navigate = useNavigate()

	useEffect(() => {
		dispatch(
			fetchUsers({
				pageNumber: pagination.pageNumber,
				pageSize: pagination.pageSize,
				filters,
				sort,
			})
		)
	}, [dispatch, pagination.pageNumber, pagination.pageSize, filters, sort])

	const handleNavigate = (path: string) => {
		navigate(path)
	}

	const handleFilterChange = (newFilters: UserFilter) => {
		dispatch(setFilters(newFilters))
		dispatch(fetchUsers({ pageNumber: 1, pageSize: pagination.pageSize, filters: newFilters, sort }))
	}

	const handleSortChange = (field: keyof UserSort) => {
		dispatch(setSort(field))
		dispatch(
			fetchUsers({ pageNumber: 1, pageSize: pagination.pageSize, filters, sort: { [field]: true } })
		)
	}

	const handlePageChange = (pageNumber: number) => {
		dispatch(fetchUsers({ pageNumber, pageSize: pagination.pageSize, filters, sort }))
	}

	return (
		<UserList
			users={users}
			loading={loading}
			error={error}
			pagination={pagination}
			onPageChange={handlePageChange}
			onNavigate={handleNavigate}
			onFilterChange={handleFilterChange}
			onSortChange={handleSortChange}
			filters={filters}
			sort={sort}
		/>
	)
}

export default UserListContainer
