import axios from "axios"
import { Category, PaginatedResponse } from "../../types"
import { CATEGORIES, CATEGORIES_PAGINATED, CATEGORY_BY_ID } from "..";

interface CategoryQueryParams {
    searchTerm?: string;
}

/**
 * Fetch a paginated list of categories with optional filters.
 */
export const getAllCategories = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    params: CategoryQueryParams = {},
): Promise<PaginatedResponse<Category>> => {
    const url = CATEGORIES_PAGINATED(pageNumber, pageSize)
    const response = await axios.get<PaginatedResponse<Category>>(url, { params })
    return response.data
}

/**
 * Fetch a single category by its ID.
 */
export const getCategoryById = async (id: string): Promise<Category> => {
    const response = await axios.get<Category>(CATEGORY_BY_ID(id));
    return response.data;
};

/**
 * Create a new category.
 */
export const addCategory = async (category: Partial<Category>): Promise<Category> => {
    const response = await axios.post<Category>(CATEGORIES, category);
    return response.data;
};

/**
 * Update an existing category by ID.
 */
export const updateCategory = async (id: string, updatedCategory: Partial<Category>): Promise<Category> => {
    const response = await axios.put<Category>(CATEGORY_BY_ID(id), updatedCategory);
    return response.data;
};

/**
 * Delete a category by ID.
 */
export const deleteCategory = async (id: string): Promise<void> => {
    await axios.delete(CATEGORY_BY_ID(id));
};

