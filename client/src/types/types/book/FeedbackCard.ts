export interface FeedbackCard
{
    feedbackId:string;
    comment?: string;
    rating: number;
    date: Date;
    userDisplayData: UserDisplayData;
}

export interface UserDisplayData{
    userName: string;
    userImageUrl?: string;
}