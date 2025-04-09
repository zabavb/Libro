import { BookView, Book } from '../../types';
import { Language } from '../../types/subTypes/book/Language';
import { CoverType } from '../../types/subTypes/book/CoverType';
import { dateToString } from './commonAdapters';

export const languageNumberToEnum = (languageNumber: number): Language => {
  const languageMap: { [key: number]: Language } = {
    0: Language.ENGLISH,
    1: Language.UKRAINIAN,
    2: Language.SPANISH,
    3: Language.FRENCH,
    4: Language.GERMAN,
    5: Language.OTHER,
  };

  return languageMap[languageNumber] ?? Language.OTHER;
};

export const languageEnumToNumber = (language: Language): number => {
  const languageMap: { [key in Language]: number } = {
    [Language.ENGLISH]: 0,
    [Language.UKRAINIAN]: 1,
    [Language.SPANISH]: 2,
    [Language.FRENCH]: 3,
    [Language.GERMAN]: 4,
    [Language.OTHER]: 5,
  };

  return languageMap[language] ?? 5;
};

export const coverNumberToEnum = (coverNumber: number): CoverType => {
  const coverMap: { [key: number]: CoverType } = {
    0: CoverType.SOFT_COVER,
    1: CoverType.HARD_COVER,
    2: CoverType.RING_BINDING,
    3: CoverType.LEATHER,
    4: CoverType.DUST_JACKET,
  };

  return coverMap[coverNumber] ?? CoverType.SOFT_COVER;
};

export const coverEnumToNumber = (cover: CoverType): number => {
  const coverMap: { [key in CoverType]: number } = {
    [CoverType.SOFT_COVER]: 0,
    [CoverType.HARD_COVER]: 1,
    [CoverType.RING_BINDING]: 2,
    [CoverType.LEATHER]: 3,
    [CoverType.DUST_JACKET]: 4,
  };

  return coverMap[cover] ?? 0;
};

export const BookToBookView = (response: Book): BookView => {
  return {
    bookId: response.bookId,
    title: response.title,
    authorId: response.authorId,
    publisherId: response.publisherId,
    price: response.price,
    language: languageNumberToEnum(response.language),
    year: dateToString(response.year),
    description: response.description,
    cover: coverNumberToEnum(response.cover),
    isAvailable: response.isAvailable,
    feedbackIds: response.feedbackIds,
    discountRate: response.discountRate,
    startDate: response.startDate,
    endDate: response.endDate
    //subcategoryIds: response.subcategoryIds,
  };
};
