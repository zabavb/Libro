import axios from "axios";
import { ORDERS, ORDER_BY_ID, GRAPHQL } from "..";
import { Order, PaginatedResponse, OrderFilter, OrderSort, GraphQLResponse } from "../../types";
import { getAuthHeaders } from "./common";


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
        headers: getAuthHeaders(),
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
        headers: getAuthHeaders(),
    });
    return response.data;
};

export const createOrder = async (order: Partial<Order>): Promise<Order> => {
    const response = await axios.post(ORDERS, order);
    return response.data;
}

export const updateOrder = async (id: string, order: Partial<Order>): Promise<Order> => {
    const response = await axios.put(ORDER_BY_ID(id), order);
    return response.data;
}

export const deleteOrder = async (id: string): Promise<void> => {
    await axios.delete(ORDER_BY_ID(id));
}