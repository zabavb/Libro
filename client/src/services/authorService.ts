import { PaginatedResponse, AuthorFilter, AuthorSort, ServiceResponse, Author } from "../types"
import {
    getAllAuthors,
    getAuthorById,
    addAuthor,
    updateAuthor,
    deleteAuthor
} from "../api/repositories/authorRepository"

export const fetchAuthorsService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    filters?: AuthorFilter,
    sort?: AuthorSort
): Promise<ServiceResponse<PaginatedResponse<Author>>> => {
    const response: ServiceResponse<PaginatedResponse<Author>> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        const formattedSort = Object.fromEntries(
            Object.entries(sort || {}).map(([Key, value]) => [Key, value ? 1 : 2])
        )

        const params = {
            searchTerm,
            ...filters,
            ...formattedSort,
        }
        response.data = await getAllAuthors(pageNumber, pageSize, params)
    }
    catch (error) {
        console.error('Failed to fetch authors', error)
        response.error =
            'An error occured while fetching authors. Please try again later';
    } finally {
        response.loading = false;
    }
    return response;
};

/**
 * Fetch a author by its ID.
 */
export const fetchAuthorByIdService = async (
    id: string
): Promise<ServiceResponse<Author>> => {
    const response: ServiceResponse<Author> = {
        data: null,
        loading: true,
        error:null,
    };

    try{
        response.data = await getAuthorById(id);
    } catch (error){
        console.error('Failed to fetch author with ID', {id, error});
        response.error = 
            'An error occurred while fetching the author. Please try again later.';
    } finally{
        response.loading = false;
    }
    return response;
};

/**
 * Add a new author.
 */
export const addAuthorService = async (
    author: FormData
): Promise<ServiceResponse<FormData>> => {
    const response: ServiceResponse<FormData> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await addAuthor(author);
    }catch(error) {
        console.error('Failed to create author', error);
        response.error = 
            'An error occurred while adding author. Please try again later.';
    }finally {
        response.loading = false;
    }

    return response;
}

/**
 * Update an existing author.
 */
export const updateAuthorService = async (
    id: string,
    author: FormData
): Promise<ServiceResponse<FormData>> => {
    const response: ServiceResponse<FormData> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await updateAuthor(id,author);
    } catch(error){
        console.error('Failed to update author with ID', {id, error});
        response.error =
            'An error occurred while updating the author. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};

/**
 * Delete a author by its ID.
 */
export const deleteAuthorService = async (
    id: string
): Promise<ServiceResponse<string>> => {
    const response: ServiceResponse<string> = {
        data: null,
        loading: true,
        error:null,
    }

    try {
        await deleteAuthor(id);
        response.data = id;
    } catch (error) {
        console.error('Failed to delete author with ID', {id,error});
        response.error =
            'An error occurred while deleting the author. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};