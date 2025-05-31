import { OrderWithUserName } from "@/types/types/order/OrderWithUserName";
import { createOrder, deleteOrder, GetAllOrdersWithUserName, getOrderById, updateOrder } from "../api/repositories/orderRepository";
import { Order, OrderFilter, OrderSort, PaginatedResponse, ServiceResponse, Bool } from "../types";


export const fetchOrdersService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    filters?: OrderFilter,
    sort?: OrderSort
): Promise<ServiceResponse<PaginatedResponse<OrderWithUserName>>> => {
    const response: ServiceResponse<PaginatedResponse<OrderWithUserName>> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        const defaultFilter: OrderFilter = {
            orderDateStart: null,
            orderDateEnd: null,
            deliveryDateStart: null,
            deliveryDateEnd: null,
            status: null,
            deliveryId: null,
            userId: null
        };

        const defaultSort = {
            orderDate: Bool.NULL,
            orderPrice: Bool.NULL,
            deliveryDate: Bool.NULL,
        } as OrderSort;

        const body = {
            query: `
            query GetAllOrdersWithUserName(
            $pageNumber: Int!,
          $pageSize: Int!,
          $searchTerm: String,
          $filter: OrderFilterInput!,
          $sort: OrderSortInput!)
            {
                    allOrdersWithUserName(
                        pageNumber: $pageNumber,
                        pageSize: $pageSize,
                        searchTerm: $searchTerm,
                        filter: $filter,
                        sort: $sort
                    ) {
                        items {
                            orderUiId
                            price
                            status
                            firstName
                            lastName
                        }
                        pageNumber
                        pageSize
                        totalCount
                        totalPages
                    }
            }
            `,
            variables: {
                pageNumber,
                pageSize,
                searchTerm: searchTerm ?? null,
                filter: {
                    ...defaultFilter,
                    ...filters,
                },
                sort: { ...defaultSort, ...sort },
            },
        };

        const graphQLResponse = await GetAllOrdersWithUserName(body);
        if (graphQLResponse.errors)
            throw new Error(
                `${graphQLResponse.errors[0].message} Status code: ${graphQLResponse.errors[0].extensions?.status}`,
            );
        console.log(graphQLResponse.data);
        response.data = graphQLResponse.data
            ?.allOrdersWithUserName as PaginatedResponse<OrderWithUserName>;
    }
    catch (error) {
        console.error(error instanceof Error ? error.message : String(error));
        response.error =
            'An error occurred while fetching orders. Please try again later.';
    } finally {
        response.loading = false;
    }
    console.log(response)
    return response;
}

export const fetchOrderByIdService = async (id: string): Promise<ServiceResponse<Order>> => {
    const response: ServiceResponse<Order> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        const body = {
            query: `
                query(
                    $id: UUID!
                ) {
                    order(id: $id) {
                        id
                        userId
                        books {
                            key
                            value
                        }
                        region
                        city
                        address
                        price
                        deliveryTypeId
                        deliveryPrice
                        orderDate
                        deliveryDate
                        status
                    }
                }
            `,
            variables: {
                id,
            },
        };

        const graphQLResponse = await getOrderById(body);
        if (graphQLResponse.errors)
            throw new Error(
                `${graphQLResponse.errors[0].message} Status code: ${graphQLResponse.errors[0].extensions?.status}`,
            );
        response.data = graphQLResponse.data?.order as Order;
    } catch (error) {
        console.error(`Failed to fetch order ID [${id}]`, error);
        response.error =
            'An error occurred while fetching order. Please try again later.';
    } finally {
        response.loading = false;
    }
    return response;
}

export const addOrderService = async (order: Order): Promise<ServiceResponse<Order>> => {
    const response: ServiceResponse<Order> = {
        data: null,
        loading: true,
        error: null
    };

    try {
        console.log(order);
        response.data = await createOrder(order);
    } catch (error) {
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
        data: null,
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