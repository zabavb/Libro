import { useNavigate } from "react-router-dom";
import "../../index.css"

const AdminPage = () => {
  const navigate = useNavigate();

  return (
    <>
      <h1>Admin Dashboard</h1>
      <div>
        <button onClick={() => navigate('/admin/users')}>Manage Users</button>
      </div>
      <div>
        <button onClick={() => navigate('/admin/subscriptions')}>
          Manage Subscriptions
        </button>
      </div>
      <div>
        <button onClick={() => navigate('/admin/orders')}>Manage Orders</button>
      </div>
      <div>
        <button onClick={() => navigate('/admin/deliverytypes')}>
          Manage Delivery Types
        </button>
      </div>
    </>
  );
};

export default AdminPage;
