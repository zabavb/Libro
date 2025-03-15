import { useDispatch } from 'react-redux';
import Register from '../../components/auth/Register';
import { AppDispatch } from '../../state/redux';
import { useNavigate } from 'react-router-dom';
import { useCallback } from 'react';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import { useAuth } from '../../state/context';
import { RegisterFormData } from '../../utils';
import { NotificationData } from '../../types';

const RegisterContainer: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (userData: RegisterFormData) => {
    const data = await register(userData);
    if (data.type === 'success') handleSuccess(data);
    else handleError(data);
  };

  const handleSuccess = useCallback(
    (data: NotificationData) => {
      dispatch(addNotification(data));
      navigate('/login');
    },
    [dispatch, navigate],
  );

  const handleError = useCallback(
    (data: NotificationData) => dispatch(addNotification(data)),
    [dispatch],
  );

  return <Register onSubmit={handleSubmit} />;
};

export default RegisterContainer;
