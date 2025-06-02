import React, { createContext, useContext, useEffect, useState } from "react";
import { CartItem } from "@/types/types/cart/CartItem";

type CartContextType = {
  cart: CartItem[];
  addItem: (item: CartItem) => void;
  removeItem: (item: CartItem) => void;
  clearItem: (id: string) => void;
  clearCart: () => void;
  getTotalPrice: () => number;
};

const CartContext = createContext<CartContextType | undefined>(undefined);

const KEY = "cart";

const getCartFromStorage = (): CartItem[] => {
  const storedCart = localStorage.getItem(KEY);
  return storedCart ? JSON.parse(storedCart) : [];
};

export const CartProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [cart, setCart] = useState<CartItem[]>(getCartFromStorage);

  useEffect(() => {
    localStorage.setItem(KEY, JSON.stringify(cart));
  }, [cart]);

  const addItem = (item: CartItem) => {
    setCart((prevCart) => {
      const existingItem = prevCart.find((i) => i.bookId === item.bookId);
      if (existingItem) {
        return prevCart.map((i) =>
          i.bookId === item.bookId ? { ...i, amount: i.amount + item.amount } : i
        );
      }
      return [...prevCart, item];
    });
  };

  const removeItem = (item: CartItem) => {
    setCart((prevCart) => {
      const existingItem = prevCart.find((i) => i.bookId === item.bookId);
      if (existingItem && existingItem.amount - item.amount <= 0) {
        return prevCart.filter((i) => i.bookId !== item.bookId);
      }
      if (existingItem) {
        return prevCart.map((i) =>
          i.bookId === item.bookId ? { ...i, amount: i.amount - item.amount } : i
        );
      }
      return prevCart;
    });
  };

  const clearItem = (id: string) => {
    setCart((prevCart) => prevCart.filter((i) => i.bookId !== id));
  };

  const clearCart = () => {
    setCart([]);
  };

  const getTotalPrice = () => {
    return cart.reduce((sum, item) => sum + item.price * item.amount, 0);
  };

  return (
    <CartContext.Provider
      value={{ cart, addItem, removeItem, clearItem, clearCart, getTotalPrice }}
    >
      {children}
    </CartContext.Provider>
  );
};

export const useCart = (): CartContextType => {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error("useCart must be used within a CartProvider");
  }
  return context;
};
