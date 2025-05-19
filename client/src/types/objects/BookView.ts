import { CoverType } from '../subTypes/book/CoverType';
import { Language } from '../subTypes/book/Language';

export interface BookView {
  bookId: string;
  title: string;
  authorId: string;
  publisherId: string;
  price: number;
  language: Language;
  year: string;
  description?: string;
  cover: CoverType;
  quantity: number;
    //isAvailable: boolean;
  feedbackIds: string[];
  discountRate: number;
  startDate: Date;
  endDate: Date;
  //subcategoryIds: string[]
  imageUrl?: string;
}
