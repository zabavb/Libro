import { useNavigate } from 'react-router-dom';
import '../../index.css';

const MainPage: React.FC = () => {
  const navigate = useNavigate();

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
    </div>
  );
};

export default MainPage;
