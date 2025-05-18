import React, { useEffect, useState, useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import { Author, ServiceResponse } from '../../types';
import AuthorForm from '@/components/book/admin/AuthorForm';
import { addAuthorService, fetchAuthorByIdService, updateAuthorService } from '@/services/authorService';
import { AuthorFormData } from '@/utils';
import { AuthorFormDataToAuthor } from '@/api/adapters/bookAdapter';

interface AuthorFormContainerProps {
  id?: string;
}

const AuthorFormContainer: React.FC<AuthorFormContainerProps> = ({ id }) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [isEdit, setIsEdit] = useState<boolean>(false);
  const [serviceResponse, setServiceResponse] = useState<
    ServiceResponse<Author>
  >({
    data: null,
    loading: !!id,
    error: null,
  });

  useEffect(() => {
    if (!id) return;

    (async () => {
      const response = await fetchAuthorByIdService(id);
      setServiceResponse(response);
      
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

  const handleAddAuthor = useCallback(
    async (authorForm: AuthorFormData) => {
      const author = AuthorFormDataToAuthor(authorForm);
      const response = await addAuthorService(author);

      if (response.error) handleMessage(response.error, 'error');
      else {
        handleMessage('Author created successfully!', 'success');
        handleNavigate('/admin/booksRelated/authors');
      }
    },
    [handleMessage, handleNavigate],
  );

  const handleEditAuthor = useCallback(
    async (updatedAuthor: AuthorFormData) => {
      if (!id) return;
      const author = AuthorFormDataToAuthor(updatedAuthor);
      author.authorId = id
      const response = await updateAuthorService(id, author);
      if (response.error) handleMessage(response.error, 'error');
      else handleMessage('Author updated successfully!', 'success');
    },
    [id, handleMessage],
  );

  return (
    <AuthorForm
      existingAuthor={serviceResponse.data ?? undefined}
      onAddAuthor={handleAddAuthor}
      onEditAuthor={handleEditAuthor}
      loading={serviceResponse.loading}
      isEdit={isEdit}
      onIsEdit={setIsEdit}
    />
  );
};

export default AuthorFormContainer;
