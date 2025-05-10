import React from "react"
import { useNavigate } from "react-router-dom"
import { Book } from "../../types/objects/Book"
import { BookToBookView } from "../../api/adapters/bookAdapter"
import BookAdminCard from "@/components/book/admin/BookAdminCard"

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

	const handleDelete = () => {
		console.log("DELETE")
	}

	return (
		<BookAdminCard
			book={bookView}
			onNavigate={handleNavigate}
			onDelete={handleDelete}
		/>
	)
}

export default BookAdminCardContainer
