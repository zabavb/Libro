import { useDispatch } from "react-redux"
import { addOrder, AppDispatch, editOrder, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useNavigate } from "react-router-dom"
import { Order } from "../../types"
import React, { useEffect } from "react"
import { resetOrderOperationStatus } from "../../state/redux/slices/orderSlice"
import OrderForm from "../../components/order/admin/OrderForm"


interface OrderFormContainerProps {
	orderId: string
}

const OrderFormContainer: React.FC<OrderFormContainerProps> = ({ orderId }) => {
    const dispatch = useDispatch<AppDispatch>()
    const {data: orders, operationStatus, error } = useSelector((state: RootState) => state.orders)

    const existingOrder = orders.find((order) => order.id == orderId) ?? undefined

    const navigate = useNavigate()

    const handleAddOrder = (order: Order) => {
        console.log("Adding order:", order)
        dispatch(addOrder(order))
    }

    const handleEditOrder = (id: string, order: Order) => {
        console.log("Editing order:", id, order)
        dispatch(editOrder({id,order}))
    }

    useEffect(() => {
        if (operationStatus === "success"){
            alert(existingOrder ? "Order updated successfully!" : "Order Created successfully!")
            dispatch(resetOrderOperationStatus())
            navigate("/admin/orders")
        } else if (operationStatus === "error") {
            alert(error)
            dispatch(resetOrderOperationStatus())
        }
    }, [operationStatus, existingOrder, error, dispatch, navigate])

    return (
        <OrderForm
            existingOrder={existingOrder}
            onAddOrder={handleAddOrder}
            onEditOrder={handleEditOrder}
        />
    )

}

export default OrderFormContainer
