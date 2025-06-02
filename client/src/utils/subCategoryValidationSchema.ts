import { z } from "zod";

export const subCategorySchema = z
    .object({
        name: z.string()
            .min(2, "Last name must be at least 2 characters")
            .max(50, "Name is too long"),
        categoryId: 
            z.string().uuid()
    })

export type SubCategoryFormData  = z.infer<typeof subCategorySchema>