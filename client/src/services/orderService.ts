import { OrderCard } from "@/types/types/order/OrderCard";
import { statusEnumToNumber } from "../api/adapters/orderAdapters";
import { createOrder, deleteOrder, getAllOrders, getOrderById, updateOrder } from "../api/repositories/orderRepository";
import { Order, OrderFilter, OrderSort, PaginatedResponse, ServiceResponse } from "../types";


export const fetchOrdersService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    filters?: OrderFilter,
    sort?: OrderSort
): Promise<ServiceResponse<PaginatedResponse<OrderCard>>> => {
    const response: ServiceResponse<PaginatedResponse<OrderCard>> = {
        data: null,
        loading: true,
        error: null,
    };

    try{
        const formattedSort = Object.fromEntries(
            Object.entries(sort || {}).map(([key, value]) => [key, value ? 1 : 2])
        )
    
        const params = {
            searchTerm,
            ...filters,
            status: filters?.status !== undefined ? statusEnumToNumber(filters.status).toString() : undefined,
            ...formattedSort,
        }

        response.data = await getAllOrders(pageNumber, pageSize, params)
    }
    catch(error) {
        console.error('Failed to fetch orders', error);
        response.error =
            'An error occurred while fetching orders. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;

}

export const fetchOrderByIdService = async (id: string) : Promise<ServiceResponse<Order>> => {
    const response: ServiceResponse<Order> = {
        data:null,
        loading:true,
        error: null,
    };

    try { 
        response.data = await getOrderById(id);
    } catch(error){
        console.error(`Failed to fetch order ID [${id}]`, error);
        response.error =
            'An error occurred while fetching order. Please try again later.';
    } finally{
        response.loading = false;
    }
    return response;
} 

export const addOrderService = async (order:Partial<Order>): Promise<ServiceResponse<Order>> => {
    const response: ServiceResponse<Order> = {
        data:null,
        loading: true,
        error: null
    };

    try {
        response.data = await createOrder(order);
    }catch (error){
        console.error('Failed to create order', error);
        response.error = 
            'An error occurred while adding the order. Please try again later.';
    } finally {
        response.loading = false;
    }
    return response;
}

export const editOrderService = async (
    id: string,
    order: Partial<Order>
): Promise<ServiceResponse<Order>> => {
    const response: ServiceResponse<Order> = {
        data:null,
        loading: true,
        error: null,
    }

    try {
        response.data = await updateOrder(id, order);
    } catch (error) {
        console.error(`Failed to update order ID [${id}]`, error);
        response.error =
            'An error occurred while updating the order. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response
}

export const removeOrderService = async (id: string): Promise<ServiceResponse<string>> => {
    const response: ServiceResponse<string> = {
        data: null,
        loading: true,
        error: null
    };

    try {
        await deleteOrder(id);
        response.data = id;
    } catch (error) {
        console.error(`Failed to delete order ID [${id}]`, error);
        response.error =
            'An error occurred while deleting the order. Please try again later.';
    } finally {
        response.loading = false;
    }
    return response;
}