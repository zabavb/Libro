import React from "react"
import { BookView } from "../../types"
import "react-lazy-load-image-component/src/effects/blur.css"
import "@/styles/bookCard.css"
import { CartItem } from "@/types/types/cart/CartItem"
interface BookCardProps {
    book: BookView
    onNavigate: () => void
    onAddItem: (item: CartItem) => void
}

const BookCard: React.FC<BookCardProps> = ({ book, onNavigate, onAddItem }) => {
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
                <button
                    onClick={() => onAddItem({ bookId: book.bookId, amount: 1, name: book.title, price:book.price })}>
                    Add to Cart
                    </button>
                </div>
            </div>

        </div>
    )
}

export default BookCard
