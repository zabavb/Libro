import axios from "axios"
import { BOOKS_PAGINATED } from "../index"
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

