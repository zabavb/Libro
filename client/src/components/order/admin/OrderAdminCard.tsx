import React from "react";
import { OrderView } from "../../../types/types/order/OrderView";

interface OrderAdminCardProps {
    order: OrderView
    onDelete: (e: React.MouseEvent ) => void
    onNavigate: () => void
}

const OrderAdminCard: React.FC<OrderAdminCardProps> = ({order, onDelete, onNavigate}) => {
    return(
        <>
            <hr/>
            <li
                onClick={(e) => {
                    e.stopPropagation()
                    onNavigate()
                }}>
                <div>
                    {/* To be implemented */}
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
                    {/* To be implemented */}
                    <p>
                        <strong>Delivery Type:</strong> 
                    </p>
                    <button
                        onClick={(e) => {
                            e.stopPropagation()
                            onDelete(e)
                        }}>
                        Delete
                    </button>
                </div>
            </li>
        </>
    )
}

export default OrderAdminCard