import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit"
import { getAllUsers, createUser, updateUser, deleteUser } from "../../../api/repositories/userRepository"
import { User, UserFilter, UserSort } from "../../../types"

export const fetchUsers = createAsyncThunk(
	"users/fetchUsers",
	async ({
		pageNumber = 1,
		pageSize = 10,
		searchTerm,
		filters,
		sort,
	}: {
		pageNumber?: number
		pageSize?: number
		searchTerm?: string
		filters?: UserFilter
		sort?: UserSort
	}) => {
		const response = await getAllUsers(pageNumber, pageSize, searchTerm, filters, sort)
		console.log(`userSlice.ts: fetch users:`, response)
		return response
	}
)

export const addUser = createAsyncThunk("users/addUser", async (user: Partial<User>) => {
	console.log(`userSlice.ts: add user:[${user}]`)
	return await createUser(user)
})

export const editUser = createAsyncThunk(
	"users/editUser",
	async ({ id, user }: { id: string; user: Partial<User> }) => {
		console.log(`userSlice.ts: edit id:[${id}] user:[${user}]`)
		return await updateUser(id, user)
	}
)

export const removeUser = createAsyncThunk("users/removeUser", async (id: string) => {
	console.log(`userSlice.ts: remove id:[${id}]`)
	await deleteUser(id)
	return id
})

const userSlice = createSlice({
	name: "users",
	initialState: {
		data: [] as User[],
		loading: false,
		error: null as string | null | undefined,
		operationStatus: null as "success" | "error" | "pending" | null,
		pagination: {
			pageNumber: 1,
			pageSize: 10,
			totalCount: 0,
		},
		searchTerm: "",
		filters: {} as UserFilter,
		sort: {} as UserSort,
	},
	reducers: {
		setSearchTerm: (state, action) => {
			state.searchTerm = action.payload
		},
		setFilters: (state, action) => {
			state.filters = action.payload
		},
		setSort: (state, action: PayloadAction<keyof UserSort>) => {
			const field = action.payload
			const currentSort = state.sort[field]
			const newSort = currentSort === undefined ? true : currentSort === true ? false : undefined
			state.sort = { [field]: newSort }
		},
		resetOperationStatus: (state) => {
			state.operationStatus = null
		},
	},
	extraReducers: (builder) => {
		builder
			// Fetch Users
			.addCase(fetchUsers.pending, (state) => {
				state.loading = true
				state.error = null
			})
			.addCase(fetchUsers.fulfilled, (state, action) => {
				state.loading = false
				state.data = action.payload.items
				state.pagination = {
					pageNumber: action.payload.pageNumber,
					pageSize: action.payload.pageSize,
					totalCount: action.payload.totalCount,
				}
			})
			.addCase(fetchUsers.rejected, (state, action) => {
				state.loading = false
				state.error = action.error.message
			})
			// Add User
			.addCase(addUser.pending, (state) => {
				state.operationStatus = "pending"
			})
			.addCase(addUser.fulfilled, (state, action) => {
				state.operationStatus = "success"
				state.data.push(action.payload)
			})
			.addCase(addUser.rejected, (state, action) => {
				state.operationStatus = "error"
				state.error = action.error.message
			})
			// Edit User
			.addCase(editUser.pending, (state) => {
				state.operationStatus = "pending"
			})
			.addCase(editUser.fulfilled, (state, action) => {
				state.operationStatus = "success"
				const index = state.data.findIndex((user) => user.id === action.payload.id)
				if (index !== -1) state.data[index] = action.payload
			})
			.addCase(editUser.rejected, (state, action) => {
				state.operationStatus = "error"
				state.error = action.error.message
			})
			// Remove User
			.addCase(removeUser.pending, (state) => {
				state.operationStatus = "pending"
			})
			.addCase(removeUser.fulfilled, (state, action) => {
				state.operationStatus = "success"
				state.data = state.data.filter((user) => user.id !== action.payload)
			})
			.addCase(removeUser.rejected, (state, action) => {
				state.operationStatus = "error"
				state.error = action.error.message
			})
	},
})

export const { setSearchTerm, setFilters, setSort, resetOperationStatus } = userSlice.actions

export default userSlice.reducer
