import { PaginatedResponse, Book, BookSort, ServiceResponse } from "../types"
import {
    getAllBooks,
    getBookById,
    addBook,
    updateBook,
    deleteBook
} from "../api/repositories/bookRepository"
import { coverEnumToNumber, languageEnumToNumber } from "@/api/adapters/bookAdapter";
import { BookFilter } from "@/types/filters/BookFilter";


export const fetchBooksService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    filters?: BookFilter,
    sort?: BookSort
): Promise<ServiceResponse<PaginatedResponse<Book>>> => {
    const response: ServiceResponse<PaginatedResponse<Book>> = {
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
            coverType:
            filters?.coverType !== undefined
                ? coverEnumToNumber(filters.coverType).toString()
                : undefined,
            language:
            filters?.language !== undefined
                ? languageEnumToNumber(filters.language).toString()
                : undefined,
            ...formattedSort,
        }
        response.data = await getAllBooks(pageNumber, pageSize, params)
    }
    catch (error) {
        console.error('Failed to fetch books', error)
        response.error =
            'An error occured while fetching books. Please try again later';
    } finally {
        response.loading = false;
    }

    return response;
};

/**
 * Fetch a book by its ID.
 */
export const fetchBookByIdService = async (
    id: string
): Promise<ServiceResponse<Book>> => {
    const response: ServiceResponse<Book> = {
        data: null,
        loading: true,
        error:null,
    };

    try{
        response.data = await getBookById(id);
    } catch (error){
        console.error('Failed to fetch book with ID', {id, error});
        response.error = 
            'An error occurred while fetching the book. Please try again later.';
    } finally{
        response.loading = false;
    }
    return response;
};

/**
 * Add a new book.
 */
export const addBookService = async (
    book: Partial<Book>
): Promise<ServiceResponse<Book>> => {
    const response: ServiceResponse<Book> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await addBook(book);
    }catch(error) {
        console.error('Failed to create book', error);
        response.error = 
            'An error occurred while adding book. Please try again later.';
    }finally {
        response.loading = false;
    }

    return response;
}

/**
 * Update an existing book.
 */
export const updateBookService = async (
    id: string,
    book: Partial<Book>
): Promise<ServiceResponse<Book>> => {
    const response: ServiceResponse<Book> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await updateBook(id,book);
    } catch(error){
        console.error('Failed to update book with ID', {id, error});
        response.error =
            'An error occurred while updating the book. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};

/**
 * Delete a book by its ID.
 */
export const deleteBookService = async (
    id: string
): Promise<ServiceResponse<string>> => {
    const response: ServiceResponse<string> = {
        data: null,
        loading: true,
        error:null,
    }

    try {
        await deleteBook(id);
        response.data = id;
    } catch (error) {
        console.error('Failed to delete book with ID', {id,error});
        response.error =
            'An error occurred while deleting the book. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};

/**
 * Change the discount of book
 */
export const updateBookDiscountService = async (
    bookId: string,
    discountRate: number,
    endDate: Date
): Promise<ServiceResponse<Book>> => {
    const response: ServiceResponse<Book> = {
        data: null,
        loading: true,
        error: null,
    }

    try {
        const updatedFields = {
            discountRate,
            startDate: new Date(), // Set to current time
            endDate
        };

        response.data = await updateBook(bookId, updatedFields);
    } catch (error) {
        console.error(`Failed to update discount for book ID`, {bookId,error});
        response.error = 
            "An error occurred while updating the book discount. Please try again later.";
    } finally {
        response.loading = false;
    }

    return response;
};