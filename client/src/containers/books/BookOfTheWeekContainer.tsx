import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useCart } from "@/state/context/CartContext";
import { useDispatch } from "react-redux";
import { AppDispatch } from "@/state/redux";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { BookCard as BookCardType } from "@/types/types/book/BookDetails";
import { CartItem } from "@/types/types/cart/CartItem";
import AddToCartToast from "@/components/common/AddToCartToast";
import BookOfTheWeek from "@/components/book/BookOfTheWeek";
import { fetchBooksService } from "@/services/bookService";

const BookOfTheWeekContainer: React.FC = () => {
	const { addItem, cart } = useCart();
	const navigate = useNavigate();
	const dispatch = useDispatch<AppDispatch>();

	const [book, setBook] = useState<BookCardType | null>(null);
	const [showToast, setShowToast] = useState(false);
	const [itemCount, setItemCount] = useState(0);
	const [totalPrice, setTotalPrice] = useState(0);

	useEffect(() => {
		const fetchBook = async () => {
			try {
				const response = await fetchBooksService(1, 1, "", {}, {});
				if (response.error) {
					dispatch(addNotification({ message: response.error, type: "error" }));
				} else if (response.data?.items?.length) {
					setBook(response.data.items[0]);
				}
			} catch (error) {
				dispatch(
					addNotification({
						message: error instanceof Error ? error.message : String(error),
						type: "error"
					})
				);
			}
		};
		fetchBook();
	}, [dispatch]);

	const handleAddItem = (item: CartItem) => {
		try {
			addItem(item);
			const count = cart.reduce((acc, i) => acc + i.amount, 0) + item.amount;
			const total = cart.reduce((acc, i) => acc + i.amount * i.price, 0) + item.amount * item.price;

			setItemCount(count);
			setTotalPrice(total);
			setShowToast(true);

			setTimeout(() => setShowToast(false), 5000);
		} catch (error) {
			dispatch(
				addNotification({
					message: error instanceof Error ? error.message : String(error),
					type: "error"
				})
			);
		}
	};

	if (!book) return null;

	return (
		<>
			<BookOfTheWeek
                bookId={book.bookId}
                title={book.title}
                price={book.price}
                coverImage={book.imageUrl ?? ""}
                onOrder={() =>
                    handleAddItem({
                        bookId: book.bookId,
                        name: book.title,
                        price: book.price,
                        amount: 1,
                    })
                }
            />

			{showToast && (
				<AddToCartToast
					itemCount={itemCount}
					totalPrice={totalPrice}
					onClose={() => setShowToast(false)}
				/>
			)}
		</>
	);
};


export default BookOfTheWeekContainer;
