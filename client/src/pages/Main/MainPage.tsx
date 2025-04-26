import { useNavigate } from 'react-router-dom';
import '../../index.css';
import { useAuth } from '@/state/context';

const MainPage: React.FC = () => {
  const navigate = useNavigate();
  const { logout } = useAuth();

  return (
    <div>
      <h1>Main page</h1>
      <div>
        <button onClick={() => navigate('/admin')}>Admin dashboard</button>
      </div>
      <div>
        <button onClick={() => navigate('/cart')}>Cart</button>
      </div>
      <div>
        <button onClick={() => navigate('/catalog')}>Catalog</button>
      </div>
      <div>
        <button
          onClick={() =>
            navigate('/subscriptions/78d416c7-c6af-41c8-a1fb-913180408686')
          }
        >
          Subscribe for 365
        </button>
      </div>
      <div>
        <button onClick={logout}>Logout</button>
      </div>
    </div>
  );
};

export default MainPage;
