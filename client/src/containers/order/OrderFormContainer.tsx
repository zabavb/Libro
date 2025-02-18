import { useDispatch } from "react-redux"
import { addOrder, AppDispatch, editOrder, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useNavigate } from "react-router-dom"
import { Order } from "../../types"
import React, { useEffect } from "react"
import { resetOrderOperationStatus } from "../../state/redux/slices/orderSlice"
import OrderForm from "../../components/order/admin/OrderForm"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { fetchDeliveryTypes } from "../../state/redux/slices/deliveryTypeSlice"



interface OrderFormContainerProps {
	orderId: string
}

const OrderFormContainer: React.FC<OrderFormContainerProps> = ({ orderId }) => {
    const dispatch = useDispatch<AppDispatch>()
    const {data: orders, operationStatus, error } = useSelector((state: RootState) => state.orders)
    const {data: deliveryTypes} = useSelector((state: RootState) => state.deliveryTypes)
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
        // Forcing delivery types to be loaded into Store if they weren't
        if(deliveryTypes.length === 0){
            dispatch(fetchDeliveryTypes({pageNumber:1,pageSize:10}))
        }
        if (operationStatus === "success"){
            dispatch(
                addNotification({
                    message: existingOrder ? "Order updated successfully!" : "Order created successfully!",
                    type: "success"
                })
            )
            dispatch(resetOrderOperationStatus())
            navigate("/admin/orders")
        } else if (operationStatus === "error") {
            dispatch(
                addNotification({
                    message: error,
                    type: "error",
                })
            )
            dispatch(resetOrderOperationStatus())
        }
    }, [operationStatus, existingOrder, error, dispatch, navigate, deliveryTypes])

    return (
        <OrderForm
            deliveryTypes={deliveryTypes}
            existingOrder={existingOrder}
            onAddOrder={handleAddOrder}
            onEditOrder={handleEditOrder}
        />
    )

}

export default OrderFormContainer
