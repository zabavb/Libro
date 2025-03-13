
// This file location may be changed in future

import { useEffect, useState } from "react"
import { BasketItem } from "../../types/types/basket/BasketItem"

const KEY = "basket"

const getBasketFromStorage = (): BasketItem[] => {
    const storedBasket = localStorage.getItem(KEY)
    return storedBasket ? JSON.parse(storedBasket) : []
}

const useBasket = () => {
    const [basket, setBasket] = useState<BasketItem[]>(getBasketFromStorage)

    useEffect(() => {
        localStorage.setItem(KEY, JSON.stringify(basket))
    }, [basket])

    const addItem = (item: BasketItem) => {
        setBasket((prevBasket) => {
            const existingItem = prevBasket.find((i) => i.bookId === item.bookId)
            if (existingItem) {
                return prevBasket.map((i) =>
                    i.bookId === item.bookId ? { ...i, amount: i.amount + item.amount } : i
                )
            }
            return [...prevBasket, item]
        })
    }

    const removeItem = (id: string) => {
        setBasket((prevBasket) => prevBasket.filter((item) => item.bookId !== id));
    }

    const clearBasket = () => {
        setBasket([])
    }

    return {basket, addItem, removeItem, clearBasket}
}

export default useBasket