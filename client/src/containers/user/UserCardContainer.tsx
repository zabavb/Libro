import React from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { AppDispatch } from '../../state/redux/index';
import { UserCard as UserCardType } from '../../types';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import UserCard from '../../components/user/UserCard';
import { removeUserService } from '../../services';

interface UserCardContainerProps {
  user: UserCardType;
  onDeleted: (isDeleted: boolean) => void;
}

const UserCardContainer: React.FC<UserCardContainerProps> = ({
  user,
  onDeleted,
}) => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const handleNavigate = () => {
    navigate(`/admin/users/${user.id}`);
  };

  const handleDelete = async (e: React.MouseEvent) => {
    e.stopPropagation();
    const response = await removeUserService(user.id);

    if (response.error)
      dispatch(
        addNotification({
          message: response.error,
          type: 'error',
        }),
      );
    else {
      onDeleted(true);
      dispatch(
        addNotification({
          message: 'User successfully deleted.',
          type: 'success',
        }),
      );
    }
  };

  return (
    <UserCard user={user} onNavigate={handleNavigate} onDelete={handleDelete} />
  );
};

export default UserCardContainer;
