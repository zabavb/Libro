import React from 'react';
import { Role, UserCard as UserCardType } from '../../types';

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
        {(user.role === Role.ADMIN || Role.MODERATOR) && <div>{user.role}</div>}
        <div>{user.email}</div>
        <div>{user.phoneNumber}</div>
        <div>{user.ordersCount}</div>
        <div>{user.lastOrder}</div>

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
