import { useNavigate } from "react-router-dom";
import useBasket from "../../state/context/useBasket"

const UserBasket = () => {

    const { basket, addItem, removeItem, clearBasket, clearItem } = useBasket();
    const navigate = useNavigate()
    return (
        <div>
            <h2>Shopping Cart</h2>
            {basket.length === 0 ? (
                <p>Your basket is empty.</p>
            ) : (
                <ul>
                    {basket.map((item) => (
                        <li key={item.bookId}>
                            {item.name} x ({item.amount})
                            <div>
                                <button
                                    onClick={() => removeItem({ bookId: item.bookId, amount: 1, name: item.name })}>
                                    -
                                </button>
                                <button
                                    onClick={() => addItem({ bookId: item.bookId, amount: 1, name: item.name })}>
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
                <button
                    onClick={() => addItem({ bookId: "1", amount: 1, name: "sample name" })}>
                    Add sample</button>
            </div>
            <div>
                <button onClick={() => navigate("/basket/checkout")}>Basket</button>
            </div>
        </div>
    )
}

export default UserBasket