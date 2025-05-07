import { z } from "zod"
import { Status } from "../types"


export const orderSchema = z.
    object({
        userId: z
            .string()
            .uuid("Invalid user ID"),
        books: z
            .record(
                z.string(),
                z.number().positive()
        ),
        address: z
            .string()
            .min(2,"Address must be at least 2 characters")
            .max(75, "Address is to long"),
        region: z
            .string()
            .min(2, "Region must be at least 2 characters")
            .max(50, "Region is too long"),
        city: z
            .string()
            .min(2, "City must be at least 2 characters")
            .max(50, "City is too long"),
        orderDate: z
            .string()
            .refine((val) => !isNaN(Date.parse(val)),"Invalid date")
            .refine((val) => {
                const orderDate = new Date(val).toISOString().split("T")[0]; // Extract YYYY-MM-DD
                const todayDate = new Date().toISOString().split("T")[0];

                return orderDate <= todayDate;
            }, "Order cannot be placed in the future"),
        deliveryDate: z
            .string()
            .refine((val) => !isNaN(Date.parse(val)),"Invalid date"),
        deliveryTypeId: z
            .string()
            .uuid("Invalid delivery type"),
        price: z
            .coerce
            .number()
            .nonnegative(),
        deliveryPrice: z
            .coerce
            .number()
            .nonnegative(),
        status: z
            .nativeEnum(Status, {errorMap: () => ({message: "Invalid status selected"}) })
    })
    .refine((data) => data.deliveryDate > data.orderDate, "Delivery date must be after order date")
    export type OrderFormData = z.infer<typeof orderSchema>