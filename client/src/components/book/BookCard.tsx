import React, { useState, useEffect } from "react";
import { BookView } from "../../types"
import "react-lazy-load-image-component/src/effects/blur.css"
import { likeBook, unlikeBook, isBookLiked, } from "../../services/likedBooksStorage";

interface BookAdminCardProps {
    book: BookView
    onNavigate: () => void
}

const BookCard: React.FC<BookAdminCardProps> = ({ book, onNavigate }) => {
    const [liked, setLiked] = useState(false);

    useEffect(() => {
        setLiked(isBookLiked(book.bookId));
    }, [book.bookId]);

    const toggleLike = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (liked) {
            unlikeBook(book.bookId);
        } else {
            likeBook(book);
        }
        setLiked(!liked);
    };
    return (
        <>
            <hr />
            <li
                onClick={(e) => {
                    e.stopPropagation()
                    onNavigate()
                }}>
                <div>
                    <p>
                        <strong>title</strong> {book.title}
                    </p>
                    <p>
                        <strong>year:</strong> {book.year}
                    </p>
                    <p>
                        <strong>cover:</strong> {book.cover}
                    </p>
                    <button onClick={toggleLike}>
                        {liked ? "Unlike" : "like"}
                    </button>
                </div>
               
            </li>
        </>
    )
}

export default BookCard;
