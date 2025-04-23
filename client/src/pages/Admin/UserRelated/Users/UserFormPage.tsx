import { useParams, useNavigate } from 'react-router-dom';
import UserFormContainer from '../../../../containers/user/UserFormContainer';

const UserFormPage: React.FC = () => {
  const { userId } = useParams<{ userId: string }>();
  const navigate = useNavigate();

  const handleGoBack = () => {
    navigate('/admin/users');
  };

  return (
    <div>
      <header>
        <h1>{userId ? 'Edit User' : 'Add User'}</h1>
        <button onClick={handleGoBack}>Back to User List</button>
      </header>
      <main>
        <UserFormContainer id={userId ?? ''} />
      </main>
    </div>
  );
};

export default UserFormPage;
