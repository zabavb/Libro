import { addPublisher, deletePublisher, getAllPublishers, getPublisherById, updatePublisher } from "@/api/repositories/publisherRepository";
import { PaginatedResponse, ServiceResponse } from "@/types";
import { Publisher } from "@/types/types/book/Publisher";

export const fetchPublishersService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
) : Promise<ServiceResponse<PaginatedResponse<Publisher>>> => {
    const response: ServiceResponse<PaginatedResponse<Publisher>> = {
        data:null,
        loading: true,
        error: null,
    };

    try {
        const params = {
            searchTerm,
        }
        response.data = await getAllPublishers(pageNumber, pageSize, params);
    }
    catch(error) {
          console.error('Failed to fetch publishers', error)
        response.error =
            'An error occured while fetching publishers. Please try again later';
    } finally {
        response.loading = false;
    }
    return response;
}

/**
 * Fetch a publisher by its ID.
 */
export const fetchPublisherByIdService = async (
    id: string
): Promise<ServiceResponse<Publisher>> => {
    const response: ServiceResponse<Publisher> = {
        data: null,
        loading: true,
        error:null,
    };

    try{
        response.data = await getPublisherById(id);
    } catch (error){
        console.error('Failed to fetch publisher with ID', {id, error});
        response.error = 
            'An error occurred while fetching the publisher. Please try again later.';
    } finally{
        response.loading = false;
    }
    return response;
};

/**
 * Add a new publisher.
 */
export const addPublisherService = async (
    publisher: Partial<Publisher>
): Promise<ServiceResponse<Publisher>> => {
    const response: ServiceResponse<Publisher> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await addPublisher(publisher);
    }catch(error) {
        console.error('Failed to create publisher', error);
        response.error = 
            'An error occurred while adding publisher. Please try again later.';
    }finally {
        response.loading = false;
    }

    return response;
}

/**
 * Update an existing publisher.
 */
export const editPublisherService = async (
    id: string,
    publisher: Partial<Publisher>
): Promise<ServiceResponse<Publisher>> => {
    const response: ServiceResponse<Publisher> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await updatePublisher(id,publisher);
    } catch(error){
        console.error('Failed to update publisher with ID', {id, error});
        response.error =
            'An error occurred while updating the publisher. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};

/**
 * Delete a publisher by its ID.
 */
export const removePublisherService = async (
    id: string
): Promise<ServiceResponse<string>> => {
    const response: ServiceResponse<string> = {
        data: null,
        loading: true,
        error:null,
    }

    try {
        await deletePublisher(id);
        response.data = id;
    } catch (error) {
        console.error('Failed to delete publisher with ID', {id,error});
        response.error =
            'An error occurred while deleting the publisher. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};