import { CoverType } from '../subTypes/book/CoverType';
import { Language } from '../subTypes/book/Language';

export interface BookFilter {
  authorId?: string;
  publisherid?: string;
  minPrice?: number;
  maxPrice?: number;
  minYear?: number;
  maxYear?: number;
  language?: Language;
  cover?: CoverType;
  available?: boolean;
  categoryId?: string;
  subcategoryId?: string;
  hasAudio?: boolean;
  hasDigital?: boolean;
}
