import React, { useEffect, useState } from "react";
import { CartItem } from "@/types/types/cart/CartItem";
import { Book, ServiceResponse } from "@/types";
import { fetchBookByIdService } from "@/services/bookService";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { AppDispatch } from "@/state/redux";
import { useDispatch } from "react-redux";
import "@/assets/styles/components/common/cart-card.css"

interface CheckoutListingProps {
    item: CartItem
}

const CheckoutListing: React.FC<CheckoutListingProps> = ({ item}) => {
    const dispatch = useDispatch<AppDispatch>()
    const [book, setBook] = useState<Book | null>();
    const [serviceResponse, setServiceResponse] = useState<
        ServiceResponse<Book>
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
        <div className="flex justify-between text-gray">
            <p>{book?.title}</p>
            <p>{item.amount} X {item.price} UAH</p>
        </div>
    )
}

export default CheckoutListing