import React from "react";
import { BookView } from "../../../types";
import BookAdminCard from "./BookAdminCard";

interface BookAdminListProps {
    books: BookView[];
    onDelete: (e: React.MouseEvent, bookId: string) => void;
    onNavigate: (bookId: string) => void;
}

const BookAdminList: React.FC<BookAdminListProps> = ({ books, onDelete, onNavigate }) => {
    return (
        <ul>
            {books.map((book) => (
                <BookAdminCard
                    key={book.bookId}
                    book={book}
                    onDelete={(e) => onDelete(e, book.bookId)}
                    onNavigate={() => onNavigate(book.bookId)}
                />
            ))}
        </ul>
    );
};

export default BookAdminList;