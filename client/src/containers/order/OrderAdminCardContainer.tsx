import { useDispatch } from "react-redux";
import { Order } from "../../types";
import { AppDispatch } from "../../state/redux";
import { useNavigate } from "react-router-dom";
import OrderAdminCard from "../../components/order/admin/OrderAdminCard";
import { OrderToOrderView } from "../../api/adapters/orderAdapters";
import { removeOrderService } from "../../services";
import { addNotification } from "../../state/redux/slices/notificationSlice";

interface OrderAdminCardContainerProps {
    order: Order
}

const OrderAdminCardContainter: React.FC<OrderAdminCardContainerProps> = ({ order }) => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()

    const orderView = OrderToOrderView(order)


    const handleDelete = async (e: React.MouseEvent) => {
        e.stopPropagation()
        const response = await removeOrderService(order.id)
        dispatch(
              response.error
                ? addNotification({
                    message: response.error,
                    type: 'error',
                  })
                : addNotification({
                    message: 'Order successfully deleted.',
                    type: 'success',
                  }),
            );
    }

    const handleNavigate = () => {
        navigate(`/admin/orders/${order.id}`)
    }

    return (
        <OrderAdminCard
            order={orderView}
            onDelete={handleDelete}
            onNavigate={handleNavigate}/>
    )
}

export default OrderAdminCardContainter