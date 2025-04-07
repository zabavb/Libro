import React from "react";
import { OrderCard } from "@/types/types/order/OrderCard";
import { dateToString } from "@/api/adapters/commonAdapters";

interface OrderAdminCardProps {
    order: OrderCard
    onDelete: (e: React.MouseEvent ) => void
    onNavigate: () => void
}

const OrderAdminCard: React.FC<OrderAdminCardProps> = ({order, onDelete, onNavigate}) => {
    console.log(order.books)
    return(
        <>
            <hr/>
            <li
                onClick={(e) => {
                    e.stopPropagation()
                    onNavigate()
                }}>
                <div>
                    <div>
                        <strong>Books:</strong>
                        {order.books != null ?  order.books.items.map(book => (
                            <p>{book.title}</p>
                        )) : (<p>Failed to fetch books</p>)}
                    </div>
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
                        <strong>Price:</strong> {order.fullPrice}
                    </p>
                    <p>
                        <strong>Order Date:</strong> {dateToString(order.orderDate)}
                    </p>
                    <p>
                        <strong>Delivery Date:</strong> {dateToString(order.deliveryDate)}
                    </p>
                    <p>
                        <strong>Status:</strong> {order.status}
                    </p>
                    <p>
                        <strong>Delivery Type:</strong>  {order.delivery.item.serviceName}
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