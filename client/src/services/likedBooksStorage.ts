import { BookCard } from "@/types/types/book/BookDetails";

const STORAGE_KEY = "likedBooks";

export function getLikedBooks(): BookCard[] {
    const data = localStorage.getItem(STORAGE_KEY);
    return data ? JSON.parse(data) : [];
}

export function likeBook(book: BookCard): void {
    const liked = getLikedBooks();
    if (!liked.find(b => b.bookId === book.bookId)) {
        liked.push(book);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(liked));
    }
}

export function unlikeBook(bookId: string): void {
    const liked = getLikedBooks().filter(book => book.bookId !== bookId);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(liked));
}

export function isBookLiked(bookId: string): boolean {
    return getLikedBooks().some(book => book.bookId === bookId);
}
