import React from "react";
import BookCard from "../../components/book/BookCard";
import { useNavigate } from "react-router-dom";
import { BookView } from "../../types";
import { CartItem } from "../../types/types/cart/CartItem";

interface Props {
    books: BookView[];
}

const BooksListPage: React.FC<Props> = ({ books }) => {
    const navigate = useNavigate();
    const navigateToBook = (bookId: string) => {
        navigate(`/books/${bookId}`);
    };

    return (
        <ul>
            {books.map((book) => (
                <BookCard
                    key={book.bookId}
                    book={book}
                    onNavigate={() => navigateToBook(book.bookId)}
                    onAddItem={() => console.log("Item added")}
                />
            ))}
        </ul>
    );
};

export default BooksListPage;
