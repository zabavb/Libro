import { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import {
  ServiceResponse,
  SubscribeRequest,
  Subscription as SubscriptionType,
  User,
} from '../../types';
import {
  fetchSubscriptionByIdService,
  subscribeService,
} from '../../services/subscriptionService';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import Subscription from '../../components/user/Subscription';
import { useNavigate } from 'react-router-dom';

interface SubscriptionContainerProps {
  id: string;
}

const SubscriptionContainer: React.FC<SubscriptionContainerProps> = ({
  id,
}) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [serviceResponse, setServiceResponse] = useState<
    ServiceResponse<SubscriptionType>
  >({
    data: null,
    loading: !!id,
    error: null,
  });
  const [isSubscribed, setIsSubscribed] = useState<boolean>(false);

  const handleIsSubscribed = useCallback(() => {
    const tmpSubscriptions = localStorage.getItem('subscriptions');
    if (!tmpSubscriptions) setIsSubscribed(false);
    else {
      const subscriptions = JSON.parse(tmpSubscriptions) as string[];
      if (subscriptions.includes(id)) setIsSubscribed(true);
    }
  }, [id]);

  const handleMessage = useCallback(
    (message: string, type: 'success' | 'error') => {
      dispatch(addNotification({ message, type }));
    },
    [dispatch],
  );

  useEffect(() => {
    (async () => {
      const response = await fetchSubscriptionByIdService(id);
      setServiceResponse(response);

      if (response.error) handleMessage(response.error, 'error');
      handleIsSubscribed();
    })();
  }, [id, dispatch, handleIsSubscribed, handleMessage]);

  const handleSubscribe = async (subscriptionId: string) => {
    const user = localStorage.getItem('user');
    if (!user)
      return handleMessage('You must be logined to subscribe', 'error');

    const extractedUser = JSON.parse(user) as User;

    const response = await subscribeService({
      userId: extractedUser.id,
      subscriptionId: subscriptionId,
    } as SubscribeRequest);

    if (response.error) handleMessage(response.error, 'error');
    else {
      handleMessage(`You have successfully subscribed!`, 'success');
      navigate(-1); // Go back to the previous page
    }
  };

  return (
    <Subscription
      subscription={serviceResponse.data!}
      isSubscribed={isSubscribed}
      onSubscribe={handleSubscribe}
      loading={serviceResponse.loading}
    />
  );
};

export default SubscriptionContainer;
