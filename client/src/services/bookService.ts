import { PaginatedResponse, Book, BookSort, ServiceResponse } from "../types"
import {
    getAllBooks,
    getBookById,
    addBook,
    updateBook,
    deleteBook,
    getOwnedBooks
} from "../api/repositories/bookRepository"
import { coverEnumToNumber, languageEnumToNumber } from "@/api/adapters/bookAdapter";
import { BookFilter } from "@/types/filters/BookFilter";
import { BookCard, BookDetails, BookLibraryItem } from "@/types/types/book/BookDetails";


export const fetchBooksService = async (
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string,
    filters?: BookFilter,
    sort?: BookSort
): Promise<ServiceResponse<PaginatedResponse<BookCard>>> => {
    const response: ServiceResponse<PaginatedResponse<BookCard>> = {
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
            coverType:
            filters?.cover !== undefined
                ? coverEnumToNumber(filters.cover).toString()
                : undefined,
            language:
            filters?.language !== undefined
                ? languageEnumToNumber(filters.language).toString()
                : undefined,
            ...formattedSort,
        }
        response.data = await getAllBooks(pageNumber, pageSize, params)
    }
    catch (error) {
        console.error('Failed to fetch books', error)
        response.error =
            'An error occured while fetching books. Please try again later';
    } finally {
        response.loading = false;
    }

    return response;
};

export const fetchOwnedBooksService = async (
    userId: string,
    pageNumber: number = 1,
    pageSize: number = 10,
): Promise<ServiceResponse<PaginatedResponse<BookLibraryItem>>> => {
    const response: ServiceResponse<PaginatedResponse<BookLibraryItem>> = {
        data: null,
        loading: true,
        error: null,
    };

    try {

        const body = {
            query: `
            query GetUserOwnedBooks(
                $pageNumber: Int!,
                $pageSize: Int!,
                $userId: UUID!)
            {
                    getUserOwnedBooks(
                        pageNumber: $pageNumber,
                        pageSize: $pageSize,
                        userId: $userId
                    ) {
                        items {
                            title
                            authorName
                            imageUrl
                            pdfFileUrl
                            audioUrl
                        }
                        pageNumber
                        pageSize
                        totalCount
                        totalPages
                    }
            }
            `,
            variables: {
                userId,
                pageNumber,
                pageSize
            },
        };


        const graphQLResponse = await getOwnedBooks(body);
        if (graphQLResponse.errors)
            throw new Error(
                `${graphQLResponse.errors[0].message} Status code: ${graphQLResponse.errors[0].extensions?.status}`,
            );
        console.log(graphQLResponse.data);
        response.data = graphQLResponse.data
            ?.getUserOwnedBooks as PaginatedResponse<BookLibraryItem>;
    }
    catch (error) {
        console.error(error instanceof Error ? error.message : String(error));
        response.error =
            'An error occurred while fetching books. Please try again later.';
    } finally {
        response.loading = false;
    }
    console.log(response)
    return response;
}

/**
 * Fetch a book by its ID.
 */
export const fetchBookByIdService = async (
    bookId: string
): Promise<ServiceResponse<BookDetails>> => {
    const response: ServiceResponse<BookDetails> = {
        data: null,
        loading: true,
        error:null,
    };

    try{
        const body = {
            query: `
            query GetBookDetails(
            $bookId: UUID!
            ) {
                getBookDetails(bookId: $bookId){
                    bookId
                    title
                    price
                    language
                    year
                    description
                    cover
                    quantity
                    imageUrl
                    hasDigital
                    authorId
                    authorName
                    authorDescription
                    authorImageUrl
                    publisherName
                    categoryName
                    subcategories
                    latestFeedback{
                        feedbackId
                        comment
                        rating
                        date
                        userDisplayData{
                            userName
                            userImageUrl
                            }
                    }   
                    bookFeedbacks {
                        avgRating
                        feedbackAmount
                    }
                }
            }
            `,
            variables: {
                bookId,
            },
        };

        const graphQLResponse = await getBookById(body);
        if (graphQLResponse.errors)
            throw new Error(
                `${graphQLResponse.errors[0].message} Status code: ${graphQLResponse.errors[0].extensions?.status}`,
            );
        response.data = graphQLResponse.data?.getBookDetails  as BookDetails;
    } catch (error){
         console.error(`Failed to fetch book ID [${bookId}]`, error);
        response.error =
            'An error occurred while fetching book. Please try again later.';
    } finally {
        response.loading = false;
    }
    return response;
};

/**
 * Add a new book.
 */
export const addBookService = async (
    book: FormData
): Promise<ServiceResponse<FormData>> => {
    const response: ServiceResponse<FormData> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await addBook(book);
    }catch(error) {
        console.error('Failed to create book', error);
        response.error = 
            'An error occurred while adding book. Please try again later.';
    }finally {
        response.loading = false;
    }

    return response;
}

/**
 * Update an existing book.
 */
export const updateBookService = async (
    id: string,
    book: FormData
): Promise<ServiceResponse<FormData>> => {
    const response: ServiceResponse<FormData> = {
        data: null,
        loading: true,
        error: null,
    };

    try {
        response.data = await updateBook(id,book);
    } catch(error){
        console.error('Failed to update book with ID', {id, error});
        response.error =
            'An error occurred while updating the book. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};

/**
 * Delete a book by its ID.
 */
export const deleteBookService = async (
    id: string
): Promise<ServiceResponse<string>> => {
    const response: ServiceResponse<string> = {
        data: null,
        loading: true,
        error:null,
    }

    try {
        await deleteBook(id);
        response.data = id;
    } catch (error) {
        console.error('Failed to delete book with ID', {id,error});
        response.error =
            'An error occurred while deleting the book. Please try again later.';
    } finally {
        response.loading = false;
    }

    return response;
};

/**
 * Change the discount of book
 */
export const updateBookDiscountService = async (
    bookId: string,
    discountRate: number,
    endDate: Date
): Promise<ServiceResponse<Book>> => {
    const response: ServiceResponse<Book> = {
        data: null,
        loading: true,
        error: null,
    }

    try {
        const updatedFields = {
            discountRate,
            startDate: new Date(), // Set to current time
            endDate
        };

        response.data = await updateBook(bookId, updatedFields);
    } catch (error) {
        console.error(`Failed to update discount for book ID`, {bookId,error});
        response.error = 
            "An error occurred while updating the book discount. Please try again later.";
    } finally {
        response.loading = false;
    }
};

export const deleteBookDiscountService = async (
    bookId: string,
    discountRate: number,
    endDate: Date
): Promise<Book> => {
    try {
        const updatedFields = {
            discountRate: 0,
            startDate: new Date(), // Set to current time
            endDate: new Date()
        };

        return await updateBook(bookId, updatedFields);
    } catch (error) {
        console.error(`updateBookDiscountService: Failed to update discount for book ID ${bookId}`, error);
        throw new Error("An error occurred while updating the book discount. Please try again later.");
    }

    return response;
};