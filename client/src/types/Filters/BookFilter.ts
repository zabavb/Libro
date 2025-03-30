import { CoverType } from '../subTypes/book/CoverType';
import { Language } from '../subTypes/book/Language';

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
  discountRate: number;
  startDate: Date;
  endDate: Date;
}
