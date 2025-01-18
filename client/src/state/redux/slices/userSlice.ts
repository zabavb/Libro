import { createSlice, createAsyncThunk } from "@reduxjs/toolkit"
import {
	getAllUsers,
	createUser,
	updateUser,
	deleteUser,
	User,
} from "../../../api/repositories/userRepository"

export const fetchUsers = createAsyncThunk("users/fetchUsers", async () => {
	return await getAllUsers()
})

export const addUser = createAsyncThunk(
	"users/addUser",
	async (user: Partial<User>) => {
		return await createUser(user)
	}
)

export const editUser = createAsyncThunk(
	"users/editUser",
	async ({ id, user }: { id: string; user: Partial<User> }) => {
		return await updateUser(id, user)
	}
)

export const removeUser = createAsyncThunk(
	"users/removeUser",
	async (id: string) => {
		await deleteUser(id)
		return id
	}
)

const userSlice = createSlice({
	name: "users",
	initialState: {
		data: [] as User[],
		loading: false,
		error: null as string | null | undefined,
	},
	reducers: {},
	extraReducers: (builder) => {
		builder
			.addCase(fetchUsers.pending, (state) => {
				state.loading = true
			})
			.addCase(fetchUsers.fulfilled, (state, action) => {
				state.loading = false
				state.data = action.payload
        state.error = null;
			})
			.addCase(fetchUsers.rejected, (state, action) => {
				state.loading = false
        state.error = action.error.message;
			})
			.addCase(addUser.fulfilled, (state, action) => {
				state.data.push(action.payload)
			})
			.addCase(editUser.fulfilled, (state, action) => {
				const index = state.data.findIndex(
					(user) => user.id === action.payload.id
				)
				if (index !== -1) state.data[index] = action.payload
			})
			.addCase(removeUser.fulfilled, (state, action) => {
				state.data = state.data.filter(
					(user) => user.id !== action.payload
				)
			})
	},
})

export default userSlice.reducer
