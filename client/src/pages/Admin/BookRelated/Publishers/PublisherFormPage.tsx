import PublisherFormContainer from '@/containers/books/PublisherFormContainer';
import { useParams } from 'react-router-dom';

const PublisherFormPage = () => {
  const { publisherId } = useParams<{ publisherId: string }>();
  return (
    <div>
      <main>
        <PublisherFormContainer id={publisherId}/>
      </main>
    </div>
  );
};

export default PublisherFormPage;
