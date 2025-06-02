import { useForm } from "react-hook-form";
import { Order, Status } from "../../../types";
import { zodResolver } from "@hookform/resolvers/zod";
import { dateToString } from "../../../api/adapters/commonAdapters";
import React, { useEffect, useState } from "react";
import { statusEnumToNumber, statusNumberToEnum } from "../../../api/adapters/orderAdapters";
import { OrderFormData, orderSchema } from "../../../utils";
import OrderFormBookSearch from "./OrderFormBookSearch";
import { useNavigate } from "react-router-dom";
import "@/assets/styles/components/order/order-form.css"
import "@/assets/styles/base/status-display.css"
import OrderAdminFormBookList from "./OrderAdminFormBookList";
import {icons} from '@/lib/icons'
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

    const statusValue = watch("status");

    useEffect(() => {
        if (existingOrder && Array.isArray(existingOrder.books)) {
            const record: Record<string, number> = {};
            for (const book of existingOrder.books) {
                record[book.key] = book.value;
            }
            setBookObjs(record);
        }
    }, [existingOrder])

    useEffect(() => {

        if (existingOrder) {
            setValue("userId", existingOrder.userId)
            setValue("books", bookObjs);
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
    }

    useEffect(() => {
    console.log("Validation errors:", errors);
    }, [errors]);

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

    const uid = existingOrder?.id.toString().split('-')[4];

    return (
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>

                <header className="header-container">
                    <div className="flex h-10 gap-2.5">
                        <button className="cancel-button" type="button" onClick={() => navigate('/admin/orders')}><img src={icons.oArrowLeft}/></button>
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
                        <h1 className=" font-semibold text-2xl mb-5">Order № {uid}</h1>
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
                    </div>
                    <div className="book-form-container">

                        <div className="mb-3.5">
                            <OrderFormBookSearch
                                onBookAdd={handleBookAdd}
                            />
                            <div className="overflow-auto" style={{ maxHeight: "900px" }}>
                                <OrderAdminFormBookList
                                    books={bookObjs}
                                    onBookDelete={handleBookDelete}
                                    onBookAdd={handleBookAdd}
                                />
                            </div>
                        </div>
                    </div>
                </main>
            </form>
        </div>
    )
}

export default OrderForm