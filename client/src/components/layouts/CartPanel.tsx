import React, { useEffect, useState } from "react";
import cartIcon from '@/assets/icons/cartBig.svg'
import '@/assets/styles/layout/cart-panel.css'
import { useCart } from "@/state/context/CartContext";
import closeIcon from '@/assets/icons/menuClose.svg'
import CartCard from "../common/CartCard";
import { CartItem } from "@/types/types/cart/CartItem";
import { useNavigate } from "react-router-dom";
const CartPanel: React.FC = () => {
    const navigate = useNavigate();
    const [isOpen, setIsOpen] = useState<boolean>(false);
    const [total, setTotal] = useState<number>(0);
    const { cart, removeItem, addItem, clearCart, clearItem, getTotalPrice } = useCart();

    const handleAdd = (item: CartItem) => {
        addItem({ bookId: item.bookId, amount: 1, name: item.name, price: item.price });
    };

    const handleRemove = (item: CartItem) => {
        removeItem({ bookId: item.bookId, amount: 1, name: item.name, price: item.price });
    };

    const handleClear = (bookId: string) => {
        clearItem(bookId)
    }

    useEffect(() => {
        setTotal(getTotalPrice());
    }, [cart, getTotalPrice])

    useEffect(() => {
        if (isOpen) {
            document.body.style.overflow = 'hidden';
        } else {
            document.body.style.overflow = '';
        }

        return () => {
            document.body.style.overflow = '';
        };
    }, [isOpen])


    return (
        <div>
            <img src={cartIcon}
                className='cursor-pointer'
                onClick={() => setIsOpen(true)}
            />

            <div
                className={`dim ${isOpen && 'visible'}`}
                aria-hidden={!isOpen}
                onClick={() => setIsOpen(false)}
            />
            <div className={`cart-panel ${isOpen ? 'right-0' : 'right-[-445px]'}`}>
                <div className="p-[30px] bg-[#1A1D23] h-[17%]">
                    <img src={closeIcon} className="close-icon" onClick={() => setIsOpen(false)} />
                    <h1 className="font-semibold text-2xl">Cart</h1>

                    {cart.length > 0 ? (
                        <div className="flex justify-between">
                            <p className="text-[#929089]">{cart.length} psc.</p>
                            <p className="text-[#929089] cursor-pointer" onClick={clearCart}>Clear cart</p>
                        </div>
                    ) : ""}
                </div>
                <div className="cart-main">
                    {cart.length > 0 ?
                        cart.map((item) => (
                            <CartCard
                                key={item.bookId}
                                item={item}
                                onAdd={handleAdd}
                                onRemove={handleRemove}
                                onItemClear={handleClear} />
                        ))
                        :
                        (
                            <div className="font-semibold text-lg text-[#1A1D23]">
                                <p className="text-[#FF642E]">Your cart is empty.</p>
                                <p>Don't hesitate and browse our catalog to find something good for you!</p>
                            </div>
                        )
                    }
                </div>
                {cart.length > 0 ? (
                    <div className="cart-footer">
                        <div className="flex justify-between font-semibold text-2xl">
                            <p>Total</p>
                            <p>{total} UAH</p>
                        </div>
                        <div>
                            <button className="checkout-btn" onClick={() => navigate("/checkout")}>To Checkout</button>
                        </div>
                    </div>
                ) : ""}
            </div>
        </div>
    );
};

export default CartPanel;
