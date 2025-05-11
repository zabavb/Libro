import React, { useEffect, useState, useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import { ServiceResponse } from '../../types';
import { PublisherFormData } from '@/utils';
import { PublisherFormDataToPublisher } from '@/api/adapters/bookAdapter';
import { Publisher } from '@/types/types/book/Publisher';
import { addPublisherService, editPublisherService, fetchPublisherByIdService } from '@/services';
import PublisherForm from '@/components/book/admin/PublisherForm';

interface PublisherFormContainerProps {
  id?: string;
}

const PublisherFormContainer: React.FC<PublisherFormContainerProps> = ({ id }) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [isEdit, setIsEdit] = useState<boolean>(false);
  const [serviceResponse, setServiceResponse] = useState<
    ServiceResponse<Publisher>
  >({
    data: null,
    loading: !!id,
    error: null,
  });

  useEffect(() => {
    if (!id) return;

    (async () => {
      const response = await fetchPublisherByIdService(id);
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

  const handleAddPublisher = useCallback(
    async (publisherForm: PublisherFormData) => {
      const publisher = PublisherFormDataToPublisher(publisherForm);
      const response = await addPublisherService(publisher);

      if (response.error) handleMessage(response.error, 'error');
      else {
        handleMessage('Publisher created successfully!', 'success');
        handleNavigate('/admin/booksRelated/publishers');
      }
    },
    [handleMessage, handleNavigate],
  );

  const handleEditAuthor = useCallback(
    async (updatedPublisher: PublisherFormData) => {
      if (!id) return;
      const publisher = PublisherFormDataToPublisher(updatedPublisher);
      publisher.publisherId = id
      const response = await editPublisherService(id, publisher);
      if (response.error) handleMessage(response.error, 'error');
      else handleMessage('Publisher updated successfully!', 'success');
    },
    [id, handleMessage],
  );

  return (
    <PublisherForm
        existingPublisher={serviceResponse.data ?? undefined}
        onAddPublisher={handleAddPublisher}
        onEditPublisher={handleEditAuthor}
        loading={serviceResponse.loading}
        onIsEdit={setIsEdit}
        isEdit={isEdit}
    />
  );
};

export default PublisherFormContainer;
