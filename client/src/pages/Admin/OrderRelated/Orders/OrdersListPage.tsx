import { useNavigate } from 'react-router-dom';
import OrderListContainer from '../../../../containers/order/OrderListContainer';

const OrderListPage = () => {
  const navigate = useNavigate();

  return (
    <div>
      <header>
        <h1>Order Management</h1>
        <button onClick={() => navigate('/admin')}>
          Back to Admin Dashboard
        </button>
      </header>
      <main>
        <OrderListContainer />
      </main>
    </div>
  );
};

export default OrderListPage;
