import React from "react"
import "@/assets/styles/components/order/order-book-list.css"
import { OrderedBook } from "../../types/types/order/OrderedBook";
interface OrderFormBookListProps {
    books: OrderedBook[];
}
const OrderFormBookList: React.FC<OrderFormBookListProps> = ({books}) => {
    return(
        <div> 
            <table>
                <thead>
                    <tr>
                        <th style={{width:"70%"}}>Title</th>
                        <th style={{width:"10%"}}>X</th>
                        <th className="w-1/5">Price</th>
                    </tr>
                </thead>
                <tbody>
                    {books.map((book) => (
                        <tr key={book.bookId}>
                            <td style={{ cursor: "default" }}>{book.bookId}</td>
                            <td style={{ cursor: "default" }}>{book.quantity}</td>
                            <td style={{ cursor: "default" }}>TBD</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

export default OrderFormBookList