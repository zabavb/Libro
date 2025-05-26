import { z } from "zod";

const today = new Date();

export const authorSchema = z
    .object({
        name: z.string()
            .min(2, "Last name must be at least 2 characters")
            .max(50, "Name is too long"),
        dateOfBirth: z
            .string()
            .refine((val) => !isNaN(Date.parse(val)), 'Invalid date')
            .refine(
                (val) => new Date(val) <= today,
                'Date of birth cannot be in the future',)
            .optional(),
        biography: z
            .string()
            .min(10, "Biography must be at least 10 characters")
            .max(400, "Biography is too long")
            .optional(),
        citizenship: z
            .string()
            .min(2, "Citizenship must be at least 2 characters")
            .max(50, "Citizenship is too long")
            .optional(),
        image: z
            .instanceof(File)
            .refine((file) => ["image/png", "image/jpeg", "image/jpg"].includes(file.type), {
                message: "Image must be PNG, JPG, or JPEG",
            })
            .optional(),
    })

export type AuthorFormData = z.infer<typeof authorSchema>