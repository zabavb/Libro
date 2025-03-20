import { useDispatch } from "react-redux";
import { Order } from "../../types";
import { AppDispatch } from "../../state/redux";
import { useSelector } from "react-redux";
import { RootState } from "../../state/redux/store";
import { useEffect } from "react";
import { resetOrderOperationStatus } from "../../state/redux/slices/orderSlice";
import { OrderToOrderView } from "../../api/adapters/orderAdapters";
import OrderCard from "../../components/order/OrderCard";

interface OrderCardContainerProps {
    order: Order
}

const OrderCardContainter: React.FC<OrderCardContainerProps> = ({ order }) => {
    const dispatch = useDispatch<AppDispatch>()
    const { operationStatus, error} = useSelector((state: RootState) => state.orders)
    const orderView = OrderToOrderView(order)

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
        <OrderCard
            order={orderView}/>
    )
}

export default OrderCardContainter