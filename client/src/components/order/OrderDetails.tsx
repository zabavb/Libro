import { Order } from "../../types";
import React, { useEffect, useState } from "react";
import OrderFormBookList from "./admin/OrderFormBookList";
import { useNavigate } from "react-router-dom";
import "@/assets/styles/components/order-form.css"
import "@/assets/styles/base/status-display.css"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { statusNumberToEnum } from "@/api/adapters/orderAdapters";
interface OrderDetailsProps {
    existingOrder?: Order;
    loading: boolean;
}

const OrderDetails: React.FC<OrderDetailsProps> = ({ existingOrder }) => {
    const [bookObjs, setBookObjs] = useState<Record<string, number>>({})
    const navigate = useNavigate();

    /* Outsource the function into separate lib */
    function cutFloat(num: number): number {
        const parts = num.toString().split('.');
        if (parts.length === 1) return num; // no decimal part
        const decimal = parts[1].slice(0, 2);
        return parseFloat(`${parts[0]}.${decimal}`);
    }
    
    const fullPrice = cutFloat((existingOrder?.price ?? 0) + (existingOrder?.deliveryPrice ?? 0));

    useEffect(() => {
        if (existingOrder) {
            setBookObjs(existingOrder.books)
        }
    }, [existingOrder])

    return (
        <div>
            <header className="header-container">
                <div className="flex h-10 gap-2.5">
                    <button className="cancel-button" onClick={() => navigate('/admin/orders')}><FontAwesomeIcon icon={faArrowLeft} /></button>
                    <button className="update-button" type="submit">Update</button>
                </div>

                <div className="profile-icon">
                    <div style={{ borderRadius: "50%", backgroundColor: "grey", height: "43px", width: "43px" }}></div>
                    <p className="profile-name">Name Surname</p>
                </div>
            </header>
                <main className="main-container">
                    {/* Left form */}
                    <div className="main-form-container">
                        <h1 className=" font-semibold text-2xl mb-5">Order Details</h1>

                        <div className="input-container">
                            <p className="input-title">Region</p>
                            <p className="input-text">{existingOrder?.region}</p>
                        </div>

                        <div className="input-container">
                            <p className="input-title">City</p>
                            <p className="input-text">{existingOrder?.city}</p>
                        </div>

                        <div className="input-container">
                            <p className="input-title">Address</p>
                            <p className="input=text">{existingOrder?.address}</p>
                        </div>

                        <div className="flex gap-2.5">
                            <div className="input-container">
                                <p className="input-title">Order Date</p>
                                <p className="input-text">{existingOrder?.orderDate.toString()}</p>
                            </div>
                            <div className="input-container">
                                <p className="input-title">Delivery Date</p>
                                <p className="input=text">{existingOrder?.deliveryDate.toString()}</p>
                            </div>
                        </div>

                        <div className="subscription-box">
                            <div className="subscription-logo">
                                <p className="m-0 font-semibold text-center w-full">365</p>
                            </div>
                            <div className="flex flex-col gap-3.5">
                                <p className="text-xl font-semibold">Activated<br />Delivery 365</p>
                                <p className="h-1/2 text-sm font-semibold text-[#9C9C97]">Free delivery for a <br /> year for 365₴</p>
                            </div>

                        </div>
                    </div>
                    <div className="book-form-container">
                        <div className="mb-11">
                            <h1 className=" font-semibold text-2xl mb-1 text-[#FF642E]">Status</h1>
                            <div>
                                <div className="status-form-container">
                                    <p className={`input-select ${statusNumberToEnum(existingOrder?.status ?? 0).toString().toLowerCase()}`}>
                                    {statusNumberToEnum(existingOrder?.status ?? 0)}
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div className="mb-3.5 text-[#FF642E]">
                            <h1 className="font-semibold text-2xl">Order</h1>
                            <p className="font-medium text-sm">№ {existingOrder?.id}</p>
                        </div>
                        <div className="mb-3.5">
                            <div style={{maxHeight:"240px", overflow:"auto"}}>
                                <OrderFormBookList
                                    books={bookObjs}
                                    onBookDelete={() => {}}
                                    onBookAdd={() => {}}
                                />
                            </div>
                        </div>
                        <div className="flex justify-between text-[#FF642E] font-semibold mb-8">
                            <p className="text-xl">Total</p>
                            <p className="text-2xl">{fullPrice}  ₴</p>
                        </div>
                        <div className="flex flex-col gap-2">
                            <h1 className="font-semibold text-[#FF642E] text-2xl">Delivery</h1>
                            <p className="delivery-card">Delivery Name</p>
                        </div>
                    </div>
                </main>
        </div>
    )
}

export default OrderDetails