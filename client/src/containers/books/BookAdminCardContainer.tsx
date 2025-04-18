import React from "react"
import { useNavigate } from "react-router-dom"
import { Book } from "../../types/objects/Book"
import { BookToBookView } from "../../api/adapters/bookAdapter"
import BookCard from "../../components/book/BookCard"

interface BookCardContainerProps {
	book: Book
}

const BookAdminCardContainer: React.FC<BookCardContainerProps> = ({ book }) => {
    console.log("Book in BookCardContainer:", book);
	const bookView = BookToBookView(book)
	const navigate = useNavigate()

	const handleNavigate = () => {
		navigate(`/admin/books/${book.bookId}`)
	}

	return (
		<BookCard
			book={bookView}
			onNavigate={handleNavigate}
		/>
	)
}

export default BookAdminCardContainer
