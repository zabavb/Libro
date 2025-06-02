import React from "react";
import { OrderView } from "../../types/types/order/OrderView";
import '@/assets/styles/components/order/order-card.css'
interface OrderCardProps {
    order: OrderView
}



const OrderCard: React.FC<OrderCardProps> = ({ order }) => {
    const orderUid = order.id.split('-')[4];
    return (
        <div className="order-card-container">
            <div className="flex justify-between font-semibold">
                <p>№{orderUid}</p>
                <p className="text-[#9C9C97]">{order.orderDate}</p>
                <p>{order.status}</p>
            </div>
            <div className="flex flex-col gap-2">
                {Object.entries(order.books).map(([id, amnt]) => (
                    <div className="flex justify-between items-center">
                        <div className="flex gap-3.5">
                            <img alt="BOOKIMAGE" />
                            <div className="flex flex-col">
                                <p>bookName</p>
                                <p className="text-[#929089]">authorName</p>
                                <div className="flex">
                                    <p>999 UAH</p>
                                    <p className="text-[#9C9C97]">COVERTYPE</p>
                                    <p className="text-[#9C9C97]">{id.split('-')[4]}</p>
                                </div>
                            </div>
                        </div>
                        <p className="text-[#9C9C97]">
                            {amnt} pcs.
                        </p>
                    </div>
                ))}
                <div className="flex justify-between">
                    <p className="text-lg text-[#FF642E] font-bold">
                        Total {order.price} UAH
                    </p>
                    <p className="text-[#9C9C97]">More details</p>
                </div>
                <button>Repeat order</button>
            </div>
        </div>
    )
}

export default OrderCard