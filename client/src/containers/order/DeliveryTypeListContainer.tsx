import { useDispatch } from "react-redux"
import { AppDispatch, fetchDeliveryTypes, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useNavigate } from "react-router-dom"
import { useEffect } from "react"
import DeliveryTypeList from "../../components/order/Deliverylist"

const DeliveryTypeListContainer = () => {
    const dispatch = useDispatch<AppDispatch>()
    const {
        data: deliveryTypes,
        loading,
        error,
        pagination
    } = useSelector((state: RootState) => state.deliveryTypes)
    const navigate = useNavigate()

    useEffect(() => {
        dispatch(
            fetchDeliveryTypes({
                pageNumber: pagination.pageNumber,
                pageSize: pagination.pageSize
            })
        )
    }, [dispatch, pagination.pageNumber, pagination.pageSize])

    const handleNavigate = (path: string) => {
        navigate(path)
    }

    const handlePageChange = (pageNumber: number) => {
        dispatch(fetchDeliveryTypes({pageNumber, pageSize: pagination.pageSize}))
    }

    return (
        <DeliveryTypeList
        deliveryTypes={deliveryTypes}
        loading={loading}
        error={error}
        pagination={pagination}
        onPageChange={handlePageChange}
        onNavigate={handleNavigate}
        />
    )
}

export default DeliveryTypeListContainer