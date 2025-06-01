import React from "react";
import "@/assets/styles/base/status-display.css"
import {icons} from '@/lib/icons'
import { OrderWithUserName } from "@/types/types/order/OrderWithUserName";
import { statusEnumToStatusView } from "@/api/adapters/orderAdapters";
interface OrderAdminCardProps {
    order: OrderWithUserName
    onDelete: (e: React.MouseEvent) => void
    onNavigate: () => void
}

const OrderAdminCard: React.FC<OrderAdminCardProps> = ({ order, onDelete, onNavigate }) => {
    const orderUid = order.orderUiId.split('-')[4];
    return (
        <>
            <tr style={{textAlign:"center"}}>
                <td>
                    {order.firstName} {order.lastName}
                </td>
                <td>
                    {orderUid}...
                </td>
                <td>
                    {order.price.toFixed(2)} ₴
                </td>
                <td>
                    <div className={`status ${order.status.toLowerCase()}`}>{statusEnumToStatusView(order.status)}</div>
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