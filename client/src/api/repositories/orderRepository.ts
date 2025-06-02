import axios from "axios";
import { ORDERS, ORDER_BY_ID, GET_ORDER_COUNTS, GRAPHQL } from "..";
import { Order, PaginatedResponse, OrderFilter, OrderSort, GraphQLResponse} from "../../types";
import { PeriodType } from "@/types/types/order/PeriodType"; 
import { getAuthHeaders } from "./common";
import { OrderWithUserName } from "@/types/types/order/OrderWithUserName";
import { OrderDetails } from "@/types/types/order/OrderDetails";


export const GetAllOrdersWithUserName = async (body: {
    query: string;
    variables: {
        pageNumber: number;
        pageSize: number;
        searchTerm: string | null;
        filter: OrderFilter;
        sort: OrderSort;
    };
}): Promise<GraphQLResponse<{ allOrdersWithUserName: PaginatedResponse<OrderWithUserName> }>> => {
    const response = await axios.post<GraphQLResponse<{ allOrdersWithUserName: PaginatedResponse<OrderWithUserName> }>>(GRAPHQL, body, {
        headers: getAuthHeaders("application/json"),
    });
    return response.data;
}

export const getAllOrders = async (body: {
    query: string;
    variables: {
        pageNumber: number;
        pageSize: number;
        searchTerm: string | null;
        filter: OrderFilter;
        sort: OrderSort;
    };
}): Promise<GraphQLResponse<{ allOrders: PaginatedResponse<Order> }>> => {
    const response = await axios.post<GraphQLResponse<{ allOrders: PaginatedResponse<Order> }>>(GRAPHQL, body, {
        headers: getAuthHeaders("application/json"),
    });
    return response.data;
}

export const getOrderById = async (body: {
    query: string;
    variables: {
        id: string;
    };
}): Promise<GraphQLResponse<{ order: Order }>> => {
    const response = await axios.post<GraphQLResponse<{ order: Order }>>(GRAPHQL, body, {
        headers: getAuthHeaders("application/json"),
    });
    return response.data;
};

export const createOrder = async (order: Partial<Order>): Promise<Order> => {
    const response = await axios.post<Order>(ORDERS, order);
    return response.data;
}

export const updateOrder = async (id: string, order: Partial<Order>): Promise<Order> => {
    const response = await axios.put<Order>(ORDER_BY_ID(id), order);
    return response.data;
}

export const deleteOrder = async (id: string): Promise<void> => {
    await axios.delete(ORDER_BY_ID(id))
}

export const getOrderCounts = async (period: PeriodType): Promise<number[]> => {
    const response = await axios.get<number[]>(GET_ORDER_COUNTS(period));
    return response.data;
};

export const getAllOrderDetails= async (body: {
    query: string;
    variables: {
        pageNumber: number;
        pageSize: number;
        searchTerm: string | null;
        filter: OrderFilter;
        sort: OrderSort;
    };
}): Promise<GraphQLResponse<{ allOrderDetails: PaginatedResponse<OrderDetails> }>> => {
    const response = await axios.post<GraphQLResponse<{ allOrderDetails: PaginatedResponse<OrderDetails> }>>(GRAPHQL, body,{
        headers: getAuthHeaders("application/json"),
    });
    return response.data;
};