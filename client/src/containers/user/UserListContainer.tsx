import { useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState, AppDispatch, fetchUsers } from "../../state/redux/index"
import { useNavigate } from "react-router-dom"
import UserList from "../../components/user/UserList"
import { UserFilter, UserSort } from "../../types"
import { setFilters, setSearchTerm, setSort } from "../../state/redux/slices/userSlice"

const UserListContainer = () => {
	const dispatch = useDispatch<AppDispatch>()
	const {
		data: users,
		loading,
		error,
		pagination,
		searchTerm,
		filters,
		sort,
	} = useSelector((state: RootState) => state.users)
	const navigate = useNavigate()

	useEffect(() => {
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
			error={error}
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
