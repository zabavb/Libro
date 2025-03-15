import { useNavigate } from "react-router-dom";
import useBasket from "../../state/context/useBasket"

const UserBasket = () => {

    const { basket, addItem, removeItem, clearBasket, clearItem } = useBasket();
    const navigate = useNavigate()
    return (
        <div>
            {basket.length === 0 ? (
                <p>Your basket is empty.</p>
            ) : (
                <ul>
                    {basket.map((item) => (
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
                <button onClick={clearBasket}>Clear Basket</button>
            </div>
            <div>
                {/* If price isn't calculating during test, replace the bookId with one that is in BookAPI database */}
                <button
                    onClick={() => addItem({ bookId: "685DB04F-01C9-4313-B7A1-27B9EB507398", amount: 1, name: "Емоційний інтелект", price:400.99 })}>
                    Add sample</button>
            </div>
            <div>
                <button onClick={() => navigate("/basket/checkout")}>Checkout</button>
            </div>
        </div>
    )
}

export default UserBasket