import { PaginatedResponse, Book } from "../types"
import {
    getAllBooks,
    getBookById,
    addBook,
    updateBook,
    deleteBook
} from "../api/repositories/bookRepository"


export const fetchBooksService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    
): Promise<PaginatedResponse<Book>> => {
    try {
        const params = {
        }
        const response = await getAllBooks(pageNumber, pageSize, params);
        return response;
    } catch (error) {
        console.error(`fetchBooksService: Failed to fetch books`, error)
        throw new Error("An error occurred while fetching books. Please try again later.")
    }
}
/**
 * Fetch a book by its ID.
 */
export const fetchBookByIdService = async (bookId: string): Promise<Book> => {
    try {
        return await getBookById(bookId);
    } catch (error) {
        console.error(`fetchBookByIdService: Failed to fetch book with ID ${bookId}`, error);
        throw new Error("An error occurred while fetching the book. Please try again later.");
    }
};

/**
 * Add a new book.
 */
export const addBookService = async (newBook: Omit<Book, "bookId">): Promise<Book> => {
    try {
        return await addBook(newBook);
    } catch (error) {
        console.error("addBookService: Failed to add a book", error);
        throw new Error("An error occurred while adding the book. Please try again later.");
    }
};

/**
 * Update an existing book.
 */
export const updateBookService = async (bookId: string, updatedBook: Partial<Book>): Promise<Book> => {
    try {
        return await updateBook(bookId, updatedBook);
    } catch (error) {
        console.error(`updateBookService: Failed to update book with ID ${bookId}`, error);
        throw new Error("An error occurred while updating the book. Please try again later.");
    }
};

/**
 * Delete a book by its ID.
 */
export const deleteBookService = async (bookId: string): Promise<void> => {
    try {
        await deleteBook(bookId);
    } catch (error) {
        console.error(`deleteBookService: Failed to delete book with ID ${bookId}`, error);
        throw new Error("An error occurred while deleting the book. Please try again later.");
    }
};

/**
 * Change the discount of book
 */
export const updateBookDiscountService = async (
    bookId: string,
    discountRate: number,
    endDate: Date
): Promise<Book> => {
    try {
        const updatedFields = {
            discountRate,
            startDate: new Date(), // Set to current time
            endDate
        };

        return await updateBook(bookId, updatedFields);
    } catch (error) {
        console.error(`updateBookDiscountService: Failed to update discount for book ID ${bookId}`, error);
        throw new Error("An error occurred while updating the book discount. Please try again later.");
    }
};