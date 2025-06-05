export interface FeedbackCard
{
    feedbackId:string;
    comment?: string;
    rating: number;
    date: Date;
    userDisplayData: UserDisplayData;
}

export interface FeedbackAdminCard{
    feedbackId: string;
    comment?: string;
    rating: number;
    date: Date;
    title: string;
    bookImageUrl?: string;
    userName?: string;
}

export interface UserDisplayData{
    userName: string;
    userImageUrl?: string;
}