import { useForm } from "react-hook-form";
import { Order, Status } from "../../../types";
import { zodResolver } from "@hookform/resolvers/zod";
import { dateToString } from "../../../api/adapters/commonAdapters";
import React, { useEffect, useState } from "react";
import { statusEnumToNumber, statusNumberToEnum } from "../../../api/adapters/orderAdapters";
import { OrderFormData, orderSchema } from "../../../utils";
import OrderFormBookSearch from "./OrderFormBookSearch";
import OrderFormBookList from "./OrderFormBookList";
import { useNavigate } from "react-router-dom";
import "@/assets/styles/components/order-form.css"
import "@/assets/styles/base/status-display.css"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
interface OrderFormProps {
    existingOrder?: Order;
    onEditOrder: (existingOrder: Order) => Promise<void>;
    loading: boolean;
}

const OrderForm: React.FC<OrderFormProps> = ({ existingOrder, onEditOrder }) => {
    const [bookObjs, setBookObjs] = useState<Record<string, number>>({})
    const navigate = useNavigate();

    const {
        register,
        handleSubmit,
        watch,
        setValue,
        formState: { errors }
    } = useForm<OrderFormData>({
        resolver: zodResolver(orderSchema),
        defaultValues:
        {
            userId: "",
            books: {},
            address: "",
            region: "",
            city: "",
            orderDate: dateToString(new Date()),
            deliveryDate: dateToString(new Date(new Date().getDate() + 1)),
            price: 0,
            deliveryPrice: 0,
            deliveryTypeId: "",
            status: Status.PENDING
        },
    })

    /* Outsource the function into separate lib */
    function cutFloat(num: number): number {
        const parts = num.toString().split('.');
        if (parts.length === 1) return num; // no decimal part
        const decimal = parts[1].slice(0, 2);
        return parseFloat(`${parts[0]}.${decimal}`);
    }
    
    const statusValue = watch("status");
    const fullPrice = cutFloat((existingOrder?.price ?? 0) + (existingOrder?.deliveryPrice ?? 0));

    useEffect(() => {
        if (existingOrder) {
            setBookObjs(existingOrder.books)
        }
    }, [existingOrder])

    useEffect(() => {
        if (existingOrder) {
            setValue("userId", existingOrder.userId)
            setValue("books", existingOrder.books);
            setValue("region", existingOrder.region)
            setValue("city", existingOrder.city)
            setValue("address", existingOrder.address)
            setValue("price", existingOrder.price)
            setValue("deliveryPrice", existingOrder.deliveryPrice)
            setValue("orderDate", dateToString(existingOrder.orderDate))
            setValue("deliveryDate", dateToString(existingOrder.deliveryDate))
            setValue("deliveryTypeId", existingOrder.deliveryTypeId)
            setValue("status", statusNumberToEnum(existingOrder.status))
        }
    }, [existingOrder, setValue, navigate])

    const onSubmit = (data: OrderFormData) => {
        const id = existingOrder?.id ?? "";

        const order: Order = {
            id: id,
            userId: data.userId,
            books: bookObjs,
            region: data.region,
            city: data.city,
            address: data.address,
            orderDate: new Date(data.orderDate),
            deliveryDate: new Date(data.deliveryDate),
            price: data.price,
            deliveryPrice: data.deliveryPrice,
            deliveryTypeId: data.deliveryTypeId,
            status: statusEnumToNumber(data.status)
        }

        if (existingOrder) {
            onEditOrder(order)
            console.log("Order edited")
        }
        else {
            console.log("Order Does not exist")
        }
    }

    const handleBookAdd = (bookId: string) => {
        setBookObjs((prev) => ({
            ...prev,
            [bookId]: (prev?.[bookId] || 0) + 1
        }));
    }

    const handleBookDelete = (bookId: string) => {
        setBookObjs((prev) => {
            if (!prev || !prev[bookId]) return prev;

            const updatedBooks = { ...prev };

            if (updatedBooks[bookId] > 1) {
                updatedBooks[bookId] -= 1;
            } else {
                delete updatedBooks[bookId];
            }

            return updatedBooks;
        });
    }

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
            <form onSubmit={handleSubmit(onSubmit)}>
                <main className="main-container">
                    {/* Left form */}
                    <div className="main-form-container">
                        <h1 className=" font-semibold text-2xl mb-5">Order Update</h1>
                        {/* Hidden values, cannot be edited */}
                        <input className="absolute" type="hidden" {...register("userId")} />
                        <input className="absolute" type="hidden" {...register("deliveryTypeId")} />
                        <input className="absolute" type="hidden" {...register("price")} />
                        <input className="absolute" type="hidden" {...register("deliveryPrice")} />

                        <div className="input-container">
                            <p className="input-title">Region</p>
                            <input
                                className="input-text"
                                {...register("region")}
                                placeholder="Region" />
                            <p>{errors.region?.message}</p>
                        </div>

                        <div className="input-container">
                            <p className="input-title">City</p>
                            <input
                                className="input-text"
                                {...register("city")}
                                placeholder="City" />
                            <p>{errors.city?.message}</p>
                        </div>

                        <div className="input-container">
                            <p className="input-title">Address</p>
                            <input
                                className="input-text"
                                {...register("address")}
                                placeholder="Address" />
                            <p>{errors.address?.message}</p>
                        </div>

                        <div className="flex gap-2.5">
                            <div className="input-container">
                                <p className="input-title">Order Date</p>
                                <input
                                    className="input-date"
                                    type="date"
                                    {...register("orderDate")}
                                    placeholder="Order Date" />
                                <p>{errors.orderDate?.message}</p>
                            </div>
                            <div className="input-container">
                                <p className="input-title">Delivery Date</p>
                                <input
                                    className="input-date"
                                    type="date"
                                    {...register("deliveryDate")}
                                    placeholder="Delivery Date" />
                                <p>{errors.deliveryDate?.message}</p>
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
                                    <select
                                        className={`input-select ${statusValue.toLowerCase()}`}
                                        {...register("status")}>
                                        {Object.entries(Status).map(([key, value]) => (
                                            <option
                                                className="select-option"
                                                key={key}
                                                value={value}>
                                                {value}
                                            </option>
                                        ))}
                                    </select>
                                    <p>{errors.status?.message}</p>
                                </div>
                            </div>
                        </div>
                        <div className="mb-3.5 text-[#FF642E]">
                            <h1 className="font-semibold text-2xl">Order</h1>
                            <p className="font-medium text-sm">№ {existingOrder?.id}</p>
                        </div>
                        <div className="mb-3.5">
                            <OrderFormBookSearch
                                onBookAdd={handleBookAdd}
                            />
                            <div style={{maxHeight:"240px", overflow:"auto"}}>
                                <OrderFormBookList
                                    books={bookObjs}
                                    onBookDelete={handleBookDelete}
                                    onBookAdd={handleBookAdd}
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
            </form>
        </div>
    )
}

export default OrderForm