import React, { useEffect, useState } from "react";
import { CartItem } from "@/types/types/cart/CartItem";
import { Book, ServiceResponse } from "@/types";
import { fetchBookByIdService } from "@/services/bookService";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { AppDispatch } from "@/state/redux";
import { useDispatch } from "react-redux";
import "@/assets/styles/components/common/cart-card.css"

interface CartCardProps {
    item: CartItem
    onAdd: (item: CartItem) => void;
    onRemove: (item: CartItem) => void;
    onItemClear: (bookId: string) => void;
}

const CartCard: React.FC<CartCardProps> = ({ item, onAdd, onRemove, onItemClear }) => {
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
            console.log(response)
            setBook(response.data);
        })();
    }, [item, dispatch])



    return (
        
        <div className="flex gap-[14px] text-dark relative">
            <p className="delete-btn"
             onClick={() => onItemClear(item.bookId)}>
                Delete
            </p>
            <img src={`https://picsum.photos/seed/${item.bookId}/80/120`} />
            <div className="flex flex-col gap-4">
                <div>
                    <p className="text-black">{book?.title}</p>
                    <p className="text-gray">AUTHOR_TMP</p>
                </div>
                <div>
                    <p>{book?.price} UAH</p>
                    <p className="text-accent">{(book?.quantity ?? 0 > 0) ? "Available" : "Not available"}</p>
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