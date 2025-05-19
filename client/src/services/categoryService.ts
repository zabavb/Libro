import { addCategory, deleteCategory, getAllCategories, getCategoryById, updateCategory } from "@/api/repositories/categoryRepository";
import { Category, CategorySort, PaginatedResponse, ServiceResponse } from "../types";

export const fetchCategoriesService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    sort?: CategorySort
): Promise<ServiceResponse<PaginatedResponse<Category>>> => {
    const response: ServiceResponse<PaginatedResponse<Category>> = {
        data: null,
        loading: true,
        error: null
    }

    try{
    
    const formattedSort = Object.fromEntries(
        Object.entries(sort || {}).map(([key,value]) => [key, value ? 1 : 2])
    )

    const params = {
        searchTerm,
        ...formattedSort
    }


    response.data = await getAllCategories(pageNumber, pageSize, params);
    }catch(error){
        console.error('Failed to fetch categories', error);
        response.error =
            'An error occurred while fetching categories. Please try again later.';
    } finally{
        response.loading = false;
    }

    return response;
}

export const fetchCategoryByIdService = async (id: string) : Promise<ServiceResponse<Category>> => {
    const response: ServiceResponse<Category> = {
        data: null,
        loading: true,
        error: null,
    }

    if(id == "")
        return response

    try{
        response.data = await getCategoryById(id);
    } catch (error){
        console.error(`Failed to fetch category ID [${id}]`, error);
        response.error = 
            'An error occurred while fetching category. Please try again later.';
    } finally{
        response.loading = false;
    }
    return response;
}

export const addCategoryService = async (category: Partial<Category>): Promise<ServiceResponse<Category>> => {
    const response: ServiceResponse<Category> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await addCategory(category);
    } catch (error){
        console.error('Failed to create category', error)
        response.error = 
            'An error occurred while adding the category. Please try again later.';
    } finally {
        response.loading = false;
    }
    return response;
}

export const editCategoryService = async (
    id: string,
    category: Partial<Category>): Promise<ServiceResponse<Category>> => {
    const response: ServiceResponse<Category> = {
        data: null,
        loading: true,
        error: null
    }

    
    try {
        response.data = await updateCategory(id,category)
        console.log(response)
    } catch (error){
        console.error(`Failed to update category ID [${id}]`, error);
        response.error = 'An error occurred while updating the category. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
}

export const removeCategoryService = async (id: string): Promise<ServiceResponse<string>> => {
    const response: ServiceResponse<string> = {
        data: null,
        loading: true,
        error:null
    };

    try {
        await deleteCategory(id);
        response.data = id;
    } catch (error) {
        console.error(`Failed to delete category ID [${id}]`, error)
    } finally {
        response.loading = false;
    }
    return response;
}