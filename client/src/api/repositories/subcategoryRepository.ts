import axios from "axios"
import { PaginatedResponse, SubCategory } from "../../types"
import { SUBCATEGORIES, SUBCATEGORIES_PAGINATED, SUBCATEGORY_BY_ID } from "..";

interface subCategoryQueryParams {
    categoryId?: string;
    searchTerm?: string;
}

/**
 * Fetch a paginated list of subcategories with optional filters.
 */
export const getAllSubCategories = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    params: subCategoryQueryParams = {},
): Promise<PaginatedResponse<SubCategory>> => {
    const url = SUBCATEGORIES_PAGINATED(pageNumber, pageSize)
    const response = await axios.get<PaginatedResponse<SubCategory>>(url, { params })
    return response.data
}

/**
 * Fetch a single subcategory by its ID.
 */
export const getSubCategoryById = async (id: string): Promise<SubCategory> => {
    const response = await axios.get<SubCategory>(SUBCATEGORY_BY_ID(id));
    return response.data;
};

/**
 * Create a new subcategory.
 */
export const addSubCategory = async (subCategory: Partial<SubCategory>): Promise<SubCategory> => {
    const response = await axios.post<SubCategory>(SUBCATEGORIES, subCategory);
    return response.data;
};

/**
 * Update an existing subcategory by ID.
 */
export const updateSubCategory = async (id: string, updatedSubCategory: Partial<SubCategory>): Promise<SubCategory> => {
    const response = await axios.put<SubCategory>(SUBCATEGORY_BY_ID(id), updatedSubCategory);
    return response.data;
};

/**
 * Delete a subcategory by ID.
 */
export const deleteSubCategory = async (id: string): Promise<void> => {
    await axios.delete(SUBCATEGORY_BY_ID(id));
};

