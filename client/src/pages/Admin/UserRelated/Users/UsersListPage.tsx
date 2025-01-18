import { useNavigate } from "react-router-dom";
import UserList from "../../../../components/user/UserList";

const UserListPage = () => {
  const navigate = useNavigate();

  return (
    <div>
      <header style={styles.header}>
        <h1>User Management</h1>
        <button style={styles.backButton} onClick={() => navigate("/admin")}>
          Back to Admin Dashboard
        </button>
      </header>
      <main style={styles.main}>
        <UserList />
      </main>
    </div>
  );
};

const styles = {
  header: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    padding: "16px",
    borderBottom: "1px solid #ddd",
  },
  backButton: {
    padding: "8px 16px",
    backgroundColor: "#007bff",
    color: "#fff",
    border: "none",
    borderRadius: "4px",
    cursor: "pointer",
  },
  main: {
    padding: "16px",
  },
};

export default UserListPage;
