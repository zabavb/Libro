import { useForm } from "react-hook-form";
import { DeliveryType, Order, Status } from "../../types";
import { zodResolver } from "@hookform/resolvers/zod";
import React, { useEffect, useState } from "react";
import { statusEnumToNumber } from "../../api/adapters/orderAdapters";
import { OrderFormData, orderSchema } from "../../utils";
import { useNavigate } from "react-router-dom";

interface UserCheckoutFormProps {
    books: Record<string, number>
    deliveryTypes?: DeliveryType[]
    price: number
    onAddOrder: (order: Order) => void
    loading: boolean
}

const UserCheckoutForm: React.FC<UserCheckoutFormProps> = ({ books, price, deliveryTypes, onAddOrder, loading }) => {

    const [userId, setUserId] = useState<string>("");
    const navigate = useNavigate()



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
            orderDate: new Date().toString(),
            deliveryDate: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toString(),
            price: 25,
            deliveryPrice: 25,
            deliveryTypeId: "",
            status: Status.PENDING
        },
    })

    useEffect(() => {
        
        const json = localStorage.getItem('user');
        if (json != null) {
            const user = JSON.parse(json)
            setUserId(user.id)
            setValue("userId", user.id);
        }
        else {
            navigate('/login')
        }
    }, [navigate, loading, setValue, userId])

    const onSubmit = (data: OrderFormData) => {

        const order: Order = {
            id: "00000000-0000-0000-0000-000000000000",
            userId: userId,
            books: books,
            region: data.region,
            city: data.city,
            address: data.address,
            orderDate: new Date(data.orderDate),
            deliveryDate: new Date(data.deliveryDate),
            price: data.price,
            deliveryPrice: 25, // Implement delivery price field to delivery type???
            deliveryTypeId: data.deliveryTypeId,
            status: statusEnumToNumber(data.status)
        }

        onAddOrder(order)
    }

    if (loading) return <p>Loading...</p>
    return (
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>
                <hr />
                <h2>Order Form</h2>
                <hr />
                <p>Total Price: {price}</p>
                {/* Hidden data, can't be set by user */}
                <input type="hidden" {...register("userId")}/>
                <p>{errors.userId?.message}</p>
                <input type="hidden" {...register("orderDate")} />
                <p>{errors.orderDate?.message}</p>
                <input type="hidden" {...register("deliveryDate")} />
                <p>{errors.deliveryDate?.message}</p>
                <input type="hidden" {...register("price")} />
                <p>{errors.price?.message}</p>
                <input type="hidden" {...register("deliveryPrice")} />
                <p>{errors.deliveryPrice?.message}</p>
                <input type="hidden"{...register("status")} />
                <p>{errors.status?.message}</p>

                {/* Form */}
                <input {...register("region")}
                    placeholder="Region" />
                <p>{errors.region?.message}</p>

                <input {...register("city")}
                    placeholder="City" />
                <p>{errors.city?.message}</p>

                <input {...register("address")}
                    placeholder="Address" />
                <p>{errors.address?.message}</p>

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
                <hr />
                <button type="submit">Place Order</button>
            </form>
        </div>
    )
}

export default UserCheckoutForm