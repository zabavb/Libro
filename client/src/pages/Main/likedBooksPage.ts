import React, { useEffect, useState } from "react";
import { BookView } from "../../types";
import { getLikedBooks } from "../../services/likedBooksStorage";
import BookCard from "../../components/book/BookCard";

const LikedBooksPage: React.FC = () => {
    const [likedBooks, setLikedBooks] = useState<BookView[]>([]);

    useEffect(() => {
        setLikedBooks(getLikedBooks());
    }, []);

    return (
        <div>
            <h2>Liked Books < /h2>
            <ul>
            {likedBooks.length === 0 ? (
                <p>No liked books yet.< /p>
            ) : (
                likedBooks.map((book) => (
                    <BookCard
                          key= { book.bookId }
                          book = { book }
                          onNavigate = {() => console.log("Navigate to", book.bookId)}
                    />
                ))
            )}
        </ul>
    < /div>
  );
};

export default LikedBooksPage;
