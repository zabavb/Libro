import Profile from '@/components/user/Profile';
import { editUserService, fetchSubscriptionByIdService } from '@/services';
import { editPasswordService } from '@/services/passwordService';
import { useAuth } from '@/state/context';
import { addNotification } from '@/state/redux/slices/notificationSlice';
import { ComplicatedLoading, Subscription, User } from '@/types';
import { UserProfileFormData } from '@/utils';
import { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

const ProfileContainer: React.FC = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { token } = useAuth();

  const [user, setUser] = useState<User>({} as User);

  const [subscription, setSubscription] = useState<Subscription | null>(null);
  const [isSubscribedFor365, setIsSubscribedFor365] = useState(false);

  const [loading, setLoading] = useState<
    ComplicatedLoading<UserProfileFormData>
  >({
    isLoading: false,
    fieldName: 'all',
  });

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

  const fetchUser = useCallback(async (): Promise<void> => {
    const data = JSON.parse(localStorage.getItem('user') || '{}') as User;
    if (token && data?.id) setUser(data);
    else {
      handleMessage('You are not authorized!', 'error');
      handleNavigate('/login');
    }
  }, [token, handleMessage, handleNavigate]);

  const fetchSubscription = useCallback(async (): Promise<void> => {
    const data = JSON.parse(
      localStorage.getItem('subscriptions') || '{}',
    ) as string[];
    const subscription365Id = import.meta.env.VITE_SUBSCRIPTION_365_ID;

    if (token && data.length > 0 && data.includes(subscription365Id))
      setIsSubscribedFor365(true);

    const response = await fetchSubscriptionByIdService(subscription365Id);

    if (response.error) handleMessage(response.error, 'error');
    else setSubscription(response.data as Subscription | null);
  }, [token, handleMessage]);

  useEffect(() => {
    (async () => {
      setLoading({ isLoading: true, fieldName: 'all' });
      await fetchUser();
      await fetchSubscription();
      setLoading({ isLoading: false, fieldName: 'all' });
    })();
  }, [fetchUser, fetchSubscription]);

  const handleEditUser = useCallback(
    async <K extends keyof UserProfileFormData>(
      field: K,
      value: UserProfileFormData[K],
    ) => {
      if (!user?.id) return;
      setLoading({ isLoading: true, fieldName: field });

      const data = { ...user, [field]: value };
      const response = await editUserService(user.id, data);

      if (response.error) handleMessage(response.error, 'error');
      else {
        handleMessage(`${field} successfully changed!`, 'success');
        localStorage.setItem('user', JSON.stringify(data));
        setUser(user);
      }
      setLoading({ isLoading: false, fieldName: field });
    },
    [user, handleMessage],
  );

  const handleUpdatePassword = useCallback(
    async (id: string, password: string) => {
      if (!user?.id) return;
      setLoading({ isLoading: true, fieldName: 'password' });

      const response = await editPasswordService(id, password);

      if (response.error) handleMessage(response.error, 'error');
      else handleMessage('Password updated successfully!', 'success');
      setLoading({ isLoading: false, fieldName: 'password' });
    },
    [user, handleMessage],
  );

  return (
    <Profile
      user={user}
      onEditUser={handleEditUser}
      onUpdatePassword={handleUpdatePassword}
      subscription={subscription ?? undefined}
      isSubscribedFor365={isSubscribedFor365}
      loading={loading}
      onNavigate={handleNavigate}
    />
  );
};

export default ProfileContainer;
