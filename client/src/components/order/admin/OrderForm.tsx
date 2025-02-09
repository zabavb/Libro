import { useForm } from "react-hook-form";
import { Order, Status } from "../../../types";
import { OrderFormData, orderSchema } from "../../../utils/orderValidationSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { dateToString } from "../../../api/adapters/commonAdapters";
import { useEffect, useState } from "react";
import { statusEnumToNumber, statusNumberToEnum } from "../../../api/adapters/orderAdapters";

interface OrderFormProps {
    existingOrder?: Order
    onAddOrder: (order: Order) => void
    onEditOrder: (id: string, updatedOrder: Order) => void
}

const OrderForm: React.FC<OrderFormProps> = ({ existingOrder, onAddOrder, onEditOrder }) => {
    const [bookIds, setBookIds] = useState<string[]>([]);
    
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
            bookIds: [],
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
            setValue("bookIds", existingOrder.bookIds as [string, ...string[]]);
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
    }, [existingOrder, setValue])

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

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setBookIds(e.target.value.split(",").map((item) => item.trim()));
      };

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            {/* manual for now, will be rewritten in the future */}
            <input {...register("userId")}
            placeholder="User ID" />
            <p>{errors.userId?.message}</p>

            {/* To implement: More user friendly input */}
            <input {...register("bookIds")}
            placeholder="Write bookIds separated by commas" 
            onChange={handleChange}/>
            <p>{errors.bookIds?.message}</p>


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

            {/* manual for now, will be rewritten after creation of delivery service */}
            <input {...register("deliveryTypeId")}
            placeholder="Delivery type id" />
            <p>{errors.deliveryTypeId?.message}</p>

            <input
                type="number"
                {...register("price")}
                placeholder="Price"
            />
            <p>{errors.price?.message}</p>

            <input
                type="number"
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