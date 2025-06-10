import { CoverType } from "@/types/subTypes/book/CoverType";
import { Language } from "@/types/subTypes/book/Language";
import { FeedbackCard } from "./FeedbackCard";

export interface BookDetails{
    bookId: string;
    title:string;
    price:number;
    language:Language;
    year: Date
    description?: string;
    cover: CoverType;
    quantity: number;
    imageUrl?: string;
    hasDigital: boolean;
    authorId: string;
    authorName: string;
    authorDescription: string;
    authorImageUrl?: string;
    publisherName: string;
    categoryName:string;
    subcategories: string[];
    latestFeedback: FeedbackCard[];
    bookFeedbacks: BookFeedbacks;
}

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

export interface BookLibraryItem{
    title: string;
    authorName: string;
    imageUrl?: string;
    pdfFileUrl?: string;
    audioUrl?: string;
}