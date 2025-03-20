import { useLocation } from 'react-router-dom';
import RegisterContainer from '../../containers/auth/RegisterContainer';
import { User } from '../../types';

const RegisterPage: React.FC = () => {
  const location = useLocation();
  const user = location.state?.user as User | undefined;

  return <RegisterContainer user={user} />;
};

export default RegisterPage;
