import React /* , { useState } */ from "react"
import { registerService } from "../../services"
import { useForm } from "react-hook-form"
import { RegisterFormData, registerSchema } from "../../utils"
import { zodResolver } from "@hookform/resolvers/zod"

const Register: React.FC = () => {
	// const [loading, setLoading] = useState(false)

	const {
		register,
		handleSubmit,
		formState: { errors, isSubmitting },
	} = useForm<RegisterFormData>({
		resolver: zodResolver(registerSchema),
	})

	const onSubmit = async (data: RegisterFormData) => {
		// setLoading(true)
		try {
			await registerService(data)
		} catch (error) {
			console.error("Registration failed: ", error)
		} /* finally {
			setLoading(false)
		} */
	}

	return (
		<div>
			<h2>Register</h2>
			<form onSubmit={handleSubmit(onSubmit)}>
				<input
					{...register("firstName")}
					placeholder="First Name"
				/>
				<p>{errors.firstName?.message}</p>

				<input
					{...register("lastName")}
					placeholder="Last Name (Optional)"
				/>
				<p>{errors.lastName?.message}</p>

				<input
					{...register("email")}
					placeholder="Email"
					type="email"
				/>
				<p>{errors.email?.message}</p>

				<input
					{...register("phoneNumber")}
					placeholder="Phone Number"
					type="tel"
				/>
				<p>{errors.phoneNumber?.message}</p>

				<input
					{...register("password")}
					placeholder="Password"
					type="password"
				/>
				<p>{errors.password?.message}</p>

				<input
					{...register("confirmPassword")}
					placeholder="Confirm Password"
					type="password"
				/>
				<p>{errors.confirmPassword?.message}</p>

				<button
					type="submit"
					disabled={/* loading ||  */ isSubmitting}>
					{/* loading */ isSubmitting ? "Registering..." : "Register"}
				</button>
			</form>
		</div>
	)
}

export default Register
