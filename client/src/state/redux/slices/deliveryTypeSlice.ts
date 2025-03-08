import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { addDeliveryTypeService, editDeliveryTypeService, fetchDeliveryTypesService, removeDeliveryTypeService } from "../../../services";
import { DeliverySort, DeliveryType } from "../../../types";

export const fetchDeliveryTypes = createAsyncThunk(
    "deliverytypes/fetchDeliveryTypes",
    async({
        pageNumber = 1,
        pageSize = 10,
        searchTerm,
        sort
    }:{
        pageNumber?: number
        pageSize?: number
        searchTerm?: string
        sort?: DeliverySort
    }) => {
        return await fetchDeliveryTypesService(pageNumber, pageSize, searchTerm, sort)
    }
)

export const addDeliveryType = createAsyncThunk("deliverytypes/addDeliveryTypes",
    async (deliveryType: Partial<DeliveryType>
    ) => {
    return await addDeliveryTypeService(deliveryType)
})

export const editDeliveryType = createAsyncThunk("deliverytypes/editDeliveryTypes",
    async({id, deliveryType}: {id: string; deliveryType: Partial<DeliveryType>}) => {
        return await editDeliveryTypeService(id,deliveryType)
    }
)

export const removeDeliveryType = createAsyncThunk("deliverytypes/removeDeliveryType",
    async(id: string) => {
        await removeDeliveryTypeService(id)
        return id
    }
)

const deliveryTypeSlice = createSlice({
    name: "deliverytypes",
    initialState: {
        data: [] as DeliveryType[],
        loading: false,
        error: null as string | null ,
        operationStatus: null as "success" | "error" | "pending" | null,
        pagination: {
            pageNumber: 1,
            pageSize: 10,
            totalCount: 0
        },
        searchTerm: "",
        sort: {} as DeliverySort
    },
    reducers: {
        resetDeliveryTypeOperationStatus: (state) => {
            state.operationStatus = null
        },
        setDeliverySearchTerm: (state,action) => {
            state.searchTerm = action.payload
        },
        setDeliverySort: (state, action: PayloadAction<keyof DeliverySort>) => {
            const field = action.payload
            const currentSort = state.sort[field]
            const newSort = currentSort === undefined ? true : currentSort === true ? false: undefined
            state.sort = { [field]: newSort }
        }
    },
    extraReducers: (builder) => {
        builder
        // Fetch delivery types
            .addCase(fetchDeliveryTypes.pending, (state) => {
                state.loading = true
                state.error = null
            })
            .addCase(fetchDeliveryTypes.fulfilled, (state, action) => {
                state.loading = false
                state.data = action.payload.items
                state.pagination = {
                    pageNumber: action.payload.pageNumber,
                    pageSize: action.payload.pageSize,
                    totalCount: action.payload.totalCount
                }
            })
            .addCase(fetchDeliveryTypes.rejected, (state, action) => {
                state.loading = false
                state.error = action.error.message ?? "Failed to fetch delivery types."
            })
            // Add delivery type
            .addCase(addDeliveryType.pending, (state) => {
                state.operationStatus = "pending"
            })
            .addCase(addDeliveryType.fulfilled, (state, action) => {
                state.operationStatus = "success"
                state.data.push(action.payload)
            })
            .addCase(addDeliveryType.rejected, (state, action) => {
                state.operationStatus = "error"
                state.error = action.error.message ?? "Failed to add delivery type."
            })
            // Edit delivery type
            .addCase(editDeliveryType.pending, (state) => {
                state.operationStatus = "pending"
            })
            .addCase(editDeliveryType.fulfilled, (state, action) => {
                state.operationStatus = "success"
                const index = state.data.findIndex((order) => order.id === action.payload.id)
                if (index !== -1) state.data[index] = action.payload
            })
            .addCase(editDeliveryType.rejected, (state, action) => {
                state.operationStatus = "error"
                state.error = action.error.message ?? "Failed to update delivery type."
            })
            // remove delivery type
            .addCase(removeDeliveryType.pending, (state) => {
                state.operationStatus = "pending"
            })
            .addCase(removeDeliveryType.fulfilled, (state, action) => {
                state.operationStatus = "success"
                state.data = state.data.filter((order) => order.id !== action.payload)
            })
            .addCase(removeDeliveryType.rejected, (state, action) =>{
                state.operationStatus = "error"
                state.error = action.error.message ?? "Failed to delete delivery type."
            })
    }
})

export const {setDeliverySearchTerm, setDeliverySort, resetDeliveryTypeOperationStatus} = deliveryTypeSlice.actions
export default deliveryTypeSlice.reducer