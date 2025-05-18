import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../state/redux';
import { useAuth } from '../../state/context';
import { useNavigate } from 'react-router-dom';
import { useCallback } from 'react';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import Login from '../../components/auth/Login';
import { LoginFormData } from '../../utils';
import { NotificationData, User } from '../../types';

const LoginContainer: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { oAuth, login } = useAuth();
  const navigate = useNavigate();

  const handleOAuth = async (token: string | undefined) => {
    const data = await oAuth(token);

    // If it is new user
    if (data as User) navigate('/', { state: { authOpen:true, user: data as User } });

    if ((data as NotificationData).type === 'success')
      handleSuccess({ type: 'success', message: 'Login successful!' });
    else handleError(data as NotificationData);
  };

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

  return <Login onOAuth={handleOAuth} onSubmit={handleSubmit} />;
};

export default LoginContainer;
