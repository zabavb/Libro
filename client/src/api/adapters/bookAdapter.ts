import { BookView, Book, Author, Category, SubCategory } from '../../types';
import { Language } from '../../types/subTypes/book/Language';
import { CoverType } from '../../types/subTypes/book/CoverType';
import { dateToString } from './commonAdapters';
import { AuthorFormData, CategoryFormData, PublisherFormData, SubCategoryFormData } from '@/utils';
import { Publisher } from '@/types/types/book/Publisher';
import { BookFormData } from '@/utils/bookValidationSchema';
import { BookCard, BookDetails } from '@/types/types/book/BookDetails';

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
    0: CoverType.SOFTCOVER,
    1: CoverType.HARDCOVER,
    2: CoverType.RINGBINDING,
    3: CoverType.LEATHER,
    4: CoverType.DUSTJACKET,
  };

  return coverMap[coverNumber] ?? CoverType.SOFTCOVER;
};

export const coverEnumToNumber = (cover: CoverType): number => {
  const coverMap: { [key in CoverType]: number } = {
    [CoverType.SOFTCOVER]: 0,
    [CoverType.HARDCOVER]: 1,
    [CoverType.RINGBINDING]: 2,
    [CoverType.LEATHER]: 3,
    [CoverType.DUSTJACKET]: 4,
    [CoverType.OTHER]: 5,
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
      //isAvailable: response.isAvailable,
    quantity: response.quantity,
    feedbackIds: response.feedbackIds,
    discountRate: response.discountRate,
    startDate: response.startDate,
    endDate: response.endDate,
    //subcategoryIds: response.subcategoryIds,
    imageUrl: response.imageUrl,
  };
};


export const BookDetailsToBookCard = (response: BookDetails): BookCard => {
  return {
    bookId: response.bookId,
    title: response.title,
    price: response.price,
    imageUrl: response.imageUrl,
    authorId: response.authorId,
    authorName: response.authorName,
    isAvailable: response.quantity > 0,
  };
};

export const AuthorFormDataToAuthor = (form: AuthorFormData, id?: string) : Author => ({
  ...form,
  authorId: id ?? '00000000-0000-0000-0000-000000000000',
  name: form.name,
  biography: form.biography ?? undefined,
  dateOfBirth: form.dateOfBirth ? new Date(form.dateOfBirth) : undefined,
  citizenship: form.citizenship ?? undefined,
});

export const PublisherFormDataToPublisher = (form: PublisherFormData, id?: string) : Publisher => ({
  ...form,
  publisherId: id ?? '00000000-0000-0000-0000-000000000000',
  name: form.name,
  description: form.description ?? undefined,
});

export const CategoryFormDataToCategory = (form: CategoryFormData, id?: string) : Category => ({
  ...form,
  categoryId: id ?? '00000000-0000-0000-0000-000000000000',
  name: form.name,
});

export const SubCategoryFormDataToSubCategory = (form: SubCategoryFormData, id?: string): SubCategory => ({
  ...form,
  subCategoryId: id ?? '00000000-0000-0000-0000-000000000000',
  categoryId: form.categoryId,
  name: form.name,
})
