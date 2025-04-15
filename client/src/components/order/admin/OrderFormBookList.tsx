import React from "react"
import "@/assets/styles/components/order-book-list.css"
interface OrderFormBookListProps {
    books: Record<string,number>
    onBookDelete: (bookId: string) => void
    onBookAdd: (bookId: string) => void
}

const OrderFormBookList: React.FC<OrderFormBookListProps> = ({books, onBookDelete,onBookAdd}) => {
    
    
    return(
        <div> 
            <table>
                <thead>
                    <tr>
                        <th style={{width:"70%"}}>Title</th>
                        <th style={{width:"5%"}}>X</th>
                        <th className="w-1/5">Price</th>
                        <th style={{width:"5%"}}></th>
                    </tr>
                </thead>
                <tbody>
                {Object.entries(books)?.map(([book,count]) => (
                    <tr>
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
    )
}

export default OrderFormBookList