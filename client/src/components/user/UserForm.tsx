import React, { useState, useEffect } from "react"
import { User } from "../../types"
import { dateToString } from "../../api/adapters/commonAdapters"

interface UserFormProps {
	existingUser?: User
	onAddUser: (user: User) => void
	onEditUser: (id: string, updatedUser: User) => void
}

const UserForm: React.FC<UserFormProps> = ({ existingUser, onAddUser, onEditUser }) => {
	const [id, setId] = useState("00000000-0000-0000-0000-000000000000")
	const [firstName, setFirstName] = useState("")
	const [lastName, setLastName] = useState("")
	const [dateOfBirth, setDateOfBirth] = useState(dateToString(new Date(new Date().getFullYear() - 18)))
	const [email, setEmail] = useState("")
	const [phoneNumber, setPhoneNumber] = useState("")
	const [role, setRole] = useState(2)

	useEffect(() => {
		if (existingUser) {
			setId(existingUser.id)
			setFirstName(existingUser.firstName)
			setLastName(existingUser.lastName)
			setDateOfBirth(dateToString(existingUser.dateOfBirth))
			setEmail(existingUser.email)
			setPhoneNumber(existingUser.phoneNumber)
			setRole(existingUser.role)
		}
	}, [existingUser])

	const handleSubmit = (e: React.FormEvent) => {
		e.preventDefault()
		const user: User = {
			id,
			firstName,
			lastName,
			dateOfBirth: new Date(dateOfBirth),
			email,
			phoneNumber,
			role,
		}
		if (existingUser) onEditUser(existingUser.id, user)
		else onAddUser(user)
	}

	return (
		<form onSubmit={handleSubmit}>
			<input
				type="text"
				value={firstName}
				onChange={(e) => setFirstName(e.target.value)}
				placeholder="First Name"
				required
			/>
			<input
				type="text"
				value={lastName}
				onChange={(e) => setLastName(e.target.value)}
				placeholder="Last Name"
			/>
			<input
				type="date"
				value={dateOfBirth}
				onChange={(e) => setDateOfBirth(e.target.value)}
				placeholder="Date of Birth"
			/>
			<input
				type="email"
				value={email}
				onChange={(e) => setEmail(e.target.value)}
				placeholder="Email"
				required
			/>
			<input
				type="phone"
				value={phoneNumber}
				onChange={(e) => setPhoneNumber(e.target.value)}
				placeholder="Phone Number"
				required
			/>
			<select
				value={role}
				onChange={(e) => setRole(Number.parseInt(e.target.value))}
				required>
				<option
					value=""
					disabled>
					Select Role
				</option>
				<option value="0">Admin</option>
				<option value="1">Moderator</option>
				<option value="2">User</option>
				<option value="3">Guest</option>
			</select>

			<button type="submit">{existingUser ? "Update User" : "Add User"}</button>
		</form>
	)
}

export default UserForm
