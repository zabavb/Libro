export interface FeedbackFilter{
    rating?: number;
    minPublicationDate?: Date;
    maxPublicationDate?: Date;
    isPurchasedByReviewer?:boolean;
}