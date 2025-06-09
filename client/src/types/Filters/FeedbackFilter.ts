export interface FeedbackFilter{
    rating?: number | null;
    minPublicationDate?: Date | null;
    maxPublicationDate?: Date | null;
    isPurchasedByReviewer?:boolean | null;
    userId?: string | null;
}