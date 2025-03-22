import { useNavigate } from "react-router-dom";
import useCart from "../../state/context/useCart";

const UserCart = () => {

    const { cart, addItem, removeItem, clearCart, clearItem } = useCart();
    const navigate = useNavigate()
    return (
        <div>
            {cart.length === 0 ? (
                <p>Your cart is empty.</p>
            ) : (
                <ul>
                    {cart.map((item) => (
                        <li key={item.bookId}>
                            {item.name} x ({item.amount})
                            <div>
                                <button
                                    onClick={() => removeItem({ bookId: item.bookId, amount: 1, name: item.name, price:item.price })}>
                                    -
                                </button>
                                <button
                                    onClick={() => addItem({ bookId: item.bookId, amount: 1, name: item.name, price:item.price})}>
                                    +
                                </button>
                                <button onClick={() => clearItem(item.bookId)}>
                                    Remove
                                </button>
                            </div>
                        </li>
                    ))}
                </ul>
            )}
            <div>
                <button onClick={clearCart}>Clear Basket</button>
            </div>
            <div>
                {/* If price isn't calculating during test, replace the bookId with one that is in BookAPI database */}
                <button
                    onClick={() => addItem({ bookId: "685DB04F-01C9-4313-B7A1-27B9EB507398", amount: 1, name: "Емоційний інтелект", price:400.99 })}>
                    Add sample</button>
            </div>
            <div>
                <button onClick={() => navigate("/cart/checkout")}>Checkout</button>
            </div>
        </div>
    )
}

export default UserCart