import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from './state/context';
import { Role, User } from './types';
import { roleEnumToNumber } from './api/adapters/userAdapter';

// const DISABLE_AUTH = true;

const PrivateRoute = () => {
  const { token } = useAuth();
  const user = JSON.parse(localStorage.getItem('user') || '{}') as User;

  const isPrivileged =
    token &&
    user &&
    (user.role === roleEnumToNumber(Role.ADMIN) ||
      user.role === roleEnumToNumber(Role.MODERATOR));

  return isPrivileged ? <Outlet /> : <Navigate to='/login' state={{ authOpen: true}}/>;
};

export default PrivateRoute;
