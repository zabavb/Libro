import React, { useEffect, useState, useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import UserForm from '../../components/user/UserForm';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import {
  addUserService,
  editUserService,
  fetchUserByIdService,
} from '../../services';
import { ServiceResponse, User, UserForm as UserFormType } from '../../types';
import { UserFormDataToUser } from '../../api/adapters/userAdapter';
import { UserFormData } from '../../utils';

interface UserFormContainerProps {
  id?: string;
}

const UserFormContainer: React.FC<UserFormContainerProps> = ({ id }) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [serviceResponse, setServiceResponse] = useState<
    ServiceResponse<UserFormType>
  >({
    data: null,
    loading: !!id,
    error: null,
  });

  useEffect(() => {
    if (!id) return;

    (async () => {
      const response = await fetchUserByIdService(id);
      setServiceResponse(response);

      if (response.error)
        dispatch(addNotification({ message: response.error, type: 'error' }));
    })();
  }, [id, dispatch]);

  const handleNavigate = useCallback(
    (route: string) => navigate(route),
    [navigate],
  );

  const handleMessage = useCallback(
    (message: string, type: 'success' | 'error') => {
      dispatch(addNotification({ message, type }));
    },
    [dispatch],
  );

  const handleAddUser = useCallback(
    async (userForm: UserFormData) => {
      const user = UserFormDataToUser(userForm);
      const response = await addUserService(user);

      if (response.error) handleMessage(response.error, 'error');
      else {
        handleMessage('User created successfully!', 'success');
        handleNavigate('/admin/users');
      }
    },
    [handleMessage, handleNavigate],
  );

  const handleEditUser = useCallback(
    async (existingUser: UserFormType, userForm: UserFormData) => {
      if (!id) return;
      const user = UserFormDataToUser(userForm);
      const margedData: User = { ...existingUser, ...user };
      const response = await editUserService(id, margedData);

      if (response.error) handleMessage(response.error, 'error');
      else handleMessage('User updated successfully!', 'success');
    },
    [id, handleMessage],
  );

  return (
    <UserForm
      existingUser={serviceResponse.data ?? undefined}
      onAddUser={handleAddUser}
      onEditUser={handleEditUser}
      loading={serviceResponse.loading}
    />
  );
};

export default UserFormContainer;
