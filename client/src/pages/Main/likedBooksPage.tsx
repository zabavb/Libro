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
        <>
            <h2>Liked Books </h2>
            
            {likedBooks.length === 0 ? (
                <p>No liked books yet.</p>
            ) : (
                <ul>
                {likedBooks.map((book) => (
                    <li key={book.bookId}>
                    <BookCard
                        book={book}
                        onNavigate={() => console.log("Navigate to", book.bookId)}
                        onAddItem={() => console.log("Item added")}
                    />
                    </li>
                ))}
                </ul>
            )}
        
    </>
  );
};

export default LikedBooksPage;
