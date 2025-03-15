import { useNavigate } from 'react-router-dom';
import UserBasket from '../../../../components/user/UserBasket';

const UserBasketPage = () => {
  const navigate = useNavigate();

  const handleGoBack = () => {
    navigate('/');
  };

  return (
    <div>
      <header>
        <h1>Basket</h1>
        <button onClick={handleGoBack}>Back</button>
      </header>
      <main>
        <UserBasket />
      </main>
    </div>
  );
};

export default UserBasketPage;
