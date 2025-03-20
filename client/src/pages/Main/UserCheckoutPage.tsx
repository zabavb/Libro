import { useNavigate } from 'react-router-dom';
import useBasket from '../../state/context/useBasket';
import UserCheckoutFormContainer from '../../containers/user/UserCheckoutFormContainer';

const UserCheckoutPage = () => {
    const { basket } = useBasket();
    const navigate = useNavigate()

    return (
        <div>
            <header>
                <h1>Checkout</h1>
                <button onClick={() => navigate('/Basket')}>
                    Back to cart
                </button>
            </header>
            <main>
                {basket.map((item) => (
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
