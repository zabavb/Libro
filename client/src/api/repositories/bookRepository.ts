import axios from "axios"
import { BOOKS_PAGINATED, BOOKS, BOOK_BY_ID } from "../index"
import { PaginatedResponse } from "../../types"
import { Book } from "../../types/objects/Book"

interface BookQueryParams {
	
}

/**
 * Fetch a paginated list of Books with optional filters.
 */
export const getAllBooks = async (
	pageNumber: number = 1,
	pageSize: number = 10,
	params: BookQueryParams = {}
): Promise<PaginatedResponse<Book>> => {
	const url = BOOKS_PAGINATED(pageNumber, pageSize)
	const response = await axios.get<PaginatedResponse<Book>>(url, { params })
	return response.data
}

/**
 * Fetch a book by ID.
 */
export const getBookById = async (id: string): Promise<Book> => {
    const url = BOOK_BY_ID(id);
    const response = await axios.get<Book>(url);
    return response.data;
};


/**
 * Update a Book by its ID.
 */
export const updateBook = async (id: string, updatedBook: Partial<Book>): Promise<Book> => {
    const response = await axios.put<Book>(BOOK_BY_ID(id), updatedBook);
    return response.data;
};

/**
 * Delete a Book by its ID.
 */
export const deleteBook = async (id: string): Promise<void> => {
    await axios.delete(BOOK_BY_ID(id));
};

/**
 * Add a new Book.
 */
export const addBook = async (newBook: Omit<Book, "bookId">): Promise<Book> => {
    const response = await axios.post<Book>(BOOKS, newBook);
    return response.data;
};