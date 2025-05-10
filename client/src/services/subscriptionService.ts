import {
  createSubscription,
  deleteSubscription,
  getAllSubscriptions,
  getSubscriptionById,
  subscribe,
  subscriptionsforFiltering,
  unsubscribe,
  updateSubscription,
} from '../api/repositories/subscriptionRepository';
import {
  BySubscription,
  PaginatedResponse,
  ServiceResponse,
  SubscribeRequest,
  Subscription,
  SubscriptionCard,
} from '../types';

/**
 * Fetch a number of active subscriptions.
 */
export const fetchActiveSubscriptionsCount = async (): Promise<number> => {
    const res = await axios.get(API_URL);
    return res.data;
};

/**
 * Fetch a paginated list of subscriptions with optional search term.
 */
export const fetchSubscriptionsService = async (
  pageNumber: number = 1,
  pageSize: number = 10,
  searchTerm?: string,
): Promise<ServiceResponse<PaginatedResponse<SubscriptionCard>>> => {
  const response: ServiceResponse<PaginatedResponse<SubscriptionCard>> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    response.data = await getAllSubscriptions(pageNumber, pageSize, searchTerm);
  } catch (error) {
    console.error('Failed to fetch subscriptions', error);
    response.error =
      'An error occurred while fetching subscriptions. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Fetch a single subscription by their ID.
 */
export const fetchSubscriptionByIdService = async (
  id: string,
): Promise<ServiceResponse<Subscription>> => {
  const response: ServiceResponse<Subscription> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    response.data = await getSubscriptionById(id);
  } catch (error) {
    console.error(`Failed to fetch subscription ID [${id}]`, error);
    response.error =
      'An error occurred while fetching the subscription. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Create a new subscription.
 */
export const addSubscriptionService = async (
  subscription: Partial<FormData>,
): Promise<ServiceResponse<FormData>> => {
  const response: ServiceResponse<FormData> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    response.data = await createSubscription(subscription);
  } catch (error) {
    console.error('Failed to create subscription', error);
    response.error =
      'An error occurred while adding the user. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Update an existing subscription by ID.
 */
export const editSubscriptionService = async (
  id: string,
  subscription: Partial<FormData>,
): Promise<ServiceResponse<FormData>> => {
  const response: ServiceResponse<FormData> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    response.data = await updateSubscription(id, subscription);
  } catch (error) {
    console.error(`Failed to update subscription ID [${id}]`, error);
    response.error =
      'An error occurred while updating the subscription. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Delete a subscription by ID.
 */
export const removeSubscriptionService = async (
  id: string,
): Promise<ServiceResponse<string>> => {
  const response: ServiceResponse<string> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    await deleteSubscription(id);
    response.data = id;
  } catch (error) {
    console.error(`Failed to delete subscription ID [${id}]`, error);
    response.error =
      'An error occurred while deleting the subscription. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Subscribe to a subscription by user's ID and subscription's ID.
 */
export const subscribeService = async (
  request: SubscribeRequest,
): Promise<ServiceResponse<Subscription>> => {
  const response: ServiceResponse<Subscription> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    await subscribe(request);
    if (!response.error) setSubscriptionInLocalStorage(request.subscriptionId);
  } catch (error) {
    console.error('Failed to subscribe', error);
    response.error =
      'An error occurred while subscribing. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Unsubscribe from a subscription by user's ID and subscription's ID.
 */
export const unSubscribeService = async (
  request: SubscribeRequest,
): Promise<ServiceResponse<Subscription>> => {
  const response: ServiceResponse<Subscription> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    await unsubscribe(request);
    if (!response.error)
      removeSubscriptionFromLocalStorage(request.subscriptionId);
  } catch (error) {
    console.error('Failed to unsubscribe', error);
    response.error =
      'An error occurred while unsubscribing. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

const setSubscriptionInLocalStorage = async (id: string): Promise<void> => {
  localStorage.setItem(
    'subscriptions',
    JSON.stringify([
      ...(JSON.parse(
        localStorage.getItem('subscriptions') || '[]',
      ) as string[]),
      id,
    ]),
  );
};

const removeSubscriptionFromLocalStorage = async (
  id: string,
): Promise<void> => {
  const editedSubscriptions = JSON.parse(
    localStorage.getItem('subscriptions') || '',
  ).filter((subscriptionId: string) => subscriptionId !== id);

  localStorage.setItem('subscriptions', JSON.stringify(editedSubscriptions));
};

/**
 * Fetches all subscriptions, reduced to very small size only for filtering
 */
export const subscriptionsforFilteringService = async (): Promise<
  ServiceResponse<BySubscription[]>
> => {
  const response: ServiceResponse<BySubscription[]> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    response.data = await subscriptionsforFiltering();
  } catch (error) {
    console.error('Failed to fetch', error);
    response.error =
      'An error occurred while fetching subscriptions for filtering. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};
