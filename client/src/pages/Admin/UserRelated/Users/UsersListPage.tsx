import { useNavigate } from "react-router-dom";
import UserList from "../../../../components/user/UserList";

const UserListPage = () => {
  const navigate = useNavigate();

  return (
    <div>
      <header>
        <h1>User Management</h1>
        <button onClick={() => navigate("/admin")}>
          Back to Admin Dashboard
        </button>
      </header>
      <main>
        <UserList />
      </main>
    </div>
  );
};

export default UserListPage;