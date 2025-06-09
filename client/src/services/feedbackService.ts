import { addFeedback, getAllFeedbacks, getFeedbackById } from "@/api/repositories/feedbackRepository";
import { Bool, Feedback, FeedbackFilter, FeedbackSort, PaginatedResponse, ServiceResponse } from "@/types";
import { FeedbackAdminCard } from "@/types/types/book/FeedbackCard";

export const fetchFeedbacksService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    filters?: FeedbackFilter,
    sort?: FeedbackSort
): Promise<ServiceResponse<PaginatedResponse<FeedbackAdminCard>>> => {
    const response: ServiceResponse<PaginatedResponse<FeedbackAdminCard>> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        const defaultFilter: FeedbackFilter = {
            rating: null,
            minPublicationDate: null,
            maxPublicationDate: null,
            isPurchasedByReviewer: null,
            userId: null
        };

        const defaultSort = {
            newest: Bool.NULL,
            rating: Bool.NULL
        } as FeedbackSort;

        const body = {
            query: `
            query GetFeedbacksForAdmin(
                $pageNumber: Int!,
                $pageSize: Int!,
                $searchTerm: String,
                $filter: FeedbackFilterInput!,
                $sort: FeedbackSortInput!)
            {
                    getFeedbacksForAdmin(
                        pageNumber: $pageNumber,
                        pageSize: $pageSize,
                        searchTerm: $searchTerm,
                        filter: $filter,
                        sort: $sort
                    ) {
                        items {
                            feedbackId
                            comment
                            rating
                            date
                            userId
                            userName
                            title
                            bookImageUrl
                        }
                        pageNumber
                        pageSize
                        totalCount
                        totalPages
                    }
            }
            `,
            variables: {
                pageNumber,
                pageSize,
                searchTerm: searchTerm ?? null,
                filter: {
                    ...defaultFilter,
                    ...filters,
                },
                sort: { ...defaultSort, ...sort },
            },
        };


        const graphQLResponse = await getAllFeedbacks(body);
        if (graphQLResponse.errors)
            throw new Error(
                `${graphQLResponse.errors[0].message} Status code: ${graphQLResponse.errors[0].extensions?.status}`,
            );
        console.log(graphQLResponse.data);
        response.data = graphQLResponse.data
            ?.getFeedbacksForAdmin as PaginatedResponse<FeedbackAdminCard>;
    }
    catch (error) {
        console.error(error instanceof Error ? error.message : String(error));
        response.error =
            'An error occurred while fetching feedbacks. Please try again later.';
    } finally {
        response.loading = false;
    }
    console.log(response)
    return response;
}

/**
 * Fetch a feedback by its ID.
 */
export const fetchFeedbackByIdService = async (
    id: string
): Promise<ServiceResponse<Feedback>> => {
    const response: ServiceResponse<Feedback> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await getFeedbackById(id);
    } catch (error) {
        console.error('Failed to fetch feedback with ID', { id, error });
        response.error =
            'An error occurred while fetching the feedback. Please try again later.';
    } finally {
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
    } catch (error) {
        console.error('Failed to create feedback', error);
        response.error =
            'An error occurred while adding feedback. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
}