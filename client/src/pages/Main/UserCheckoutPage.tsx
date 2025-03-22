import { useNavigate } from 'react-router-dom';
import UserCheckoutFormContainer from '../../containers/user/UserCheckoutFormContainer';
import useCart from '../../state/context/useCart';

const UserCheckoutPage = () => {
    const { cart } = useCart();
    const navigate = useNavigate()

    return (
        <div>
            <header>
                <h1>Checkout</h1>
                <button onClick={() => navigate('/Cart')}>
                    Back to cart
                </button>
            </header>
            <main>
                {cart.map((item) => (
                    <li key={item.bookId}>
                        {item.name} x ({item.amount})
                    </li>
                ))}
                <UserCheckoutFormContainer />
            </main>
        </div>
    );
};

export default UserCheckoutPage;
