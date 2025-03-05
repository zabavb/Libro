import React from "react"

interface OrderFormBookListProps {
    books: Record<string,number>
    onBookDelete: (bookId: string) => void
    onBookAdd: (bookId: string) => void
}

const OrderFormBookList: React.FC<OrderFormBookListProps> = ({books, onBookDelete,onBookAdd}) => {
    
    
    return(
        <div> 
        {Object.entries(books)?.map(([book,count]) => (
            <div key={book}>
                <p>{book} (x{count})</p>
                <div style={{display:"flex"}}>
                <p style={{cursor:"pointer"}} onClick={() => onBookDelete(book)}>-</p>
                <p style={{cursor:"pointer"}} onClick={() => onBookAdd(book)}>+</p>
                </div>
            </div>
        ))}
        </div>
    )
}

export default OrderFormBookList