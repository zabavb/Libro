import React, { useEffect, useState } from "react";
import { CartItem } from "@/types/types/cart/CartItem";
import { ServiceResponse } from "@/types";
import { fetchBookByIdService } from "@/services/bookService";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { AppDispatch } from "@/state/redux";
import { useDispatch } from "react-redux";
import "@/assets/styles/components/common/cart-card.css"
import { BookDetails } from "@/types/types/book/BookDetails";
import noImageUrl from '@/assets/noImage.svg'
interface CartCardProps {
    item: CartItem
    onAdd: (item: CartItem) => void;
    onRemove: (item: CartItem) => void;
    onItemClear: (bookId: string) => void;
}

const CartCard: React.FC<CartCardProps> = ({ item, onAdd, onRemove, onItemClear }) => {
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
        
        <div className="flex gap-[14px] text-[#1A1D23] relative">
            <p className="delete-btn"
             onClick={() => onItemClear(item.bookId)}>
                Delete
            </p>
            <img  className="w-[75px] h-[120px]" src={book?.imageUrl ?? noImageUrl} />
            <div className="flex flex-col gap-4">
                <div>
                    <p className="text-[#1A1D23]">{book?.title}</p>
                    <p className="text-[#929089]">{book?.authorName}</p>
                </div>
                <div>
                    <div className="flex gap-2">
                        <p>{book?.price.toFixed(2)} UAH</p>
                        <p>●</p>
                        <p className="text-[#929089]">{book?.cover}</p>
                    </div>
                    <p className="text-[#FF642E]">{(book?.quantity ?? 0 > 0) ? "Available" : "Not available"}</p>
                </div>
                <div className="flex gap-2.5 items-center">
                    <p className="counter-btn" onClick={() => onRemove(item)}>-</p>
                    <p className="item-counter">{item.amount} psc</p>
                    <p className="counter-btn" onClick={() => onAdd(item)}>+</p>
                </div>
            </div>
        </div>
    )
}

export default CartCard