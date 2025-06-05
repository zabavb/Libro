import React from "react"
import { useNavigate } from "react-router-dom"
import BookCard from "../../components/book/BookCard"
import { useCart } from "@/state/context/CartContext"
import { CartItem } from "@/types/types/cart/CartItem"
import { AppDispatch } from "@/state/redux"
import { useDispatch } from "react-redux"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { BookCard as BookCardType } from "@/types/types/book/BookDetails"

interface BookCardContainerProps {
	book: BookCardType
}

const BookCardContainer: React.FC<BookCardContainerProps> = ({ book  }) => {
	const { addItem } = useCart();
	const navigate = useNavigate()
	const dispatch = useDispatch<AppDispatch>()

	const handleNavigate = () => {
		navigate(`/books/${book.bookId}`)
	}

	const handleAddItem = (item: CartItem) => {
			try{
				addItem(item);
				dispatch(
					addNotification({
						message: `Item has been successfully added to the cart`,
						type:"success"
					})
				)
			}catch(error){
				dispatch(
					addNotification({
						message: error instanceof Error ? error.message : String(error),
						type:"error"
					})
				)
			}
	}

	return (
		<BookCard
			book={book}
			onNavigate={handleNavigate}
			onAddItem={handleAddItem}
		/>
	)
}

export default BookCardContainer
