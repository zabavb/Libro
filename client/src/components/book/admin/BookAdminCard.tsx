import React from "react";
import { BookView } from "../../../types";

interface BookAdminCardProps {
    book: BookView;
    onDelete: (e: React.MouseEvent) => void;
    onNavigate: () => void;
}

const BookAdminCard: React.FC<BookAdminCardProps> = ({ book, onDelete, onNavigate }) => {
    return (
        <>
            <hr />
            <li
                onClick={(e) => {
                    e.stopPropagation();
                    onNavigate();
                }}>
                <div>
                    <p>
                        <strong>Title:</strong> {book.title}
                    </p>
                    <p>
                        <strong>Year:</strong> {book.year}
                    </p>
                    <p>
                        <strong>Cover:</strong> {book.cover}
                    </p>
                    <p>
                        <strong>Price:</strong> {book.price}
                    </p>
                    <p>
                        <strong>Discount:</strong> {book.discountRate} <strong> %</strong>
                    </p>
                    <p>
                        <strong>End of discount:</strong> {book.endDate.toString()}
                    </p>
                    <p>
                        <strong>Language:</strong> {book.language}
                    </p>
                    <p>
                        <strong>Available:</strong> {book.isAvailable ? "Yes" : "No"}
                    </p>
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            onDelete(e);
                        }}>
                        Delete
                    </button>
                </div>
            </li>
        </>
    );
};

export default BookAdminCard;
