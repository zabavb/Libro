import {  PaginatedResponse, Book } from "../types"
import { getAllBooks } from "../api/repositories/bookRepository"

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

