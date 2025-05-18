import axios from "axios"
import { Feedback, PaginatedResponse } from "../../types"
import { FEEDBACK_BY_ID, FEEDBACKS, FEEDBACKS_PAGINATED } from "..";

interface FeedbackQueryParams {
    rating?: number;
    minPublicationDate?: Date;
    maxPublicationDate?: Date;
    isPurchasedByReviewer?:boolean
    searchTerm?: string;
}

/**
 * Fetch a paginated list of feedbacks with optional filters.
 */
export const getAllFeedbacks = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    params: FeedbackQueryParams = {},
): Promise<PaginatedResponse<Feedback>> => {
    const url = FEEDBACKS_PAGINATED(pageNumber, pageSize)
    const response = await axios.get<PaginatedResponse<Feedback>>(url, { params })
    
    console.log(response)
    return response.data
}

/**
 * Fetch a single feedback by its ID.
 */
export const getFeedbackById = async (id: string): Promise<Feedback> => {
    const response = await axios.get<Feedback>(FEEDBACK_BY_ID(id));
    return response.data;
};

/**
 * Create a new feedback.
 */
export const addFeedback = async (feedback: Partial<Feedback>): Promise<Feedback> => {
    const response = await axios.post<Feedback>(FEEDBACKS, feedback);
    return response.data;
};

