import React, { useState } from "react"
import { useNavigate } from "react-router-dom"
import BookCard from "../../components/book/BookCard"
import { useCart } from "@/state/context/CartContext"
import { CartItem } from "@/types/types/cart/CartItem"
import { AppDispatch } from "@/state/redux"
import { useDispatch } from "react-redux"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { BookCard as BookCardType } from "@/types/types/book/BookDetails"
import AddToCartToast from "../../components/common/AddToCartToast"

interface BookCardContainerProps {
	book: BookCardType
}

const BookCardContainer: React.FC<BookCardContainerProps> = ({ book }) => {
	const { addItem, cart } = useCart()
	const navigate = useNavigate()
	const dispatch = useDispatch<AppDispatch>()

	const [showToast, setShowToast] = useState(false)
	const [itemCount, setItemCount] = useState(0)
	const [totalPrice, setTotalPrice] = useState(0)

	const handleNavigate = () => {
		navigate(`/books/${book.bookId}`)
	}

	const handleAddItem = (item: CartItem) => {
		try {
			addItem(item)

			const count = cart.reduce((acc: number, i: CartItem) => acc + i.amount, 0) + item.amount
			const total = cart.reduce((acc: number, i: CartItem) => acc + i.amount * i.price, 0) + item.amount * item.price


			setItemCount(count)
			setTotalPrice(total)
			setShowToast(true)

			setTimeout(() => setShowToast(false), 5000)

			dispatch(
				addNotification({
					message: `Item has been successfully added to the cart`,
					type: "success"
				})
			)
		} catch (error) {
			dispatch(
				addNotification({
					message: error instanceof Error ? error.message : String(error),
					type: "error"
				})
			)
		}
	}

	return (
		<>
			<BookCard
				book={book}
				onNavigate={handleNavigate}
				onAddItem={handleAddItem}
			/>
			{showToast && (
				<AddToCartToast
					itemCount={itemCount}
					totalPrice={totalPrice}
					onClose={() => setShowToast(false)}
				/>
			)}
		</>
	)
}

export default BookCardContainer
