import { useNavigate } from 'react-router-dom';

const ForbiddenPage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <>
      <h1>403: Access Denied</h1>
      <button onClick={() => navigate('/login')}>Login</button>
    </>
  );
};

export default ForbiddenPage;
