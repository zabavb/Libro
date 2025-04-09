import { Order } from "../../types";
import { OrderToOrderView } from "../../api/adapters/orderAdapters";
import OrderCard from "../../components/order/OrderCard";

interface OrderCardContainerProps {
    order: Order
}

const OrderCardContainter: React.FC<OrderCardContainerProps> = ({ order }) => {
    const orderView = OrderToOrderView(order)

    return (
        <OrderCard
            order={orderView}/>
    )
}

export default OrderCardContainter