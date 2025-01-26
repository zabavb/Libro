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
    <li onClick={() => navigate(`/admin/users/${user.id}`)}>
      <div>
        <p><strong>First Name:</strong> {user.firstName}</p>
        <p><strong>Last Name:</strong> {user.lastName}</p>
        <p><strong>Date of Birth:</strong> {user.dateOfBirth}</p>
        <p><strong>Email:</strong> {user.email}</p>
        <p><strong>Phone Number:</strong> {user.phoneNumber}</p>
        <p><strong>Role:</strong> {user.role}</p>
      </div>
      <button onClick={handleDelete}>Delete</button>
    </li>
  );
};

export default UserCard;
