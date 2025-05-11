import { addFeedback, deleteFeedback, getAllFeedbacks, getFeedbackById, updateFeedback } from "@/api/repositories/feedbackRepository";
import { Feedback, FeedbackFilter, FeedbackSort, PaginatedResponse, ServiceResponse } from "@/types";

export const fetchFeedbacksService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    filters?: FeedbackFilter,
    sort?: FeedbackSort
): Promise<ServiceResponse<PaginatedResponse<Feedback>>> => {
    const response: ServiceResponse<PaginatedResponse<Feedback>> = {
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
            ...formattedSort,
        }
        response.data = await getAllFeedbacks(pageNumber, pageSize, params)
    }
    catch (error) {
        console.error('Failed to fetch feedbacks', error)
        response.error =
            'An error occured while fetching feedbacks. Please try again later';
    } finally {
        response.loading = false;
    }
    return response;
};

/**
 * Fetch a feedback by its ID.
 */
export const fetchFeedbackByIdService = async (
    id: string
): Promise<ServiceResponse<Feedback>> => {
    const response: ServiceResponse<Feedback> = {
        data: null,
        loading: true,
        error:null,
    };

    try{
        response.data = await getFeedbackById(id);
    } catch (error){
        console.error('Failed to fetch feedback with ID', {id, error});
        response.error = 
            'An error occurred while fetching the feedback. Please try again later.';
    } finally{
        response.loading = false;
    }
    return response;
};

/**
 * Add a new feedback.
 */
export const addFeedbackService = async (
    feedback: Partial<Feedback>
): Promise<ServiceResponse<Feedback>> => {
    const response: ServiceResponse<Feedback> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await addFeedback(feedback);
    }catch(error) {
        console.error('Failed to create feedback', error);
        response.error = 
            'An error occurred while adding feedback. Please try again later.';
    }finally {
        response.loading = false;
    }

    return response;
}

/**
 * Update an existing feedback.
 */
export const editFeedbackService = async (
    id: string,
    feedback: Partial<Feedback>
): Promise<ServiceResponse<Feedback>> => {
    const response: ServiceResponse<Feedback> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await updateFeedback(id,feedback);
    } catch(error){
        console.error('Failed to update feedback with ID', {id, error});
        response.error =
            'An error occurred while updating the feedback. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};

/**
 * Delete a feedback by its ID.
 */
export const removeFeedbackService = async (
    id: string
): Promise<ServiceResponse<string>> => {
    const response: ServiceResponse<string> = {
        data: null,
        loading: true,
        error:null,
    }

    try {
        await deleteFeedback(id);
        response.data = id;
    } catch (error) {
        console.error('Failed to delete feedback with ID', {id,error});
        response.error =
            'An error occurred while deleting the feedback. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};