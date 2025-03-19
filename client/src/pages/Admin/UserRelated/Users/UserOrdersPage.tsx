import { useNavigate, useParams } from 'react-router-dom';
import UserOrdersContainer from '../../../../containers/user/UserOrdersListContainer';

const UserOrdersPage = () => {
  const navigate = useNavigate();
  // Temporary
  const { userId } = useParams<{userId: string}>()

  return (
    <div>
      <header>
        <h1>My Orders</h1>
        <button onClick={() => navigate('/')}>
          Back
        </button>
      </header>
      <main>
        <UserOrdersContainer
            userId={userId != undefined ? userId : ""} />
      </main>
    </div>
  );
};

export default UserOrdersPage;
