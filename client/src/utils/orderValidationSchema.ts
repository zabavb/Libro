import { z } from "zod"
import { Status } from "../types"


const today = new Date()


export const orderSchema = z.
    object({
        userId: z
            .string()
            .uuid("Invalid user ID"),
        bookIds: z
            .string(),
            // Temporary removed
            // .array(z.string().min(1))
            // .nonempty("There must be at least a single book in order"),
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
            .refine((val) => new Date(val).getDate() <= today.getDate(), "Order cannot be placed in the future"),
        deliveryDate: z
            .string()
            .refine((val) => !isNaN(Date.parse(val)),"Invalid date"),
        deliveryTypeId: z
            .string()
            .uuid("Invalid delivery type"),
        price: z
            .coerce
            .number()
            .positive(),
        deliveryPrice: z
            .coerce
            .number()
            .positive(),
        status: z
            .nativeEnum(Status, {errorMap: () => ({message: "Invalid status selected"}) })
    })
    .refine((data) => data.deliveryDate > data.orderDate, "Delivery date must be after order date")
    export type OrderFormData = z.infer<typeof orderSchema>