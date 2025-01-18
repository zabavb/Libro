import React from "react";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { removeUser } from "../../state/redux/slices/userSlice";
import { AppDispatch } from "../../state/redux/store";

interface UserCardProps {
  user: {
    id: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    email: string;
    phoneNumber: string;
    role: string;
  };
}

const UserCard: React.FC<UserCardProps> = ({ user }) => {
  const navigate = useNavigate();
  const dispatch = useDispatch<AppDispatch>();

  const handleDelete = (e: React.MouseEvent) => {
    e.stopPropagation();
    dispatch(removeUser(user.id));
  };

  return (
    <li
      style={styles.card}
      onClick={() => navigate(`/admin/users/${user.id}`)}
    >
      <div>
        <p><strong>First Name:</strong> {user.firstName}</p>
        <p><strong>Last Name:</strong> {user.lastName}</p>
        <p><strong>Date of Birth:</strong> {user.dateOfBirth}</p>
        <p><strong>Email:</strong> {user.email}</p>
        <p><strong>Phone Number:</strong> {user.phoneNumber}</p>
        <p><strong>Role:</strong> {user.role}</p>
      </div>
      <button
        style={styles.deleteButton}
        onClick={handleDelete}
      >
        Delete
      </button>
    </li>
  );
};

const styles = {
  card: {
    border: "1px solid #ddd",
    borderRadius: "8px",
    padding: "16px",
    cursor: "pointer",
    width: "300px",
    transition: "transform 0.2s, box-shadow 0.2s",
    boxShadow: "0 2px 4px rgba(0, 0, 0, 0.1)",
  },
  deleteButton: {
    marginTop: "10px",
    padding: "8px 16px",
    backgroundColor: "#dc3545",
    color: "#fff",
    border: "none",
    borderRadius: "4px",
    cursor: "pointer",
  },
};

export default UserCard;
