import React from "react"
import "@/assets/styles/components/order/order-book-list.css"
interface OrderFormBookListProps {
    books: Record<string,number>
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
                {Object.entries(books)?.map(([book,count]) => (
                    <tr>
                        <td style={{cursor:"default"}}>{book}</td>
                        <td style={{cursor:"default"}}>{count}</td>
                        <td style={{cursor:"default"}}>TBD</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    )
}

export default OrderFormBookList