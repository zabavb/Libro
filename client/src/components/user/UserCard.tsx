import React from 'react';
import { UserCard as UserCardType } from '../../types';

interface UserCardProps {
  user: UserCardType;
  onNavigate: () => void;
  onDelete: (e: React.MouseEvent) => void;
}

const UserCard: React.FC<UserCardProps> = ({ user, onNavigate, onDelete }) => {
  return (
    <>
      <hr />
      <li
        onClick={(e) => {
          e.stopPropagation();
          onNavigate();
        }}
      >
        <div>{user.fullName}</div>
        <div>{user.email}</div>
        <div>{user.phoneNumber}</div>
        <div>{user.role}</div>
        <div>Edit</div>
        <div
          onClick={(e) => {
            e.stopPropagation();
            onDelete(e);
          }}
        >
          Remove
        </div>
      </li>
    </>
  );
};

export default UserCard;
