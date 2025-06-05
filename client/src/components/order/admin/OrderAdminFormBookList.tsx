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
                {Object.entries(books)?.map(([book,count]) => (
                    <tr key={book}>
                        <td style={{cursor:"default"}}>{book}</td>
                        <td style={{cursor:"default"}}>{count}</td>
                        <td style={{cursor:"default"}}>TBD</td>
                        <td className="text-center">
                            <p className="list-buttons rounded-t-lg" onClick={() => onBookAdd(book)}>+</p>
                            <p className="list-buttons rounded-b-lg" onClick={() => onBookDelete(book)}>-</p>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
};

export default OrderAdminFormBookList