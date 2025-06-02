import { Order } from "../../types";
import { OrderToOrderView } from "../../api/adapters/orderAdapters";
import OrderCard from "../../components/order/OrderCard";
import { OrderDetails } from "@/types/types/order/OrderDetails";

interface OrderCardContainerProps {
    order: OrderDetails
}

const OrderCardContainter: React.FC<OrderCardContainerProps> = ({ order }) => {

    return (
        <OrderCard
            order={order}/>
    )
}

export default OrderCardContainter