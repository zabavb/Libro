import axios from "axios";
import { DELIVERYTYPE_BY_ID, DELIVERYTYPES, DELIVERYTYPES_PAGINATED } from "..";
import { DeliveryType, PaginatedResponse } from "../../types";

export const getAllDeliveryTypes = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    params:{
        searchTerm: string | undefined
    }
): Promise<PaginatedResponse<DeliveryType>> => {
    const url = DELIVERYTYPES_PAGINATED(pageNumber, pageSize)
    const response = await axios.get<PaginatedResponse<DeliveryType>>(url, {params})
    return response.data
}

export const getDeliveryTypeById = async (id: string): Promise<DeliveryType> => {
    const response = await axios.get(DELIVERYTYPE_BY_ID(id))
    return response.data
}

export const createDeliveryType = async (deliveryType: Partial<DeliveryType>): Promise<DeliveryType> => {
    const response = await axios.post(DELIVERYTYPES, deliveryType)
    return response.data
}

export const updateDeliveryType = async (id: string, deliveryType: Partial<DeliveryType>): Promise<DeliveryType> => {
    const response = await axios.put(DELIVERYTYPE_BY_ID(id), deliveryType)
    return response.data
}

export const deleteDeliveryType = async (id: string): Promise<void> => {
    await axios.delete(DELIVERYTYPE_BY_ID(id))
}