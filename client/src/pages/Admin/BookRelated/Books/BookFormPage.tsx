import BookFormContainer from '@/containers/books/BookFormContainer';
import { useParams } from 'react-router-dom';

const BookFormPage = () => {
  const { bookId } = useParams<{ bookId: string }>();
  return (
    <div>
      <main>
        <BookFormContainer id={bookId ?? ''} />
      </main>
    </div>
  );
};

export default BookFormPage;
