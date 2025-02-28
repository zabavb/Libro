import { statusEnumToNumber } from "../api/adapters/orderAdapters";
import { createOrder, deleteOrder, getAllOrders, getOrderById, updateOrder } from "../api/repositories/orderRepository";
import { Order, OrderFilter, OrderSort, PaginatedResponse } from "../types";


export const fetchOrdersService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    filters?: OrderFilter,
    sort?: OrderSort
): Promise<PaginatedResponse<Order>> => {
    const formattedSort = Object.fromEntries(
        Object.entries(sort || {}).map(([key, value]) => [key, value ? 1 : 2])
    )

    const params = {
        searchTerm,
        ...filters,
        status: filters?.status !== undefined ? statusEnumToNumber(filters.status).toString() : undefined,
        ...formattedSort,
    }

    try {
        return await getAllOrders(pageNumber, pageSize, params)
    } catch (error){
        throw new Error(`Error fetching orders: ${error}`)
    }
}

export const fetchOrderByIdService = async (id: string) : Promise<Order> => {
    try { 
        return await getOrderById(id)
    } catch (error) {
        throw new Error(`Error fetching order by ID: ${error}`)
    }
} 

export const addOrderService = async (order:Partial<Order>): Promise<Order> => {
    try {
        return await createOrder(order)
    } catch(error) {
        throw new Error(`Error adding order: ${error}`)
    }
}

export const editOrderService = async (id: string, order: Partial<Order>): Promise<Order> => {
    try {
        return await updateOrder(id, order)
    } catch (error){
        throw new Error(`Error updating order: ${error}`)
    }
}

export const removeOrderService = async (id: string): Promise<void> => {
    try{
        await deleteOrder(id)
    } catch (error){
        throw new Error(`Error deleting order: ${error}`)
    }
}