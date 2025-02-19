import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit";
import { Order, OrderFilter, OrderSort } from "../../../types";
import { addOrderService, editOrderService, fetchOrdersService, removeOrderService } from "../../../services/orderService";

export const fetchOrders = createAsyncThunk(
    "orders/fetchOrders",
    async ({
        pageNumber = 1,
        pageSize = 10,
        searchTerm,
        filters,
        sort,
    }:{
        pageNumber?: number
        pageSize?: number
        searchTerm?: string
        filters?: OrderFilter
        sort?: OrderSort
    }) => {
        return await fetchOrdersService(pageNumber, pageSize, searchTerm, filters, sort)
    }
)

export const addOrder = createAsyncThunk("orders/addOrder", async (order: Partial<Order>) => {
    return await addOrderService(order)
})

export const editOrder = createAsyncThunk("orders/editOrder",
    async ({ id, order }: {id: string; order: Partial<Order>}) => {
        return await editOrderService(id, order)
    }
)

export const removeOrder = createAsyncThunk("orders/removeOrder", async (id: string) => {
    await removeOrderService(id)
    return id
})

const orderSlice = createSlice({
    name: "orders",
    initialState: {
        data: [] as Order[],
        loading: false,
        error: null as string | null | undefined,
        operationStatus: null as "success" | "error" | "pending" | null,
        pagination: {
            pageNumber: 1,
            pageSize: 10,
            totalCount: 0,
        },
        searchTerm: "",
        filters: {} as OrderFilter,
        sort: {} as OrderSort,
    },
    reducers: {
        setOrderSearchTerm: (state, action) => {
            state.searchTerm = action.payload
        },
        setOrderFilters: (state, action) => {
            state.filters = action.payload
        },
        setOrderSort: (state, action: PayloadAction<keyof OrderSort>) => {
            const field = action.payload
            const currentSort = state.sort[field]
            const newSort = currentSort === undefined ? true : currentSort === true ? false : undefined
            state.sort = { [field]: newSort }
        },
        resetOrderOperationStatus: (state) => {
            state.operationStatus = null
        }
    },
    extraReducers: (builder) => {
        builder
            // Fetch Orders
            .addCase(fetchOrders.pending, (state) => {
                state.loading = true
                state.error = null
            })
            .addCase(fetchOrders.fulfilled, (state, action) => {
                state.loading = false
                state.data = action.payload.items
                state.pagination = {
                    pageNumber: action.payload.pageNumber,
                    pageSize: action.payload.pageSize,
                    totalCount: action.payload.totalCount
                }
            })
            .addCase(fetchOrders.rejected, (state, action) => {
                state.loading = false
                state.error = action.error.message
            })
            // Add Order
            .addCase(addOrder.pending, (state) => {
                state.operationStatus = "pending"
            })
            .addCase(addOrder.fulfilled, (state, action) => {
                state.operationStatus = "success"
                state.data.push(action.payload)
            })
            .addCase(addOrder.rejected, (state, action) => {
                state.operationStatus = "error"
                state.error = action.error.message
            })
            // Edit Order
            .addCase(editOrder.pending, (state) => {
                state.operationStatus = "pending"
            })
            .addCase(editOrder.fulfilled, (state, action) => {
                state.operationStatus = "success"
                const index = state.data.findIndex((order) => order.id === action.payload.id)
                if (index !== -1) state.data[index] = action.payload
            })
            .addCase(editOrder.rejected, (state, action) => {
                state.operationStatus = "error"
                state.error = action.error.message
            })
            // Remove Order
            .addCase(removeOrder.pending, (state) => {
                state.operationStatus = "pending"
            })
            .addCase(removeOrder.fulfilled, (state, action) => {
                state.operationStatus = "success"
                state.data = state.data.filter((order) => order.id !== action.payload)
            })
            .addCase(removeOrder.rejected, (state, action) =>{
                state.operationStatus = "error"
                state.error = action.error.message
            })
    }
})

export const { setOrderSearchTerm, setOrderFilters, setOrderSort, resetOrderOperationStatus } = orderSlice.actions
export default orderSlice.reducer;