import { PaginatedResponse } from "@/types"
import { Publisher } from "@/types/types/book/Publisher"
import { PUBLISHER_BY_ID, PUBLISHERS, PUBLISHERS_PAGINATED } from ".."
import axios from "axios"

interface PublisherQueryPatams {
    searchTerm?: string;
}

/**
 * Fetch a paginated list of publishers.
 */
export const getAllPublishers = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    params: PublisherQueryPatams = {},
): Promise<PaginatedResponse<Publisher>> => {
    const url = PUBLISHERS_PAGINATED(pageNumber, pageSize)
    const response = await axios.get<PaginatedResponse<Publisher>>(url, { params })
    
    console.log(response)
    return response.data
}

/**
 * Fetch a single publisher by its ID.
 */
export const getPublisherById = async (id: string): Promise<Publisher> => {
    const response = await axios.get<Publisher>(PUBLISHER_BY_ID(id));
    return response.data;
};

/**
 * Create a new publisher.
 */
export const addPublisher = async (author: Partial<Publisher>): Promise<Publisher> => {
    const response = await axios.post<Publisher>(PUBLISHERS, author);
    return response.data;
};

/**
 * Update an existing publisher by ID.
 */
export const updatePublisher = async (id: string, updatedPublisher: Partial<Publisher>): Promise<Publisher> => {
    const response = await axios.put<Publisher>(PUBLISHER_BY_ID(id), updatedPublisher);
    return response.data;
};

/**
 * Delete a publisher by ID.
 */
export const deletePublisher = async (id: string): Promise<void> => {
    await axios.delete(PUBLISHER_BY_ID(id));
};

