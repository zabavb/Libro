import { z } from "zod";

export const publisherSchema = z
    .object({
        name: z.string()
        .min(2, "Name must be at least 2 characters")
        .max(50, "Name is too long"),
        description: z
        .string()
        .min(10, "Description must be at least 10 characters")
        .max(400, "Description is too long")
        .optional(),
    })

    export type PublisherFormData = z.infer<typeof publisherSchema>;