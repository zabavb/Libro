import { z } from "zod";
import { CoverType } from "../types/subTypes/book/CoverType";
import { Language } from "../types/subTypes/book/Language";

export const bookValidationSchema = z.object({
    title: z.string().min(2, "Title must be at least 2 characters").max(100, "Title is too long"),

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

    price: z.coerce.number().positive("Price must be a positive number"),

    language: z.nativeEnum(Language, {
        errorMap: () => ({ message: "Invalid language selected" }),
    }),

    quantity: z.coerce.number().nonnegative("Must be a non-negative number"),

    authorId: z.string(),
    publisherId: z.string(),
    categoryId: z.string(),
    discountId: z.string().optional(),

    subcategoryIds: z.array(z.string()).optional(),

    description: z.string().min(2, "Description must be at least 2 characters").max(1000, "Description is too long"),

    image: z
        .instanceof(File)
        .refine((file) => ["image/png", "image/jpeg", "image/jpg"].includes(file.type), {
            message: "Image must be PNG, JPG, or JPEG",
        })
        .optional(),

    PDF: z
        .instanceof(File)
        .refine((file) => file.type === "application/pdf", {
            message: "Document must be in PDF format",
        })
        .optional(),

    audio: z
        .instanceof(File)
        .refine((file) => file.type === "audio/mpeg" || file.type === "audio/mp3", {
            message: "Audio must be in MP3 format",
        })
        .optional(),
});

export type BookFormData = z.infer<typeof bookValidationSchema>;
