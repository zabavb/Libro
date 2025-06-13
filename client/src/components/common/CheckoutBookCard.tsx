import { fetchBookByIdService } from "@/services/bookService"
import { AppDispatch } from "@/state/redux"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { Book, ServiceResponse } from "@/types"
import { BookDetails } from "@/types/types/book/BookDetails"
import { CartItem } from "@/types/types/cart/CartItem"
import React, { useEffect, useState } from "react"
import { useDispatch } from "react-redux"
import noImageUrl from "@/assets/noImage.svg"
interface CheckoutBookCardProps {
    item: CartItem
}

const CheckoutBookCard: React.FC<CheckoutBookCardProps> = ({ item }) => {
    const dispatch = useDispatch<AppDispatch>()
    const [book, setBook] = useState<BookDetails | null>();
    const [serviceResponse, setServiceResponse] = useState<
        ServiceResponse<BookDetails>
    >({
        data: null,
        loading: true,
        error: null,
    })

    useEffect(() => {
        (async () => {
            const response = await fetchBookByIdService(item.bookId);
            setServiceResponse(response);
            if (response.error)
                dispatch(addNotification({ message: response.error, type: 'error' }));
            setBook(response.data);
        })();
    }, [item, dispatch])
    return (
        <div className="flex gap-3.5">
            <div>
                <img src={book?.imageUrl ?? noImageUrl} className="w-[70px] h-[110px]" />
            </div>
            <div className="flex flex-col gap-[5px]">
                <div className="flex justify-between font-semibold text-3xl gap-7">
                    <p>{book?.title}</p>
                    <p>{item.amount} psc.</p>
                </div>
                <div>
                    <p className="text-gray">{book?.authorName}</p>
                </div>
                <div>
                    <p className="text-gray">{book?.cover}</p>
                </div>
            </div>
        </div>
    )
}

export default CheckoutBookCard