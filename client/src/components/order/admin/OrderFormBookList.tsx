import React from "react"

interface OrderFormBookListProps {
    bookIds: string[]
    onBookDelete: (bookId: string) => void
}

const OrderFormBookList: React.FC<OrderFormBookListProps> = ({bookIds, onBookDelete}) => {
    
    
    return(
        <div> 
        {bookIds?.map((book) => (
            <div key={book}>
                <p>{book}</p>
                <button onClick={() => onBookDelete(book)}>Delete </button>
            </div>
        ))}
        </div>
    )
}

export default OrderFormBookList