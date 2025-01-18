import React, { useState } from "react"
import { useDispatch } from "react-redux"
import { addUser, editUser } from "../../state/redux/slices/userSlice"
import { AppDispatch } from "../../state/redux/store"

interface UserFormProps {
  existingUser?: {
    id: string
    firstName: string
    lastName: string
    dateOfBirth: string
    email: string
    phoneNumber: string
    role: string
  }
}

const UserForm: React.FC<UserFormProps> = ({ existingUser }) => {
  const dispatch = useDispatch<AppDispatch>()
  const [firstName, setFirstName] = useState(existingUser?.firstName || "")
  const [lastName, setLastName] = useState(existingUser?.lastName || "")
  const [dateOfBirth, setDateOfBirth] = useState(existingUser?.dateOfBirth || "")
  const [email, setEmail] = useState(existingUser?.email || "")
  const [phoneNumber, setPhoneNumber] = useState(existingUser?.phoneNumber || "")
  const [role, setRole] = useState(existingUser?.role || "")

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    const user = { firstName, lastName, dateOfBirth, email, phoneNumber, role }
    if (existingUser)
      dispatch(editUser({ id: existingUser.id, user }))
    else
      dispatch(addUser(user))
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
        onChange={(e) => setRole(e.target.value)}
        required
      >
        <option value="" disabled>Select Role</option>
        <option value="admin">Admin</option>
        <option value="moderator">Moderator</option>
        <option value="user">User</option>
        <option value="guest">Guest</option>
      </select>

      <button type="submit">
        {existingUser ? "Update User" : "Add User"}
      </button>
    </form>
  )
}

export default UserForm
