import React from "react"
import "@/assets/styles/components/order/order-book-list.css"
import { OrderedBook } from "@/types/types/order/OrderedBook";

interface OrderAdminFormBookListProps {
    books: OrderedBook[];
    onBookDelete: (bookId: string) => void;
    onBookAdd: (bookId: string) => void;
}

const OrderAdminFormBookList: React.FC<OrderAdminFormBookListProps> = ({ books, onBookDelete, onBookAdd }) => {
    return (
        <div>
            <table>
                <thead>
                    <tr>
                        <th style={{ width: "70%" }}>Title</th>
                        <th style={{ width: "5%" }}>X</th>
                        <th className="w-1/5">Price</th>
                        <th style={{ width: "5%" }}></th>
                    </tr>
                </thead>
                <tbody>
                    {books.map((book) => (
                        <tr key={book.bookId}>
                            <td style={{ cursor: "default" }}>{book.bookId}</td>
                            <td style={{ cursor: "default" }}>{book.quantity}</td>
                            <td style={{ cursor: "default" }}>TBD</td>
                            <td className="text-center">
                                <p className="list-buttons rounded-t-lg" onClick={() => onBookAdd(book.bookId)}>+</p>
                                <p className="list-buttons rounded-b-lg" onClick={() => onBookDelete(book.bookId)}>-</p>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default OrderAdminFormBookList