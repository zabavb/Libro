import axios from "axios";
import { ORDERS_PAGINATED, ORDERS, ORDER_BY_ID, GET_ORDER_COUNTS } from "..";
import { Order, PaginatedResponse } from "../../types";
import { PeriodType } from "@/types/types/order/PeriodType";  

export const getAllOrders = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    params:{
        status: string | undefined
        OrderDateStart?: string
        OrderDateEnd?: string
        DeliveryDateStart?: string
        DeliveryDateEnd?: string
        DeliveryId?: string
        UserId?: string
        searchTerm: string | undefined
    }
): Promise<PaginatedResponse<Order>> => {
    const url = ORDERS_PAGINATED(pageNumber,pageSize)
    const response = await axios.get<PaginatedResponse<Order>>(url, {params})
    return response.data
}

export const getOrderById = async (id: string): Promise<Order> => {
    const response = await axios.get(ORDER_BY_ID(id))
    return response.data
}

export const createOrder = async (order: Partial<Order>): Promise<Order> => {
    const response = await axios.post(ORDERS, order)
    return response.data
}

export const updateOrder = async (id: string, order: Partial<Order>): Promise<Order> => {
    const response = await axios.put(ORDER_BY_ID(id), order);
    return response.data
}

export const deleteOrder = async (id: string): Promise<void> => {
    await axios.delete(ORDER_BY_ID(id))
}

export const getOrderCounts = async (period: PeriodType): Promise<number[]> => {
    console.log('AAACounts:dsfdsdfssdffdssd');
    const response = await axios.get<number[]>(GET_ORDER_COUNTS(period));
    console.log('BBBCounts:dsfdsdfssdffdssd');
    return response.data;
};