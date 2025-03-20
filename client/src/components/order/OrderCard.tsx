import React from "react";
import { OrderView } from "../../types/types/order/OrderView";

interface OrderCardProps {
    order: OrderView
}

const OrderCard: React.FC<OrderCardProps> = ({order}) => {
    return(
        <>
            <hr/>
            <li>
                <div>
                    <p>
                        <strong>Books:</strong>
                        {Object.entries(order.books).map(([book, count]) => (
                            <p>{book} (x{count})</p>
                        ))
                    }
                    </p>
                    <p>
                        <strong>Region:</strong> {order.region}
                    </p>
                    <p>
                        <strong>City:</strong> {order.city}
                    </p>
                    <p>
                        <strong>Address:</strong> {order.address}
                    </p>
                    <p>
                        <strong>Price:</strong> {order.price}
                    </p>
                    <p>
                        <strong>Delivery Price:</strong> {order.deliveryPrice}
                    </p>
                    <p>
                        <strong>Order Date:</strong> {order.orderDate}
                    </p>
                    <p>
                        <strong>Delivery Date:</strong> {order.deliveryDate}
                    </p>
                    <p>
                        <strong>Status:</strong> {order.status}
                    </p>
                    <p>
                        <strong>Delivery Type:</strong> 
                    </p>
                </div>
            </li>
        </>
    )
}

export default OrderCard