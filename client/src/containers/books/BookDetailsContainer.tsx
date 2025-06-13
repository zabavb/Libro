import BookDetails from "@/components/book/BookDetails"
import AddToCartToast from "@/components/common/AddToCartToast"
import { fetchBookByIdService } from "@/services/bookService"
import { useCart } from "@/state/context/CartContext"
import { AppDispatch } from "@/state/redux"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { ServiceResponse } from "@/types"
import { BookDetails as BookDetailsType } from "@/types/types/book/BookDetails"
import { CartItem } from "@/types/types/cart/CartItem"
import { useEffect, useState } from "react"
import { useDispatch } from "react-redux"

interface BookDetailsContainerProps {
    bookId: string
}

const BookDetailsContainer: React.FC<BookDetailsContainerProps> = ({ bookId }) => {
    const dispatch = useDispatch<AppDispatch>()
    const { addItem, cart } = useCart()
    const [showToast, setShowToast] = useState(false)
    const [itemCount, setItemCount] = useState(0)
    const [totalPrice, setTotalPrice] = useState(0)
    const [serviceResponse, setServiceResponse] = useState<
        ServiceResponse<BookDetailsType>
    >({
        data: null,
        loading: true,
        error: null,
    })

    useEffect(() => {
        if (!bookId) return;
        (async () => {
            const response = await fetchBookByIdService(bookId);
            setServiceResponse(response);
            if (response.error)
                dispatch(addNotification({ message: response.error, type: 'error' }));
        })();
    }, [bookId, dispatch])

    const handleAddItem = (item: CartItem) => {
        try {
            addItem(item);
            const count = cart.reduce((acc: number, i: CartItem) => acc + i.amount, 0) + item.amount
            const total = cart.reduce((acc: number, i: CartItem) => acc + i.amount * i.price, 0) + item.amount * item.price


            setItemCount(count)
            setTotalPrice(total)
            setShowToast(true)

            setTimeout(() => setShowToast(false), 5000)

            // dispatch(
            //     addNotification({
            //         message: `Item has been successfully added to the cart`,
            //         type: "success"
            //     })
            // )
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
        <>
            <BookDetails
                onAddItem={handleAddItem}
                book={serviceResponse.data as BookDetailsType}
                loading={serviceResponse.loading}
            />
            {showToast && (
                <AddToCartToast
                    itemCount={itemCount}
                    totalPrice={totalPrice}
                    onClose={() => setShowToast(false)}
                />
            )}
        </>
    )
}

export default BookDetailsContainer