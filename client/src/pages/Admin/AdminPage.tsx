import { useNavigate } from "react-router-dom";

const AdminPage = () => {
  const navigate = useNavigate();

  return (
    <div>
      <h1>Admin Dashboard</h1>
      <div>
        <button onClick={() => navigate("/admin/users")}>
          Manage Users
        </button>
      </div>
    </div>
  );
};

export default AdminPage;
