import { useForm } from "react-hook-form";
import { Book, Order, Status } from "../../../types";
import { zodResolver } from "@hookform/resolvers/zod";
import { dateToString } from "../../../api/adapters/commonAdapters";
import React, { useEffect, useState } from "react";
import { statusEnumToNumber, statusNumberToEnum } from "../../../api/adapters/orderAdapters";
import { OrderFormData, orderSchema } from "../../../utils";
import OrderFormBookSearch from "./OrderFormBookSearch";
import OrderFormBookList from "./OrderFormBookList";
import { useNavigate } from "react-router-dom";

interface OrderFormProps {
    page: number
    books?: Book[]
    existingOrder: Order
    onEditOrder: (id: string, updatedOrder: Order) => void
    onPageChange: (page: number) => void
}

const OrderForm: React.FC<OrderFormProps> = ({page, books, existingOrder, onEditOrder, onPageChange }) => {
    const [bookObjs, setBookObjs] = useState<Record<string,number>>({})
    const navigate = useNavigate();
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

    useEffect(() => {
        if(existingOrder) {
            setBookObjs(existingOrder.books)
        }
    },[existingOrder])

    useEffect(() => {
        if(existingOrder) {
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
    }, [existingOrder, setValue, bookObjs,navigate])

    const onSubmit = (data: OrderFormData) => {
        const order: Order = {
            id: existingOrder.id,
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

        if (existingOrder){ 
            onEditOrder(existingOrder.id, order)
            console.log("Order edited")
        }
        else{
            console.log("Order Does not exist")
        } 
    }
    
    const handleBookAdd = (bookId: string) => {
        console.log("Book added")
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
        <form onSubmit={handleSubmit(onSubmit)}>
            {/* Hidden values, cannot be edited */}

            <input type="hidden" {...register("userId")}/>
            <p>{errors.userId?.message}</p>

            <input type="hidden" {...register("deliveryTypeId")}/>
            <p>{errors.deliveryTypeId?.message}</p>

            <input type="hidden" {...register("price")}/>
            <p>{errors.price?.message}</p>

            <input type="hidden" {...register("deliveryPrice")}/>
            <p>{errors.deliveryPrice?.message}</p>


            <OrderFormBookSearch
                page={page}
                books={books}
                onBookAdd={handleBookAdd}
                onPageChange={onPageChange}
                />

            <OrderFormBookList
                books={bookObjs}
                onBookDelete={handleBookDelete}
                onBookAdd={handleBookAdd}
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
            <p>{errors.deliveryDate?.message}</p>


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
            
            <div style={{display:"flex"}}>
                <button type="submit">Update Order</button>
                <p style={{cursor:"pointer"}} onClick={() => navigate('/admin/orders')}>Cancel</p>
            </div>
        </form>
    )
}

export default OrderForm