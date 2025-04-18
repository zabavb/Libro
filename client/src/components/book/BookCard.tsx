import React, { useState, useEffect } from "react";
import { BookView } from "../../types"
import "react-lazy-load-image-component/src/effects/blur.css"
import { likeBook, unlikeBook, isBookLiked, } from "../../services/likedBooksStorage";
import "@/assets/styles/components/book-card.css"
import { CartItem } from "@/types/types/cart/CartItem"

interface BookAdminCardProps {
    book: BookView
    onNavigate: () => void
    onAddItem: (item: CartItem) => void
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
        <div className="cardContainer">
            <div>
                <img className="cardImage" src={`https://picsum.photos/seed/${book.bookId}/100/150`}/>
                <div>
                    <p onClick={(e) => {
                    e.stopPropagation()
                    onNavigate()
                }} style={{cursor:"pointer"}}>{book.title}</p>
                    {/* {book.authorName}  */}
                    <p>{book.price}</p>
                </div>
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
                <button
                    onClick={() => onAddItem({ bookId: book.bookId, amount: 1, name: book.title, price:book.price })}>
                    Add to Cart
                    </button>
                </div>
            </div>

        </div>
    )
}

export default BookCard;
