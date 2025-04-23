import axios from 'axios';
import {
  BySubscription,
  PaginatedResponse,
  SubscribeRequest,
  Subscription,
  SubscriptionCard,
} from '../../types';
import { getAuthHeaders } from './common';
import {
  FOR_FILTERING,
  SUBSCRIBE,
  SUBSCRIPTION_BY_ID,
  SUBSCRIPTIONS,
  SUBSCRIPTIONS_PAGINATED,
  UNSUBSCRIBE,
} from '..';

/**
 * Fetch a paginated list of subscriptions with optional search term.
 */
export const getAllSubscriptions = async (
  pageNumber: number = 1,
  pageSize: number = 10,
  searchTerm?: string,
): Promise<PaginatedResponse<SubscriptionCard>> => {
  const url = SUBSCRIPTIONS_PAGINATED(pageNumber, pageSize);
  const response = await axios.get<PaginatedResponse<SubscriptionCard>>(url, {
    params: searchTerm,
    headers: getAuthHeaders(),
  });
  return response.data;
};

/**
 * Fetch a single subscription by their ID.
 */
export const getSubscriptionById = async (
  id: string,
): Promise<Subscription> => {
  const response = await axios.get<Subscription>(SUBSCRIPTION_BY_ID(id), {
    headers: getAuthHeaders(),
  });
  return response.data;
};

/**
 * Create a new subscription.
 */
export const createSubscription = async (
  subscription: Partial<FormData>,
): Promise<FormData> => {
  const response = await axios.post<FormData>(SUBSCRIPTIONS, subscription);
  return response.data;
};

/**
 * Update an existing subscription by ID.
 */
export const updateSubscription = async (
  id: string,
  subscription: Partial<FormData>,
): Promise<FormData> => {
  const response = await axios.put<FormData>(
    SUBSCRIPTION_BY_ID(id),
    subscription,
    {
      headers: getAuthHeaders(),
    },
  );
  return response.data;
};

/**
 * Delete a subscription by ID.
 */
export const deleteSubscription = async (id: string): Promise<void> => {
  const url = SUBSCRIPTION_BY_ID(id);
  const response = await axios.delete(url, {
    headers: getAuthHeaders(),
  });
  return response.data;
};

/**
 * Subscribe to a subscription.
 */
export const subscribe = async (request: SubscribeRequest): Promise<void> => {
  const response = await axios.post(SUBSCRIBE, request, {
    headers: getAuthHeaders(),
  });
  return response.data;
};

/**
 * Unsubscribe from a subscription.
 */
export const unsubscribe = async (request: SubscribeRequest): Promise<void> => {
  const response = await axios.post(UNSUBSCRIBE, request, {
    headers: getAuthHeaders(),
  });
  return response.data;
};

/**
 * Fetches all subscriptions, reduced to very small size only for filtering
 */
export const subscriptionsforFiltering = async (): Promise<
  BySubscription[]
> => {
  const response = await axios.get<BySubscription[]>(FOR_FILTERING, {
    headers: getAuthHeaders(),
  });
  return response.data;
};
