import { useDispatch } from "react-redux";
import { AppDispatch } from "../../state/redux";
import { useNavigate } from "react-router-dom";
import OrderAdminCard from "../../components/order/admin/OrderAdminCard";
import { removeOrderService } from "../../services";
import { addNotification } from "../../state/redux/slices/notificationSlice";
import { OrderWithUserName } from "@/types/types/order/OrderWithUserName";

interface OrderAdminCardContainerProps {
    order: OrderWithUserName
}

const OrderAdminCardContainer: React.FC<OrderAdminCardContainerProps> = ({ order }) => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()


    const handleDelete = async (e: React.MouseEvent) => {
        e.stopPropagation()
        const response = await removeOrderService(order.orderUiId)
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
        navigate(`/admin/orders/${order.orderUiId}`)
    }

    return (
        <OrderAdminCard
            order={order}
            onDelete={handleDelete}
            onNavigate={handleNavigate}/>
    )
}

export default OrderAdminCardContainer