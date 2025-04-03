import { createDeliveryType, deleteDeliveryType, getAllDeliveryTypes, getDeliveryTypeById, updateDeliveryType } from "../api/repositories/deliveryTypeRepository";
import { DeliverySort, DeliveryType, PaginatedResponse, ServiceResponse } from "../types";

export const fetchDeliveryTypesService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    sort?: DeliverySort
): Promise<ServiceResponse<PaginatedResponse<DeliveryType>>> => {
    const response: ServiceResponse<PaginatedResponse<DeliveryType>> = {
        data: null,
        loading: true,
        error: null
    }

    try{
    
    const formattedSort = Object.fromEntries(
        Object.entries(sort || {}).map(([key,value]) => [key, value ? 1 : 2])
    )

    const params = {
        searchTerm,
        ...formattedSort
    }


    response.data = await getAllDeliveryTypes(pageNumber, pageSize, params);
    }catch(error){
        console.error('Failed to fetch delivery types', error);
        response.error =
            'An error occurred while fetching delivery types. Please try again later.';
    } finally{
        response.loading = false;
    }

    return response;
}

export const fetchDeliveryTypeByIdService = async (id: string) : Promise<ServiceResponse<DeliveryType>> => {
    const response: ServiceResponse<DeliveryType> = {
        data: null,
        loading: true,
        error: null,
    }
    // empty id calls for GetAll function of the controller, this if exists to filter it out
    if(id == "")
        return response

    try{
        response.data = await getDeliveryTypeById(id);
    } catch (error){
        console.error(`Failed to fetch delivery type ID [${id}]`, error);
        response.error = 
            'An error occurred while fetching delivery type. Please try again later.';
    } finally{
        response.loading = false;
    }
    return response;
}

export const addDeliveryTypeService = async (deliveryType: Partial<DeliveryType>): Promise<ServiceResponse<DeliveryType>> => {
    const response: ServiceResponse<DeliveryType> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await createDeliveryType(deliveryType);
    } catch (error){
        console.error('Failed to create delivery type', error)
        response.error = 
            'An error occurred while adding the order. Please try again later.';
    } finally {
        response.loading = false;
    }
    return response;
}

export const editDeliveryTypeService = async (
    id: string,
    deliveryType: Partial<DeliveryType>): Promise<ServiceResponse<DeliveryType>> => {
    const response: ServiceResponse<DeliveryType> = {
        data: null,
        loading: true,
        error: null
    }

    
    try {
        response.data = await updateDeliveryType(id,deliveryType)
        console.log(response)
    } catch (error){
        console.error(`Failed to update delivery type ID [${id}]`, error);
        response.error = 'An error occurred while updating the delivery type. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
}

export const removeDeliveryTypeService = async (id: string): Promise<ServiceResponse<string>> => {
    const response: ServiceResponse<string> = {
        data: null,
        loading: true,
        error:null
    };

    try {
        await deleteDeliveryType(id);
        response.data = id;
    } catch (error) {
        console.error(`Failed to delete delivery type ID [${id}]`, error)
    } finally {
        response.loading = false;
    }
    return response;
}