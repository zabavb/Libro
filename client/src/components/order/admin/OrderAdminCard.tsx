import React from "react";
import { OrderView } from "../../../types/types/order/OrderView";

interface OrderAdminCardProps {
    order: OrderView
    onDelete: (e: React.MouseEvent) => void
    onNavigate: () => void
}

function cutFloat(num: number): number {
    const parts = num.toString().split('.');
    if (parts.length === 1) return num; // no decimal part
    const decimal = parts[1].slice(0, 3);
    return parseFloat(`${parts[0]}.${decimal}`);
  }

const OrderAdminCard: React.FC<OrderAdminCardProps> = ({ order, onDelete, onNavigate }) => {
    const orderUid = order.id.split('-')[4];
    return (
        <>
            <link rel="stylesheet" href="/src/styles/statusDisplay.css"/>
            <tr
                onClick={(e) => {
                    e.stopPropagation()
                    onNavigate()
                }} style={{textAlign:"center"}}>
                <td>
                    (to be implemented)
                </td>
                <td>
                    {orderUid}
                </td>
                <td>
                    {cutFloat(order.price + order.deliveryPrice)}
                </td>
                <td>
                    <div className={`status ${order.status.toLowerCase()}`}>{order.status}</div>
                </td>
                <td>
                    <button
                        onClick={(e) => {
                            e.stopPropagation()
                            onDelete(e)
                        }}>
                        <img width="25" height="25" src="https://img.icons8.com/ios-filled/50/trash.png" alt="delete"/>
                    </button>
                </td>
            </tr>
        </>
    )
}

export default OrderAdminCard