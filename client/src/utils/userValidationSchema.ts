import { z } from "zod"
import { Role } from "../types"

const today = new Date()
const minDateOfBirth = new Date(today.getFullYear() - 90, today.getMonth(), today.getDate())

export const userSchema = z
	.object({
		firstName: z
			.string()
			.min(2, "First name must be at least 2 characters")
			.max(50, "First name too long"),
		lastName: z
			.string()
			.min(2, "Last name must be at least 2 characters")
			.max(50, "Last name too long")
			.optional(),
		dateOfBirth: z
			.string()
			.refine((val) => !isNaN(Date.parse(val)), "Invalid date")
			.refine(
				(val) => new Date(val) >= minDateOfBirth,
				`Date of birth must be at least ${minDateOfBirth.getFullYear()} or later`
			)
			.refine((val) => new Date(val) <= today, "Date of birth cannot be in the future"),
		email: z.string().email("Invalid email").optional().or(z.literal("")),
		phoneNumber: z
			.string()
			.min(10, "Phone number must be at least 10 digits")
			.max(15, "Phone number too long")
			.optional()
			.or(z.literal("")),
		role: z.nativeEnum(Role, { errorMap: () => ({ message: "Invalid role selected" }) }),
	})
	.refine((data) => data.email !== "" || data.phoneNumber !== "", {
		message: "Either Email or Phone Number must be provided",
		path: ["email"],
	})

export type UserFormData = z.infer<typeof userSchema>
