import { useNavigate } from 'react-router-dom';
import DeliveryTypeListContainer from '../../../../containers/order/DeliveryTypeListContainer';

const DeliveriesListPage = () => {
  const navigate = useNavigate();

  return (
    <div>
      <header>
        <h1>Delivery Type Management</h1>
        <button onClick={() => navigate('/admin')}>
          Back to Admin Dashboard
        </button>
      </header>
      <main>
        <div>
          <button onClick={() => navigate('/admin/deliverytypes/add')}>Add Delivery Type</button>
        </div>
        <DeliveryTypeListContainer />
      </main>
    </div>
  );
};

export default DeliveriesListPage;
