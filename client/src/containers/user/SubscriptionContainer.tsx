import { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import {
  Role,
  SubscribeRequest,
  Subscription as SubscriptionType,
  User,
} from '../../types';
import {
  fetchSubscriptionByIdService,
  subscribeService,
  unSubscribeService,
} from '../../services/subscriptionService';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import Subscription from '../../components/user/Subscription';
import { useNavigate } from 'react-router-dom';
import { roleEnumToNumber } from '@/api/adapters/userAdapter';

interface SubscriptionContainerProps {
  id: string;
}

const SubscriptionContainer: React.FC<SubscriptionContainerProps> = ({
  id,
}) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [subscription, setSubscription] = useState<SubscriptionType>(
    {} as SubscriptionType,
  );
  const [loading, setLoading] = useState<boolean>(true);
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

  const fetchSubscription = useCallback(async () => {
    (async () => {
      setLoading(true);
      try {
        const response = await fetchSubscriptionByIdService(id);
        if (response.data) setSubscription(response.data as SubscriptionType);
        else if (response.error) throw Error(response.error);
      } catch (error) {
        dispatch(
          addNotification({
            message: error instanceof Error ? error.message : String(error),
            type: 'error',
          }),
        );
        setSubscription({} as SubscriptionType);
      }
      setLoading(false);
      handleIsSubscribed();
    })();
  }, [id, dispatch, handleIsSubscribed]);

  useEffect(() => {
    fetchSubscription();
  }, [fetchSubscription]);

  const handleSubscribe = async (subscriptionId: string) => {
    const userId = await getUserId();
    if (!userId) return;

    const response = await subscribeService({
      userId: userId,
      subscriptionId: subscriptionId,
    } as SubscribeRequest);

    if (response.error) handleMessage(response.error, 'error');
    else {
      handleMessage(`You have successfully subscribed!`, 'success');
      navigate(-1); // Go back to the previous page
    }
  };

  const handleUnSubscribe = async (subscriptionId: string) => {
    const userId = await getUserId();
    if (!userId) return;

    const response = await unSubscribeService({
      userId: userId,
      subscriptionId: subscriptionId,
    } as SubscribeRequest);

    if (response.error) handleMessage(response.error, 'error');
    else {
      handleMessage(`You have successfully Unsubscribed!`, 'success');
      navigate(-1); // Go back to the previous page
    }
  };

  const getUserId = async (): Promise<string | undefined> => {
    const user = localStorage.getItem('user');

    if (!user) handleMessage('You must be logined to subscribe', 'error');
    else {
      const extractedUser = JSON.parse(user) as User;
      if (extractedUser.role !== roleEnumToNumber(Role.USER))
        handleMessage('You must be a user to subscribe', 'error');

      return extractedUser.id;
    }
  };

  return (
    <Subscription
      subscription={subscription}
      isSubscribed={isSubscribed}
      onSubscribe={handleSubscribe}
      onUnSubscribe={handleUnSubscribe}
      loading={loading}
    />
  );
};

export default SubscriptionContainer;
