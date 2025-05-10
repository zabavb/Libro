import React from "react";
import { OrderView } from "../../../types/types/order/OrderView";
import "@/assets/styles/base/status-display.css"
import {icons} from '@/lib/icons'
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
            <tr
                onClick={(e) => {
                    e.stopPropagation()
                    onNavigate()
                }} style={{textAlign:"center"}}>
                <td>
                    (to be implemented)
                </td>
                <td>
                    {orderUid}...
                </td>
                <td>
                    {cutFloat(order.price + order.deliveryPrice)} ₴
                </td>
                <td>
                    <div className={`status ${order.status.toLowerCase()}`}>{order.status}</div>
                </td>
                <td>
                    <div className='flex gap-2'>
                        <button onClick={onNavigate} className='p-2.5 bg-[#FF642E] rounded-lg'><img src={icons.wPen} /></button>
                        <button onClick={onDelete} className='p-2.5 border-[1px] border-[#FF642E] rounded-lg'><img src={icons.oTrash} /></button>
                    </div>
                </td>
            </tr>
        </>
    )
}

export default OrderAdminCard