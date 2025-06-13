import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { ServiceResponse, Subscription } from '../../types';
import { useCallback, useEffect, useState } from 'react';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import {
  addSubscriptionService,
  editSubscriptionService,
  fetchSubscriptionByIdService,
} from '../../services/subscriptionService';
import SubscriptionForm from '../../components/user/SubscriptionForm';

interface SubscriptionFormContainerProps {
  id?: string;
}

const SubscriptionFormContainer: React.FC<SubscriptionFormContainerProps> = ({
  id,
}) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [serviceResponse, setServiceResponse] = useState<
    ServiceResponse<Subscription>
  >({
    data: null,
    loading: !!id,
    error: null,
  });

  useEffect(() => {
    if (!id) return;

    (async () => {
      const response = await fetchSubscriptionByIdService(id);
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

  const handleAddSubscription = useCallback(
    async (formData: FormData) => {
      const response = await addSubscriptionService(formData);

      if (response.error) handleMessage(response.error, 'error');
      else {
        handleMessage('Subscription created successfully!', 'success');
        handleNavigate('/admin/subscriptions');
      }
    },
    [handleMessage, handleNavigate],
  );

  const handleEditSubscription = useCallback(
    async (formData: FormData) => {
      if (!id) return;
      const response = await editSubscriptionService(id, formData);

      if (response.error) handleMessage(response.error, 'error');
      else handleMessage('Subscription updated successfully!', 'success');
    },
    [id, handleMessage],
  );

  return (
    <SubscriptionForm
      existingSubscription={serviceResponse.data ?? undefined}
      onAddSubscription={handleAddSubscription}
      onEditSubscription={handleEditSubscription}
      loading={serviceResponse.loading}
    />
  );
};

export default SubscriptionFormContainer;
