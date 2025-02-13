import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit"
import { PaginatedResponse, User, UserFilter, UserSort } from "../../../types"
import { fetchUsersService, addUserService, editUserService, removeUserService } from "../../../services"

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
	}): Promise<PaginatedResponse<User>> => fetchUsersService(pageNumber, pageSize, searchTerm, filters, sort)
)

// Add User
export const addUser = createAsyncThunk(
	"users/addUser",
	async (user: Partial<FormData>): Promise<FormData> => addUserService(user)
)

// Edit User
export const editUser = createAsyncThunk(
	"users/editUser",
	async ({ id, user }: { id: string; user: Partial<FormData> }): Promise<FormData> =>
		await editUserService(id, user)
)

// Remove User
export const removeUser = createAsyncThunk(
	"users/removeUser",
	async ({ id, imageUrl }: { id: string; imageUrl: string }): Promise<string> => {
		await removeUserService(id, imageUrl)
		return id
	}
)

const initialState = {
	data: [] as User[],
	loading: false,
	error: null as string | null,
	operationStatus: null as "success" | "error" | "pending" | null,
	pagination: {
		pageNumber: 1,
		pageSize: 10,
		totalCount: 0,
	},
	searchTerm: "",
	filters: {} as UserFilter,
	sort: {} as UserSort,
}

const userSlice = createSlice({
	name: "users",
	initialState,
	reducers: {
		setSearchTerm: (state, action: PayloadAction<string>) => {
			state.searchTerm = action.payload
		},
		setFilters: (state, action: PayloadAction<UserFilter>) => {
			state.filters = action.payload
		},
		setSort: (state, action: PayloadAction<keyof UserSort>) => {
			const field = action.payload
			state.sort = {
				[field]: state.sort[field] === true ? false : state.sort[field] === false ? undefined : true,
			}
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
				state.error = action.error.message ?? "Failed to fetch users."
			})

			// Add User
			.addCase(addUser.pending, (state) => {
				state.operationStatus = "pending"
			})
			.addCase(addUser.fulfilled, (state, action) => {
				state.operationStatus = "success"
				const extractedUser =
					action.payload instanceof FormData
						? (Object.fromEntries(action.payload.entries()) as unknown as User)
						: (action.payload as User)
				state.data.push(extractedUser)
			})
			.addCase(addUser.rejected, (state, action) => {
				state.operationStatus = "error"
				state.error = action.error.message ?? "Failed to add user."
			})

			// Edit User
			.addCase(editUser.pending, (state) => {
				state.operationStatus = "pending"
			})
			.addCase(editUser.fulfilled, (state, action) => {
				state.operationStatus = "success"
				const extractedUser =
					action.payload instanceof FormData
						? (Object.fromEntries(action.payload.entries()) as unknown as User)
						: (action.payload as User)
				const index = state.data.findIndex((user) => user.id === extractedUser.id)
				if (index !== -1) state.data[index] = extractedUser
			})
			.addCase(editUser.rejected, (state, action) => {
				state.operationStatus = "error"
				state.error = action.error.message ?? "Failed to update user."
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
				state.error = action.error.message ?? "Failed to delete user."
			})
	},
})

export const { setSearchTerm, setFilters, setSort, resetOperationStatus } = userSlice.actions

export default userSlice.reducer
