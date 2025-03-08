import { useDispatch } from "react-redux"
import { AppDispatch, fetchDeliveryTypes, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useNavigate } from "react-router-dom"
import { useEffect } from "react"
import DeliveryTypeList from "../../components/order/DeliveryList"
import { setDeliverySearchTerm, setDeliverySort } from "../../state/redux/slices/deliveryTypeSlice"
import { DeliverySort } from "../../types"

const DeliveryTypeListContainer = () => {
    const dispatch = useDispatch<AppDispatch>()
    const {
        data: deliveryTypes,
        loading,
        error,
        pagination,
        searchTerm,
        sort
    } = useSelector((state: RootState) => state.deliveryTypes)
    const navigate = useNavigate()

    useEffect(() => {
        dispatch(
            fetchDeliveryTypes({
                pageNumber: pagination.pageNumber,
                pageSize: pagination.pageSize,
                searchTerm,
                sort
            })
        )
    }, [dispatch, pagination.pageNumber, pagination.pageSize, searchTerm, sort])

    const handleNavigate = (path: string) => {
        navigate(path)
    }

    const handlePageChange = (pageNumber: number) => {
        dispatch(fetchDeliveryTypes({pageNumber, pageSize: pagination.pageSize}))
    }

    const handleSearchTermChange = (newSearchTerm: string) => {
        dispatch(setDeliverySearchTerm(newSearchTerm))
        dispatch(
            fetchDeliveryTypes({
                pageNumber: 1,
                pageSize: pagination.pageSize,
                searchTerm: newSearchTerm,
                sort
            })
        )
    }

    const handleSortChange = (field: keyof DeliverySort) => {
        dispatch(setDeliverySort(field))
        dispatch(
            fetchDeliveryTypes({
                pageNumber: 1,
                pageSize: pagination.pageSize,
                searchTerm,
                sort: { [field ]: true}
            })
        )
    }

    return (
        <DeliveryTypeList
        deliveryTypes={deliveryTypes}
        loading={loading}
        error={error}
        pagination={pagination}
        onPageChange={handlePageChange}
        onNavigate={handleNavigate}
        onSearchTermChange={handleSearchTermChange}
        searchTerm={searchTerm}
        onSortChange={handleSortChange}
        sort={sort}
        />
    )
}

export default DeliveryTypeListContainer