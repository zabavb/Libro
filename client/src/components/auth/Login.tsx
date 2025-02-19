import React /* , { useState } */ from "react"
import { useAuth } from "../../state/context"
import { LoginFormData, loginSchema } from "../../utils"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"

const Login: React.FC = () => {
	const { login } = useAuth()
	// const [loading, setLoading] = useState(false)

	const {
		register,
		handleSubmit,
		formState: { errors, isSubmitting },
	} = useForm<LoginFormData>({
		resolver: zodResolver(loginSchema),
	})

	const onSubmit = async (data: LoginFormData) => {
		// setLoading(true)
		try {
			await login(data)
		} catch (error) {
			console.error("Login failed: ", error)
		} /* finally {
			setLoading(false)
		} */
	}

	return (
		<div>
			<h2>Login</h2>
			<form onSubmit={handleSubmit(onSubmit)}>
				<div>
					<input
						{...register("identifier")}
						placeholder="Email or Phone Number"
					/>
					<p>{errors.identifier?.message}</p>
				</div>
				<div>
					<input
						{...register("password")}
						placeholder="Password"
					/>
					<p>{errors.password?.message}</p>
				</div>

				<button
					type="submit"
					disabled={/* loading ||  */ isSubmitting}>
					{/* loading */ isSubmitting ? "Logging in..." : "Login"}
				</button>
			</form>
		</div>
	)
}

export default Login
