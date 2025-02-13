import React, { useEffect, useState } from "react"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { UserFormData, userSchema } from "../../../utils"
import { User, Role } from "../../../types"
import { dateToString } from "../../../api/adapters/commonAdapters"
import { roleNumberToEnum } from "../../../api/adapters/userAdapter"

interface UserFormProps {
	existingUser?: User
	onAddUser: (user: FormData) => void
	onEditUser: (id: string, updatedUser: FormData) => void
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

	const [imagePreview, setImagePreview] = useState<string | null>(null)

	useEffect(() => {
		if (existingUser) {
			setValue("firstName", existingUser.firstName)
			setValue("lastName", existingUser.lastName)
			setValue("dateOfBirth", dateToString(existingUser.dateOfBirth))
			setValue("email", existingUser.email)
			setValue("phoneNumber", existingUser.phoneNumber)
			setValue("role", roleNumberToEnum(existingUser.role))
			setImagePreview(existingUser.imageUrl)
		}
	}, [existingUser, setValue])

	const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		const file = event.target.files?.[0]
		if (file) {
			const imageUrl = URL.createObjectURL(file)
			setImagePreview(imageUrl)
			setValue("image", file)
		}
	}

	const onSubmit = (data: UserFormData) => {
		const formData = new FormData()
		formData.append("id", existingUser?.id ?? "00000000-0000-0000-0000-000000000000")
		formData.append("firstName", data.firstName)
		formData.append("lastName", data.lastName || "")
		formData.append("dateOfBirth", data.dateOfBirth)
		formData.append("email", data.email || "")
		formData.append("phoneNumber", data.phoneNumber || "")
		formData.append("role", data.role)
		formData.append("image", data.image ?? "")
		formData.append("imageUrl", existingUser?.imageUrl ?? "")

		if (existingUser) onEditUser(existingUser.id, formData)
		else onAddUser(formData)
	}

	return (
		<form onSubmit={handleSubmit(onSubmit)}>
			<label
				htmlFor="imageUpload"
				style={{
					display: "flex",
					alignItems: "center",
					justifyContent: "center",
					width: "150px",
					height: "150px",
					border: "2px dashed #ccc",
					borderRadius: "10px",
					cursor: "pointer",
					overflow: "hidden",
					backgroundSize: "cover",
					backgroundPosition: "center",
					backgroundImage: imagePreview ? `url(${imagePreview})` : "none",
				}}>
				{!imagePreview && <span>Click to Upload</span>}
			</label>
			<input
				id="imageUpload"
				type="file"
				accept="image/*"
				style={{ display: "none" }}
				onChange={handleImageChange}
			/>
			<p>{errors.image?.message}</p>

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
