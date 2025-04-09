import React from "react";
import BookCard from "../../components/book/BookCard";
import { BookView } from "../../types";

interface Props {
    books: BookView[];
}

const BooksListPage: React.FC<Props> = ({ books }) => {
    const navigateToBook = (bookId: string) => {
        //
        // Implement navigation logic here
        //
    };

    return (
        <ul>
            {books.map((book) => (
                <BookCard
                    key={book.bookId}
                    book={book}
                    onNavigate={() => navigateToBook(book.bookId)}
                />
            ))}
        </ul>
    );
};

export default BooksListPage;
