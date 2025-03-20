import { useNavigate } from "react-router-dom"
import { AppDispatch, fetchOrders, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useDispatch } from "react-redux"
import { useEffect, useState } from "react"
import { OrderFilter, OrderSort } from "../../types"
import { setOrderFilters, setOrderSearchTerm, setOrderSort } from "../../state/redux/slices/orderSlice"
import UserOrdersList from "../../components/user/userOrderList/UserOrdersList"

const UserOrdersContainer = () => {
    const dispatch = useDispatch<AppDispatch>()
    const {
        data: orders,
        loading,
        error,
        pagination,
        searchTerm,
        filters,
        sort
    } = useSelector((state: RootState) => state.orders)
    const navigate = useNavigate()
    const [userId, setUserId] = useState<string>("");


    useEffect(() => {
        const json = localStorage.getItem('user');
        if(json != null){
            // add some verification in the future
            const user = JSON.parse(json)
            setUserId(user.id)
        }
        else{
            navigate('/login')
        }

        const filter: OrderFilter = {userId:userId};

        dispatch(
            fetchOrders({
                pageNumber: pagination.pageNumber,
                pageSize: pagination.pageSize,
                searchTerm,
                filters: filter,
                sort,
            })
        )
    }, [dispatch, pagination.pageNumber, pagination.pageSize, searchTerm, filters, sort,navigate,userId])

    const handleNavigate = (path: string) => {
        navigate(path)
    }

    const handleSearchTermChange = (newSearchTerm: string) => {
        dispatch(setOrderSearchTerm(newSearchTerm))
        dispatch(
            fetchOrders({
                pageNumber: 1,
                pageSize: pagination.pageSize,
                searchTerm: newSearchTerm,
                filters,
                sort
            })
        )
    }


    const handleFilterChange = (newFilters: OrderFilter) => {
        dispatch(setOrderFilters(newFilters))
        dispatch(
            fetchOrders({
                pageNumber: 1,
                pageSize: pagination.pageSize,
                searchTerm,
                filters:newFilters,
                sort
            })
        )
    }

    const handleSortChange = (field: keyof OrderSort) => {
        dispatch(setOrderSort(field))
        dispatch(
            fetchOrders({
                pageNumber: 1,
                pageSize: pagination.pageSize,
                searchTerm,
                filters,
                sort: { [field]: true}
            })
        )
    }

    const handlePageChange = (pageNumber: number) => {
        dispatch(fetchOrders({ pageNumber, pageSize: pagination.pageSize, filters, sort }))
    }

    return (
        <UserOrdersList
            orders={orders}
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

export default UserOrdersContainer