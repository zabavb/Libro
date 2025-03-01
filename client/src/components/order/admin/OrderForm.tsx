import { useForm } from "react-hook-form";
import { Book, DeliveryType, Order, Status } from "../../../types";
import { zodResolver } from "@hookform/resolvers/zod";
import { dateToString } from "../../../api/adapters/commonAdapters";
import React, { useEffect, useState } from "react";
import { statusEnumToNumber, statusNumberToEnum } from "../../../api/adapters/orderAdapters";
import { OrderFormData, orderSchema } from "../../../utils";
import OrderFormBookSearch from "./OrderFormBookSearch";
import OrderFormBookList from "./OrderFormBookList";

interface OrderFormProps {
    page: number
    books?: Book[]
    deliveryTypes?: DeliveryType[]
    existingOrder?: Order
    onAddOrder: (order: Order) => void
    onEditOrder: (id: string, updatedOrder: Order) => void
    onPageChange: (page: number) => void
}

const OrderForm: React.FC<OrderFormProps> = ({page, books, deliveryTypes, existingOrder, onAddOrder, onEditOrder, onPageChange }) => {
    const [bookIds, setBookIds] = useState<string[]>([])
   
    const {
        register,
        handleSubmit,
        setValue,
        formState: { errors }
    } = useForm<OrderFormData>({
        resolver: zodResolver(orderSchema),
        defaultValues:
        {
            userId: "",
            bookIds: "",
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

    useEffect(() => {
        if(existingOrder) {
            setValue("userId", existingOrder.userId)
            setValue("bookIds", existingOrder.bookIds.toString());
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
    }, [existingOrder, setValue, bookIds])

    const onSubmit = (data: OrderFormData) => {
        const order: Order = {
            id: existingOrder ? existingOrder.id : "00000000-0000-0000-0000-000000000000",
            userId: data.userId,
            bookIds: bookIds,
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

        if (existingOrder){ 
            onEditOrder(existingOrder.id, order)
            console.log("Order edited")
        }
        else{
            onAddOrder(order)
            console.log("Order add")
        } 
    }
    
    const handleBookAdd = (bookId: string) => {
        console.log("Book added")
        setBookIds((prev) => [...prev, bookId]);
    }

    const handleBookDelete = (bookId: string) => {
        setBookIds((prev) => prev.filter((id) => id !== bookId));
    }
    
    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            {/* manual for now, will be rewritten in the future */}
            <input {...register("userId")}
            placeholder="User ID" />
            <p>{errors.userId?.message}</p>

            <OrderFormBookSearch
                page={page}
                books={books}
                onBookAdd={handleBookAdd}
                onPageChange={onPageChange}
                />

            <OrderFormBookList
                bookIds={bookIds}
                onBookDelete={handleBookDelete}
            />


            <input {...register("region")}
            placeholder="Region" />
            <p>{errors.region?.message}</p>

            <input {...register("city")}
            placeholder="City" />
            <p>{errors.city?.message}</p>

            <input {...register("address")}
            placeholder="Address" />
            <p>{errors.address?.message}</p>

            <input 
            type="date"
            {...register("orderDate")}
            placeholder="Order Date" />
            <p>{errors.orderDate?.message}</p>

            <input 
            type="date"
            {...register("deliveryDate")}
            placeholder="Delivery Date" />
            <p>{errors.orderDate?.message}</p>

            <select {...register("deliveryTypeId")}>
            <option value="">Select Delivery Type</option>
                {deliveryTypes?.map((delivery) => (
                    <option
                        value={delivery.id}>
                        {delivery.serviceName}
                    </option>
                ))}
            </select>
            <p>{errors.deliveryTypeId?.message}</p>
            <input
                type="text"
                {...register("price")}
                placeholder="Price"
            />
            <p>{errors.price?.message}</p>

            <input
                type="text"
                {...register("deliveryPrice")}
                placeholder="Delivery Price"
            />
            <p>{errors.price?.message}</p>

            <select {...register("status")}>
                <option value="">Select Status</option>
                {Object.entries(Status).map(([key, value]) => (
                    <option
                        key={key}
                        value={value}>
                        {value}
                    </option>
                ))}
            </select>
            <p>{errors.status?.message}</p>

            <button type="submit">{existingOrder ? "Update Order" : "Add Order"}</button>
        </form>
    )
}

export default OrderForm