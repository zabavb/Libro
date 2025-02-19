import { useNavigate } from "react-router-dom";
import "../../index.css"

const AdminPage = () => {
  const navigate = useNavigate();

  return (
    <>
      <h1>Admin Dashboard</h1>
      <button onClick={() => navigate("/admin/users")}>
        Manage Users
      </button>
      <button onClick={() => navigate("/admin/orders")}>
        Manage Orders
      </button>
    </>
  );
};

export default AdminPage;
