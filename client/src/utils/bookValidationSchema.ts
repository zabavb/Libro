import { z } from "zod";
import { CoverType } from "../types/subTypes/book/CoverType";
import { Language } from "../types/subTypes/book/Language";

export const bookValidationSchema = z.object({
    title: z
        .string()
        .min(2, "Title must be at least 2 characters")
        .max(100, "Title is too long"),

    year: z
        .string()
        .regex(/^\d{4}$/, "Year must be a 4-digit number")
        .refine((val) => {
            const year = parseInt(val);
            const currentYear = new Date().getFullYear();
            return year >= 1450 && year <= currentYear;
        }, "Year must be realistic (after 1450 and not in the future)"),

    cover: z.nativeEnum(CoverType, {
        errorMap: () => ({ message: "Invalid cover type selected" }),
    }),

    price: z
        .coerce.number()
        .positive("Price must be a positive number"),

    language: z.nativeEnum(Language, {
        errorMap: () => ({ message: "Invalid language selected" }),
    }),

    isAvailable: z.boolean(),
});

export type BookFormData = z.infer<typeof bookValidationSchema>;
