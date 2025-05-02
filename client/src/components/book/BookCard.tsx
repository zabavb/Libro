import React, { useState, useEffect } from "react";
import { BookView } from "../../types"
import "react-lazy-load-image-component/src/effects/blur.css"
import { likeBook, unlikeBook, isBookLiked, } from "../../services/likedBooksStorage";
import { CartItem } from "@/types/types/cart/CartItem"
import "@/assets/styles/components/book/card.css"
import cartIcon from "@/assets/icons/cartIcon.svg"
interface BookAdminCardProps {
    book: BookView
    onNavigate: () => void
    onAddItem: (item: CartItem) => void
}

const BookCard: React.FC<BookAdminCardProps> = ({ onAddItem, book, onNavigate }) => {
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
        <div className="card-container">
            <img className="card-image cursor-pointer" 
            onClick={(e) => {
                        e.stopPropagation()
                        onNavigate()
                    }}
            src={`https://picsum.photos/seed/${book.bookId}/220/340`} />
            <div>
                <div 
                    className="card-info">
                    <p onClick={(e) => {
                        e.stopPropagation()
                        onNavigate()
                    }}
                    className="card-title">
                        {book.title}
                    </p>
                    <p className="card-subtext">
                        AUTHOR_TMP
                    </p>
                </div>
            </div>
            <div className="card-footer">
                <div>
                <p className="book-price">{book.price} UAH</p>
                {book.quantity > 0 ? (<p className="book-availability">Available</p>) : (<p className="book-availability">Not Available</p>)}
                </div>
                <button
                    onClick={() => onAddItem({ bookId: book.bookId, amount: 1, name: book.title, price: book.price })}
                    className="book-cart-btn">
                    <img src={cartIcon}></img>
                </button>
            </div>
        </div>
    )
}

export default BookCard;
