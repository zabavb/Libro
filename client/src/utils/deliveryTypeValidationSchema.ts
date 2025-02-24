import { z } from "zod";


export const deliveryTypeSchema = z
    .object({
        serviceName: z
            .string()
            .min(2, "Service name must be at least 2 characters")
            .max(50, "Service name is too long")
    })

    export type DeliveryTypeFormData = z.infer<typeof deliveryTypeSchema>