import { useParams } from 'react-router-dom';
import UserFormContainer from '../../../../containers/user/UserFormContainer';

const UserFormPage: React.FC = () => {
  const { userId } = useParams<{ userId: string }>();

  return (
    <div>

      <main>
        <UserFormContainer id={userId ?? ''} />
      </main>
    </div>
  );
};

export default UserFormPage;
