import axios from "axios"
import { BOOKS_PAGINATED, BOOKS, BOOK_BY_ID } from "../index"
import { PaginatedResponse } from "../../types"
import { Book } from "../../types/objects/Book"

interface BookQueryParams {
    author?: string;
    publisher?: string;
    priceFrom?: number;
    priceTo?: number;
    yearFrom?: number;
    yearTo?: number;
    language?: string;
    covertype?: string;
    inStock?: boolean;
    subcategory?:string;
    discountRate?:number;
    startDate?: Date;
    endDate?: Date;
    searchTerm?: string;
}

/**
 * Fetch a paginated list of books with optional filters.
 */
export const getAllBooks = async (
	pageNumber: number = 1,
	pageSize: number = 10,
	params: BookQueryParams = {},
): Promise<PaginatedResponse<Book>> => {
	const url = BOOKS_PAGINATED(pageNumber, pageSize)
	const response = await axios.get<PaginatedResponse<Book>>(url, { params })
	return response.data
}

/**
 * Fetch a single book by its ID.
 */
export const getBookById = async (id: string): Promise<Book> => {
    const response = await axios.get<Book>(BOOK_BY_ID(id));
    return response.data;
};

/**
 * Create a new book.
 */
export const addBook = async (book: Partial<Book>): Promise<Book> => {
    const response = await axios.post<Book>(BOOKS, book);
    return response.data;
};

/**
 * Update an existing book by ID.
 */
export const updateBook = async (id: string, updatedBook: Partial<Book>): Promise<Book> => {
    const response = await axios.put<Book>(BOOK_BY_ID(id), updatedBook);
    return response.data;
};

/**
 * Delete a book by ID.
 */
export const deleteBook = async (id: string): Promise<void> => {
    await axios.delete(BOOK_BY_ID(id));
};

