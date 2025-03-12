import { CoverType } from "../subTypes/Book/CoverType";
import { Language } from "../subTypes/Book/Language";

export interface BookFilter {
    author?: string;
    publisher?: string;
    priceFrom?: number;
    priceTo?: number;
    yearFrom?: number;
    yearTo?: number;
    language?: Language;
    coverType?: CoverType;
    inStock?: boolean;
    subcategory?: string;
}
