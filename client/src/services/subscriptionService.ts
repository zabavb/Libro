import axios from "axios";
import { Subscription, SubscriptionPlan, PaginatedResponse } from "../types";

const API_URL = "https://your-api.com/api/subscriptions"; 


export const fetchActiveSubscriptionsCount = async (): Promise<number> => {
    const res = await axios.get(API_URL);
    return res.data;
};

export const fetchSubscriptionsService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    plan?: SubscriptionPlan
): Promise<PaginatedResponse<Subscription>> => {
    const params = {
        searchTerm,
        plan,
        page: pageNumber,
        size: pageSize,
    };

    try {
        const response = await axios.get(API_URL, { params });
        return response.data;
    } catch (error) {
        throw new Error(`Error fetching subscriptions: ${error}`);
    }
};

export const fetchSubscriptionByIdService = async (id: string): Promise<Subscription> => {
    try {
        const response = await axios.get(`${API_URL}/${id}`);
        return response.data;
    } catch (error) {
        throw new Error(`Error fetching subscription by ID: ${error}`);
    }
};

export const addSubscriptionService = async (subscription: Partial<Subscription>): Promise<Subscription> => {
    try {
        const response = await axios.post(API_URL, subscription);
        return response.data;
    } catch (error) {
        throw new Error(`Error adding subscription: ${error}`);
    }
};

export const editSubscriptionService = async (id: string, subscription: Partial<Subscription>): Promise<Subscription> => {
    try {
        const response = await axios.put(`${API_URL}/${id}`, subscription);
        return response.data;
    } catch (error) {
        throw new Error(`Error updating subscription: ${error}`);
    }
};

export const removeSubscriptionService = async (id: string): Promise<void> => {
    try {
        await axios.delete(`${API_URL}/${id}`);
    } catch (error) {
        throw new Error(`Error deleting subscription: ${error}`);
    }
};
