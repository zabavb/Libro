import { useNavigate } from "react-router-dom"
import { AppDispatch, fetchOrders, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useDispatch } from "react-redux"
import React, { useEffect } from "react"
import { OrderFilter, OrderSort } from "../../types"
import { setOrderFilters, setOrderSearchTerm, setOrderSort } from "../../state/redux/slices/orderSlice"
import UserOrdersList from "../../components/user/userOrderList/UserOrdersList"

interface UserOrdersContainerProps {
    userId: string
}

const UserOrdersContainer: React.FC<UserOrdersContainerProps> = ({ userId }) => {
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

    useEffect(() => {
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
    }, [dispatch, pagination.pageNumber, pagination.pageSize, searchTerm, filters, sort, userId])

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