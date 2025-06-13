import React, { useEffect, useState } from "react";
import cartIcon from "@/assets/icons/cartBig.svg";
import { icons } from '@/lib/icons';
import "@/assets/styles/components/book/book-of-the-week.css";
import { CartItem } from "@/types/types/cart/CartItem";
import { isBookLiked, likeBook, unlikeBook } from "@/services/likedBooksStorage";
import { BookCard } from "@/types/types/book/BookDetails";
import { useDispatch } from "react-redux";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { AppDispatch } from "@/state/redux";

interface BookOfTheWeekProps {
	bookId: string;
	title: string;
	price: number;
	coverImage: string;
	onOrder: (item: CartItem) => void;
}

const BookOfTheWeek: React.FC<BookOfTheWeekProps> = ({
	bookId,
	title,
	price,
	coverImage,
	onOrder,
}) => {
	const [liked, setLiked] = useState(false);
	const dispatch = useDispatch<AppDispatch>();

	useEffect(() => {
		setLiked(isBookLiked(bookId));
	}, [bookId]);

	const handleOrderClick = () => {
		onOrder({
			bookId,
			name: title,
			price,
			amount: 1,
		});
	};

  const toggleLike = () => {
    if (liked) {
      unlikeBook(bookId);
      dispatch(
        addNotification({
          message: `"${title}" removed feom liked!`,
          type: "info",
        })
      );
    } else {
      likeBook({ bookId, title, price, imageUrl: coverImage } as BookCard);
      dispatch(
        addNotification({
          message: `"${title}" added to liked!`,
          type: "success",
        })
      );
    }
    setLiked(!liked);
  };


	return (
		<div className="book-of-the-week">
			<div className="book-container book-image-container">
				<img src={coverImage} alt={title} />
			</div>
			<div className="book-container book-info-container">
				<h3 className="book-title">{title}</h3>
				<p className="book-price">{price.toFixed(2)} грн</p>
				<div className="actions flex items-center gap-4">
					<button className="order-button flex items-center gap-2" onClick={handleOrderClick}>
						<img src={cartIcon} alt="cart" className="cart-icon" />
						Order
					</button>

					<div>
            <button
              onClick={toggleLike}
              className={`like-btn transition-all duration-300 ease-in-out border-2 rounded-full p-5 ${
                liked ? 'bg-[#FF642E] border-[#FF642E]' : 'border-white'
              }`}
            >
              <img
                className="w-10 h-10 transition-all duration-300 ease-in-out invert"
                src={icons.bHeart}
                alt="favorite"
              />
            </button>
          </div>



				</div>
			</div>
		</div>
	);
};

export default BookOfTheWeek;
