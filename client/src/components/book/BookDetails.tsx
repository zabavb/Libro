import { Book } from "@/types"
import { CartItem } from "@/types/types/cart/CartItem"

interface BookDetailsProps {
    onAddItem: (item: CartItem) => void
    book?: Book
    loading: boolean
}

const BookDetails: React.FC<BookDetailsProps> = ({ onAddItem, book, loading }) => {
    if (loading) return <p>Loading...</p>
    return (
        <div>
            <div>
                <img src={`https://picsum.photos/seed/${book?.bookId}/100/150`} />
            </div>
            <div>
                <h2>{book?.title}</h2>
                {/* Replace with authorname later */}
                <p>{book?.authorId}</p>
            </div>
            <div>
			{book != null ? (
                <button
                 onClick={() => onAddItem({ bookId: book.bookId, amount: 1, name: book.title, price:book.price })}
                 >
                    Add to cart
                </button>
			) : (
				<p>No books found.</p>
			)}  
            </div>
        </div>
    )
}

export default BookDetails