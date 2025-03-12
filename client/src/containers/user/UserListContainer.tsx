import { useEffect, useCallback } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState, AppDispatch, fetchUsers } from "../../state/redux/index"
import { useNavigate } from "react-router-dom"
import UserList from "../../components/user/UserList"
import { UserFilter, UserSort } from "../../types"
import { setFilters, setSearchTerm, setSort } from "../../state/redux/slices/userSlice"
import { addNotification } from "../../state/redux/slices/notificationSlice"

const UserListContainer = () => {
	const dispatch = useDispatch<AppDispatch>()
	const {
		data: users,
		operationStatus,
		loading,
		error,
		pagination,
		searchTerm,
		filters,
		sort,
	} = useSelector((state: RootState) => state.users)
	const navigate = useNavigate()

	const fetchUserList = useCallback(() => {
		dispatch(
			fetchUsers({
				pageNumber: pagination.pageNumber,
				pageSize: pagination.pageSize,
				searchTerm,
				filters,
				sort,
			})
		)
	}, [dispatch, pagination.pageNumber, pagination.pageSize, searchTerm, filters, sort])

	useEffect(() => {
		fetchUserList()

		if (operationStatus === "error")
			dispatch(
				addNotification({
					message: error,
					type: "error",
				})
			)
	}, [fetchUserList, dispatch, operationStatus, error])

	const handleNavigate = (path: string) => {
		navigate(path)
	}

	const handleSearchTermChange = (newSearchTerm: string) => {
		dispatch(setSearchTerm(newSearchTerm))
		dispatch(
			fetchUsers({
				pageNumber: 1,
				pageSize: pagination.pageSize,
				searchTerm: newSearchTerm,
				filters,
				sort,
			})
		)
	}

	const handleFilterChange = (newFilters: UserFilter) => {
		dispatch(setFilters(newFilters))
		dispatch(
			fetchUsers({
				pageNumber: 1,
				pageSize: pagination.pageSize,
				searchTerm,
				filters: newFilters,
				sort,
			})
		)
	}

	const handleSortChange = (field: keyof UserSort) => {
		dispatch(setSort(field))
		dispatch(
			fetchUsers({
				pageNumber: 1,
				pageSize: pagination.pageSize,
				searchTerm,
				filters,
				sort: { [field]: true },
			})
		)
	}

	const handlePageChange = (pageNumber: number) => {
		dispatch(fetchUsers({ pageNumber, pageSize: pagination.pageSize, filters, sort }))
	}

	return (
		<UserList
			users={users}
			loading={loading}
			pagination={pagination}
			onPageChange={handlePageChange}
			onNavigate={handleNavigate}
			onSearchTermChange={handleSearchTermChange}
			searchTerm={searchTerm}
			onFilterChange={handleFilterChange}
			filters={filters}
			onSortChange={handleSortChange}
			sort={sort}
		/>
	)
}

export default UserListContainer
