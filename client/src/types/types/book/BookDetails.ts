export interface BookCard{
    bookId: string;
    title: string;
    price: number;
    isAvailable: boolean;
    imageUrl?: string;
    categoryName?: string;
    authorId: string;
    authorName: string;
    rating?: BookFeedbacks;
}

export interface BookFeedbacks{
    avgRating?: number;
    feedbackAmount: number;
}