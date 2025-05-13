export interface Feedback {
    feedbackId: string;
    userId: string;
    bookId: string;
    comment?: string;
    rating: number;
    date: Date;
    isPurchased: boolean;
}