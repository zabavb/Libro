import AuthorFormContainer from '@/containers/books/AuthorFormContainer';
import { useParams } from 'react-router-dom';

const AuthorFormPage = () => {
  const { authorId } = useParams<{ authorId: string }>();
  return (
    <div>
      <main>
        <AuthorFormContainer id={authorId ?? ''} />
      </main>
    </div>
  );
};

export default AuthorFormPage;
