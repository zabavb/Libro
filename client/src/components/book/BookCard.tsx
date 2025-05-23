import React from "react";
import { BookView } from "../../types"
import "react-lazy-load-image-component/src/effects/blur.css"
import { CartItem } from "@/types/types/cart/CartItem"
import "@/assets/styles/components/book/card.css"
import { icons } from '@/lib/icons';
import noImageUrl from '@/assets/noImage.svg'
interface BookAdminCardProps {
    book: BookView
    onNavigate: () => void
    onAddItem: (item: CartItem) => void
}

const BookCard: React.FC<BookAdminCardProps> = ({ onAddItem, book, onNavigate }) => {


    return (
        <div className="card-container">
            <img className="card-image cursor-pointer" 
            onClick={(e) => {
                        e.stopPropagation()
                        onNavigate()
                    }}
            src={book.imageUrl ? book.imageUrl : noImageUrl} />
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
                <p className="book-price">{book.price.toFixed(2)} UAH</p>
                {book.quantity > 0 ? (<p className="book-availability">Available</p>) : (<p className="book-availability">Not Available</p>)}
                </div>
                <button
                    onClick={() => onAddItem({ bookId: book.bookId, amount: 1, name: book.title, price: book.price })}
                    className="book-cart-btn min-w-10 min-h-10">
                    <img src={icons.bCart}></img>
                </button>
            </div>
        </div>
    )
}

export default BookCard;
