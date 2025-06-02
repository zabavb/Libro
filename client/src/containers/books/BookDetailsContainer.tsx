import BookDetails from "@/components/book/BookDetails"
import { fetchBookByIdService } from "@/services/bookService"
import { useCart } from "@/state/context/CartContext"
import { AppDispatch } from "@/state/redux"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { Book, ServiceResponse } from "@/types"
import { CartItem } from "@/types/types/cart/CartItem"
import { useEffect, useState } from "react"
import { useDispatch } from "react-redux"

interface BookDetailsContainerProps {
    bookId: string
}

const BookDetailsContainer: React.FC<BookDetailsContainerProps> = ({ bookId }) => {
    const dispatch = useDispatch<AppDispatch>()
    const { addItem } = useCart();
    const [serviceResponse, setServiceResponse] = useState<
        ServiceResponse<Book>
    >({
        data: null,
        loading: true,
        error: null,
    })

    useEffect(() => {
        if (!bookId) return;
        console.log("hello3");
        (async () => {
            console.log("hello2")
            const response = await fetchBookByIdService(bookId);
            setServiceResponse(response);
            if (response.error)
                dispatch(addNotification({ message: response.error, type: 'error' }));
        })();
    }, [bookId,dispatch])

    const handleAddItem = (item: CartItem) => {
        try {
            addItem(item);
            dispatch(
                addNotification({
                    message: `Item has been successfully added to the cart`,
                    type: "success"
                })
            )
        } catch (error) {
            dispatch(
                addNotification({
                    message: error instanceof Error ? error.message : String(error),
                    type: "error"
                })
            )
        }
    }

    return (
        <BookDetails
            onAddItem={handleAddItem}
            book={serviceResponse.data as Book}
            loading={serviceResponse.loading}
        />
    )
}

export default BookDetailsContainer