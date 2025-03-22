import { useNavigate } from 'react-router-dom';
import UserCart from '../../components/user/UserCart';

const UserCartPage = () => {
  const navigate = useNavigate();

  const handleGoBack = () => {
    navigate('/');
  };

  return (
    <div>
      <header>
        <h1>Cart</h1>
        <button onClick={handleGoBack}>Back</button>
      </header>
      <main>
        <UserCart />
      </main>
    </div>
  );
};

export default UserCartPage;
