import React from "react";
import cartIcon from '@/assets/icons/cartBig.svg'
import heart from '@/assets/icons/heart.svg'
import "@/assets/styles/components/book/book-of-the-week.css";


type BookOfTheWeekProps = {
  title: string;
  author: string;
  price: number;
  coverImage: string;
  onOrder: () => void;
  onFavorite: () => void;
};

const BookOfTheWeek: React.FC<BookOfTheWeekProps> = ({
  title,
  author,
  price,
  coverImage,
  onOrder,
  onFavorite,
}) => {
  return (
    <div className="book-of-the-week">
      <div className="book-container book-image-container">
        <img src={coverImage} alt={title} />
      </div>
      <div className="book-container book-info-container">
        <p className="book-author">{author}</p>
        <h3 className="book-title">{title}</h3>
        <p className="book-price">{price} грн</p>
        <div className="actions">
          <button className="order-button" onClick={onOrder}>
          <img src={cartIcon} alt="cart" className="cart-icon" />
            Order
          </button>
          <button className="favorite-button" onClick={onFavorite}>
            <img src={heart} alt="heart" className="heart-icon" />
          </button>
        </div>
      </div>
    </div>
  );
};

export default BookOfTheWeek;