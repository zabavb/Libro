
// This file location may be changed in future

import { useEffect, useState } from "react"
import { CartItem } from "../../types/types/cart/CartItem"

const KEY = "cart"

const getCartFromStorage = (): CartItem[] => {
    const storedCart = localStorage.getItem(KEY)
    return storedCart ? JSON.parse(storedCart) : []
}

const useCart = () => {
    const [cart, setCart] = useState<CartItem[]>(getCartFromStorage)

    useEffect(() => {
        localStorage.setItem(KEY, JSON.stringify(cart))
    }, [cart])

    const addItem = (item: CartItem) => {
        setCart((prevCart) => {
            const existingItem = prevCart.find((i) => i.bookId === item.bookId)
            if (existingItem) {
                return prevCart.map((i) =>
                    i.bookId === item.bookId ? { ...i, amount: i.amount + item.amount } : i
                )
            }
            return [...prevCart, item]
        })
    }

    const removeItem = (item: CartItem) => {
        setCart((prevCart) => {
            const existingItem = prevCart.find((i) => i.bookId === item.bookId)
            if (existingItem && existingItem.amount - item.amount <= 0) {
                return prevCart.filter((i) => i.bookId != item.bookId)
            }
            if (existingItem) {
                return prevCart.map((i) =>
                    i.bookId === item.bookId ? { ...i, amount: i.amount - item.amount } : i
                )
            }
            return [...prevCart, item]
        })
    }

    const clearItem = (id: string) => 
    {
        setCart((prevCart) => {
            return prevCart.filter((i) => i.bookId != id)
        })
    }

    const clearCart = () => {
        setCart([])
    }

    const getTotalPrice = () => {
        return cart.reduce((sum, item) => sum + item.price * item.amount, 0);
    };

    return { cart, addItem, removeItem, clearItem, clearCart, getTotalPrice }
    
}

export default useCart
