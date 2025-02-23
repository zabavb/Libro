import { z } from "zod"

export const loginSchema = z.object({
	identifier: z
		.string()
		.min(2, "Identifier must be at least 2 characters")
		.refine((val) => /\S+@\S+\.\S+/.test(val) || /^[0-9]{10,15}$/.test(val), {
			message: "Invalid email or phone number",
		}),
	password: z.string().min(8, "Password must be at least 8 characters"),
})

export type LoginFormData = z.infer<typeof loginSchema>

export const registerSchema = z
	.object({
		firstName: z.string().min(2, "First name must be at least 2 characters").max(50, "First name too long"),
		lastName: z
			.string()
			.min(2, "Last name must be at least 2 characters")
			.max(50, "Last name too long")
			.optional(),
		email: z.string().email("Invalid email").optional().or(z.literal("")),
		phoneNumber: z
			.string()
			.regex(/^\d{10,15}$/, "Phone number must be 10-15 digits")
			.optional()
			.or(z.literal("")),
		password: z.string().min(8, "Password must be at least 8 characters"),
		confirmPassword: z.string(),
	})
	.refine((data) => data.email !== "" || data.phoneNumber !== "", {
		message: "Either Email or Phone Number must be provided",
		path: ["email"],
	})
	.refine((data) => data.password === data.confirmPassword, {
		message: "Passwords do not match",
	})

export type RegisterFormData = z.infer<typeof registerSchema>
