import { createDeliveryType, deleteDeliveryType, getAllDeliveryTypes, getDeliveryTypeById, updateDeliveryType } from "../api/repositories/deliveryTypeRepository";
import { DeliverySort, DeliveryType, PaginatedResponse } from "../types";

export const fetchDeliveryTypesService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    sort?: DeliverySort
): Promise<PaginatedResponse<DeliveryType>> => {
    const formattedSort = Object.fromEntries(
        Object.entries(sort || {}).map(([key,value]) => [key, value ? 1 : 2])
    )

    const params = {
        searchTerm,
        ...formattedSort
    }

    try{
        return await getAllDeliveryTypes(pageNumber, pageSize, params)
    } catch(error){
        throw new Error(`Error fetching delivery types: ${error}`)
    }
}

export const fetchDeliveryTypeByIdService = async (id: string) : Promise<DeliveryType> => {
    try {
        return await getDeliveryTypeById(id)
    } catch (error){
        throw new Error(`Error fetching delivery type by ID: ${error}`)
    }
}

export const addDeliveryTypeService = async (deliveryType: Partial<DeliveryType>): Promise<DeliveryType> => {
    try {
        return await createDeliveryType(deliveryType)
    } catch(error){
        throw new Error(`Error adding delivery type: ${error}`)
    }
}

export const editDeliveryTypeService = async (id: string, deliveryType: Partial<DeliveryType>): Promise<DeliveryType> => {
    try{
        return await updateDeliveryType(id, deliveryType)
    } catch (error) {
        throw new Error(`Error updating delivery type: ${error}`)
    }
}

export const removeDeliveryTypeService = async (id: string): Promise<void> => {
    try{
        await deleteDeliveryType(id)
    } catch (error){
        throw new Error(`Error deleting delivery type: ${error}`)
    }
}