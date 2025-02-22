import React from "react"
import { BookView } from "../../types"
import "react-lazy-load-image-component/src/effects/blur.css"

interface BookAdminCardProps {
    book: BookView
    onNavigate: () => void
}

const BookCard: React.FC<BookAdminCardProps> = ({ book, onNavigate }) => {
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
                   
                </div>
               
            </li>
        </>
    )
}

export default BookCard
