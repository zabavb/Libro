import React, { useEffect, useState, useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import { Book, ServiceResponse } from '../../types';
import { addBookService, fetchBookByIdService, updateBookService } from '@/services/bookService';
// import { BookFormData } from '@/utils/bookValidationSchema';
// import { BookFormDataToBook } from '@/api/adapters/bookAdapter';
import BookForm from '@/components/book/admin/BookForm';

interface BookFormContainerProps {
  id?: string;
}

const BookFormContainer: React.FC<BookFormContainerProps> = ({ id }) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [isEdit, setIsEdit] = useState<boolean>(false);
  const [serviceResponse, setServiceResponse] = useState<
    ServiceResponse<Book>
  >({
    data: null,
    loading: !!id,
    error: null,
  });

  useEffect(() => {
    if (!id) return;

    (async () => {
      const response = await fetchBookByIdService(id);
      setServiceResponse(response);
      console.log(response)
      if (response.error)
        dispatch(addNotification({ message: response.error, type: 'error' }));
    })();
  }, [id, dispatch]);

  const handleNavigate = useCallback(
    (route: string) => navigate(route),
    [navigate],
  );

  const handleMessage = useCallback(
    (message: string, type: 'success' | 'error') => {
      dispatch(addNotification({ message, type }));
    },
    [dispatch],
  );

  const handleAddBook = useCallback(
    async (bookForm: FormData) => {
      // const book = BookFormDataToBook(bookForm);
      const response = await addBookService(bookForm);

      if (response.error) handleMessage(response.error, 'error');
      else {
        handleMessage('Book created successfully!', 'success');
        handleNavigate('/admin/booksRelated/books');
      }
    },
    [handleMessage, handleNavigate],
  );

  const handleEditBook = useCallback(
    async (updateForm: FormData) => {
      if (!id) return;
      // const book = BookFormDataToBook(updatedBook);
      // book.bookId = id
      // updateForm.append("BookId", id);
      const response = await updateBookService(id, updateForm);
      if (response.error) handleMessage(response.error, 'error');
      else handleMessage('Book updated successfully!', 'success');
    },
    [id, handleMessage],
  );

  return (
    <BookForm
      existingBook={serviceResponse.data ?? undefined}
      onAddBook={handleAddBook}
      onEditBook={handleEditBook}
      loading={serviceResponse.loading}
      isEdit={isEdit}
      onIsEdit={setIsEdit}
    />
  );
};

export default BookFormContainer;
