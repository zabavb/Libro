import { createOrder, deleteOrder, getAllOrders, getOrderById, updateOrder } from "../api/repositories/orderRepository";
import { Order, OrderFilter, OrderSort, PaginatedResponse, ServiceResponse, Bool } from "../types";


export const fetchOrdersService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    filters?: OrderFilter,
    sort?: OrderSort
): Promise<ServiceResponse<PaginatedResponse<Order>>> => {
    const response: ServiceResponse<PaginatedResponse<Order>> = {
        data: null,
        loading: true,
        error: null,
    };

    try{
        const defaultFilter: OrderFilter = {
            orderDateStart: undefined,
            orderDateEnd: undefined,
            deliveryDateStart: undefined,
            deliveryDateEnd: undefined,
            status: undefined,
            deliveryId: undefined,
            userId: undefined
        };

        const defaultSort = {
            orderDate: Bool.NULL,
            orderPrice: Bool.NULL,
            deliveryDate: Bool.NULL,
        } as OrderSort;

        const body = {
            query: `
            query AllOrders($pageNumber: Int!, $pageSize: Int!,
                $searchTerm: String, $filter: OrderFilter, $sort: OrderSort)
            {
                    allOrders(pageNumber: $pageNumber, pageSize: $pageSize,
                        searchTerm: $searchTerm, filter: $filter, sort: $sort)
                    {
                        items {
                            id
                            orderDate
                            deliveryDate
                            status
                            totalPrice
                            userId
                            deliveryId
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

        const graphQLResponse = await getAllOrders(body);
        if (graphQLResponse.errors)
            throw new Error(
                `${graphQLResponse.errors[0].message} Status code: ${graphQLResponse.errors[0].extensions?.status}`,
            );

        response.data = graphQLResponse.data
            ?.allOrders as PaginatedResponse<Order>;
    }
    catch(error) {
        console.error(error instanceof Error ? error.message : String(error));
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
        const body = {
            query: `
                query Order($id: String!) {
                    order(id: $id) {
                        id
                        orderDate
                        deliveryDate
                        status
                        totalPrice
                        userId
                        deliveryId
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