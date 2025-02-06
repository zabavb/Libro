import React, { useEffect } from "react"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { UserFormData, userSchema } from "../../../utils"
import { User, Role } from "../../../types"
import { dateToString } from "../../../api/adapters/commonAdapters"
import { roleEnumToNumber, roleNumberToEnum } from "../../../api/adapters/userAdapter"

interface UserFormProps {
	existingUser?: User
	onAddUser: (user: User) => void
	onEditUser: (id: string, updatedUser: User) => void
}

const UserForm: React.FC<UserFormProps> = ({ existingUser, onAddUser, onEditUser }) => {
	const {
		register,
		handleSubmit,
		setValue,
		formState: { errors },
	} = useForm<UserFormData>({
		resolver: zodResolver(userSchema),
		defaultValues: {
			firstName: "",
			lastName: "",
			dateOfBirth: dateToString(new Date(new Date().getFullYear() - 18)),
			email: "",
			phoneNumber: "",
			role: Role.USER,
		},
	})

	useEffect(() => {
		if (existingUser) {
			setValue("firstName", existingUser.firstName)
			setValue("lastName", existingUser.lastName)
			setValue("dateOfBirth", dateToString(existingUser.dateOfBirth))
			setValue("email", existingUser.email)
			setValue("phoneNumber", existingUser.phoneNumber)
			setValue("role", roleNumberToEnum(existingUser.role))
		}
	}, [existingUser, setValue])

	const onSubmit = (data: UserFormData) => {
		const user: User = {
			id: existingUser ? existingUser.id : "00000000-0000-0000-0000-000000000000",
			firstName: data.firstName,
			lastName: data.lastName || "",
			dateOfBirth: new Date(data.dateOfBirth),
			email: data.email || "",
			phoneNumber: data.phoneNumber || "",
			role: roleEnumToNumber(data.role),
		}

		if (existingUser) onEditUser(existingUser.id, user)
		else onAddUser(user)
	}

	return (
		<form onSubmit={handleSubmit(onSubmit)}>
			<input
				{...register("firstName")}
				placeholder="First Name"
			/>
			<p>{errors.firstName?.message}</p>

			<input
				{...register("lastName")}
				placeholder="Last Name"
			/>
			<p>{errors.lastName?.message}</p>

			<input
				type="date"
				{...register("dateOfBirth")}
				placeholder="Date of Birth"
			/>
			<p>{errors.dateOfBirth?.message}</p>

			<input
				type="email"
				{...register("email")}
				placeholder="Email"
			/>
			<p>{errors.email?.message}</p>

			<input
				type="tel"
				{...register("phoneNumber")}
				placeholder="Phone Number"
			/>
			<p>{errors.phoneNumber?.message}</p>

			<select {...register("role")}>
				<option value="">Select Role</option>
				{Object.entries(Role).map(([key, value]) => (
					<option
						key={key}
						value={value}>
						{value}
					</option>
				))}
			</select>
			<p>{errors.role?.message}</p>

			<button type="submit">{existingUser ? "Update User" : "Add User"}</button>
		</form>
	)
}

export default UserForm
