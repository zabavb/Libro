import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../state/redux';
import { useAuth } from '../../state/context';
import { useNavigate } from 'react-router-dom';
import { useCallback } from 'react';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import Login from '../../components/auth/Login';
import { LoginFormData } from '../../utils';
import { NotificationData } from '../../types';

const LoginContainer: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (userData: LoginFormData) => {
    const data = await login(userData);
    if (data.type === 'success') handleSuccess(data);
    else handleError(data);
  };

  const handleSuccess = useCallback(
    (data: NotificationData) => {
      dispatch(addNotification(data));
      navigate('/');
    },
    [dispatch, navigate],
  );

  const handleError = useCallback(
    (data: NotificationData) => dispatch(addNotification(data)),
    [dispatch],
  );

  return <Login onSubmit={handleSubmit} />;
};

export default LoginContainer;
