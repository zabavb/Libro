import { addSubCategory, deleteSubCategory, getAllSubCategories, getSubCategoryById, updateSubCategory } from "@/api/repositories/subcategoryRepository";
import { PaginatedResponse, ServiceResponse, SubCategory, SubCategoryFilter } from "../types";

export const fetchSubCategoriesService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    filter?: SubCategoryFilter
): Promise<ServiceResponse<PaginatedResponse<SubCategory>>> => {
    const response: ServiceResponse<PaginatedResponse<SubCategory>> = {
        data: null,
        loading: true,
        error: null
    }
    console.log(filter)
    try {



        const params = {
            ...filter,
        }


        response.data = await getAllSubCategories(pageNumber, pageSize, params);
    } catch (error) {
        if (error instanceof Error && 'response' in error && error.response?.status === 404) {
            response.data = {pageNumber:1, pageSize:10, items:[], totalCount: 1};
            console.error('No subcategories found.');
            response.error = null;
        } else {
            console.error(error instanceof Error ? error.message : String(error));
            response.error = 'An error occurred while fetching subcategories. Please try again later.';
        }
    } finally {
        response.loading = false;
    }

    return response;
}

export const fetchSubCategoryByIdService = async (id: string): Promise<ServiceResponse<SubCategory>> => {
    const response: ServiceResponse<SubCategory> = {
        data: null,
        loading: true,
        error: null,
    }

    if (id == "")
        return response

    try {
        response.data = await getSubCategoryById(id);
    } catch (error) {
        console.error(`Failed to fetch subcategory ID [${id}]`, error);
        response.error =
            'An error occurred while fetching subcategory. Please try again later.';
    } finally {
        response.loading = false;
    }
    return response;
}

export const addSubCategoryService = async (subCategory: Partial<SubCategory>): Promise<ServiceResponse<SubCategory>> => {
    const response: ServiceResponse<SubCategory> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await addSubCategory(subCategory);
    } catch (error) {
        console.error('Failed to create subcategory', error)
        response.error =
            'An error occurred while adding the subcategory. Please try again later.';
    } finally {
        response.loading = false;
    }
    return response;
}

export const editSubCategoryService = async (
    id: string,
    subCategory: Partial<SubCategory>): Promise<ServiceResponse<SubCategory>> => {
    const response: ServiceResponse<SubCategory> = {
        data: null,
        loading: true,
        error: null
    }


    try {
        response.data = await updateSubCategory(id, subCategory)
        console.log(response)
    } catch (error) {
        console.error(`Failed to update subcategory ID [${id}]`, error);
        response.error = 'An error occurred while updating the subcategory. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
}

export const removeSubCategoryService = async (id: string): Promise<ServiceResponse<string>> => {
    const response: ServiceResponse<string> = {
        data: null,
        loading: true,
        error: null
    };

    try {
        await deleteSubCategory(id);
        response.data = id;
    } catch (error) {
        console.error(`Failed to delete subcategory ID [${id}]`, error)
    } finally {
        response.loading = false;
    }
    return response;
}