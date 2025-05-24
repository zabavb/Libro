import axios from "axios"
import { Author, PaginatedResponse } from "../../types"
import { AUTHOR_BY_ID, AUTHORS, AUTHORS_PAGINATED } from "..";

interface AuthorQueryParams {
    minBirthDate?: Date;
    maxBithDate?: Date;
    searchTerm?: string;
}

/**
 * Fetch a paginated list of authors with optional filters.
 */
export const getAllAuthors = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    params: AuthorQueryParams = {},
): Promise<PaginatedResponse<Author>> => {
    const url = AUTHORS_PAGINATED(pageNumber, pageSize)
    const response = await axios.get<PaginatedResponse<Author>>(url, { params })
    
    return response.data
}

/**
 * Fetch a single author by its ID.
 */
export const getAuthorById = async (id: string): Promise<Author> => {
    const response = await axios.get<Author>(AUTHOR_BY_ID(id));
    return response.data;
};

/**
 * Create a new author.
 */
export const addAuthor = async (author: Partial<Author>): Promise<Author> => {
    const response = await axios.post<Author>(AUTHORS, author);
    return response.data;
};

/**
 * Update an existing author by ID.
 */
export const updateAuthor = async (id: string, updatedAuthor: Partial<Author>): Promise<Author> => {
    const response = await axios.put<Author>(AUTHOR_BY_ID(id), updatedAuthor);
    return response.data;
};

/**
 * Delete a author by ID.
 */
export const deleteAuthor = async (id: string): Promise<void> => {
    await axios.delete(AUTHOR_BY_ID(id));
};

