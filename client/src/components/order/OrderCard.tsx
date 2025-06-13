import React from "react";
import { OrderView } from "../../types/types/order/OrderView";
import '@/assets/styles/components/order/order-card.css'
import { OrderDetails } from "@/types/types/order/OrderDetails";
import { dateToString } from "@/api/adapters/commonAdapters";
import { statusEnumToStatusView } from "@/api/adapters/orderAdapters";
interface OrderCardProps {
    order: OrderDetails
}



const OrderCard: React.FC<OrderCardProps> = ({ order }) => {
    const orderUid = order.orderId.split('-')[4];
    return (
        <div className="order-card-container">
            <div className="flex justify-between font-semibold">
                <p>№{orderUid}</p>
                <p className="text-[#9C9C97]">{dateToString(order.created)}</p>
                <p>{statusEnumToStatusView(order.status)}</p>
            </div>
            <div className="flex flex-col gap-2">
                {order.orderBooks.map((book) => (
                    <div className="flex justify-between items-center">
                        <div className="flex gap-3.5">
                            <img alt="BOOKIMAGE" className="w-[45px] h-[70px]" src={book.imageUrl} />
                            <div className="flex flex-col">
                                <p>{book.title}</p>
                                <p className="text-[#929089]">{book.authorName}</p>
                                <div className="flex gap-1">
                                    <p>{book.price.toFixed(2)} UAH</p>
                                    <p className="text-[#9C9C97]">-</p>
                                    <p className="text-[#9C9C97]">COVER</p>
                                    <p className="text-[#9C9C97]">-</p>
                                    <p className="text-[#9C9C97]">{book.bookId.split('-')[4]}</p>
                                </div>
                            </div>
                        </div>
                        <p className="text-[#9C9C97]">
                            {book.amount} pcs.
                        </p>
                    </div>
                ))}
                <div className="flex justify-between">
                    <p className="text-lg text-[#FF642E] font-bold">
                        Total {order.price.toFixed(2)} UAH
                    </p>
                    <p className="text-[#9C9C97]">More details</p>
                </div>
                {/* <button>Repeat order</button> */}
            </div>
        </div>
    )
}

export default OrderCard