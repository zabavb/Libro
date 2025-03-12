import { useDispatch } from "react-redux";
import { Order } from "../../types";
import { AppDispatch, removeOrder } from "../../state/redux";
import { useSelector } from "react-redux";
import { RootState } from "../../state/redux/store";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { resetOrderOperationStatus } from "../../state/redux/slices/orderSlice";
import OrderAdminCard from "../../components/order/admin/OrderAdminCard";
import { OrderToOrderView } from "../../api/adapters/orderAdapters";

interface OrderAdminCardContainerProps {
    order: Order
}

const OrderAdminCardContainter: React.FC<OrderAdminCardContainerProps> = ({ order }) => {
    const dispatch = useDispatch<AppDispatch>()
    const { operationStatus, error} = useSelector((state: RootState) => state.orders)
    const orderView = OrderToOrderView(order)
    const navigate = useNavigate()

    const handleDelete = (e: React.MouseEvent) => {
        e.stopPropagation()
        dispatch(removeOrder(order.id))
    }

    const handleNavigate = () => {
        navigate(`/admin/orders/${order.id}`)
    }

    useEffect(() => {
        if (operationStatus === "success") {
            alert("Order removed successfully!")
            dispatch(resetOrderOperationStatus())
        }else if (operationStatus === "error"){
            alert(error)
            dispatch(resetOrderOperationStatus())
        }
    }, [operationStatus, error, dispatch])

    return (
        <OrderAdminCard
            order={orderView}
            onDelete={handleDelete}
            onNavigate={handleNavigate}/>
    )
}

export default OrderAdminCardContainter