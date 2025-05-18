
export interface Book {
  bookId: string;
  title: string;
  authorId: string;
  publisherId: string;
  categoryId: string;
  discountId?: string;
  price: number
  language: number
  year: Date
  description?: string
  cover: number
  quantity: number
  imageUrl: string;
  image: File | null;
  audioFileUrl?: string;
  PdfFileUrl?: string;
  feedbackIds?: string[];
  subcategoryIds?: string[]; 
}
