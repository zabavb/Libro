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
    </>
  );
};

export default AdminPage;
