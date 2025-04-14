import React, { useCallback, useEffect, useState } from 'react';
import { BySubscription } from '../../types';
import { subscriptionsforFilteringService } from '../../services';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../state/redux';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import Loading from '../common/Loading';

const SubscriptionDropdown: React.FC<{
  onSelect: (subscriptionId: string) => void;
}> = ({ onSelect }) => {
  const [subscriptions, setSubscriptions] = useState<BySubscription[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const dispatch = useDispatch<AppDispatch>();
  const [selectedId, setSelectedId] = useState<string>('');

  const fetchSubscriptionList = useCallback(async () => {
    setLoading(true);
    try {
      const response = await subscriptionsforFilteringService();

      if (response.error)
        dispatch(
          addNotification({
            message: response.error,
            type: 'error',
          }),
        );

      if (response && response.data) {
        setSubscriptions(response.data);
      } else throw new Error('Invalid response structure');
    } catch (error) {
      dispatch(
        addNotification({
          message: error instanceof Error ? error.message : String(error),
          type: 'error',
        }),
      );
      setSubscriptions([]);
    }
    setLoading(false);
  }, [dispatch]);

  useEffect(() => {
    fetchSubscriptionList();
  }, [fetchSubscriptionList]);

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selected = e.target.value;
    setSelectedId(selected);
    onSelect(selected);
  };

  return (
    <div>
      <select
        id='subscription-select'
        value={selectedId}
        onChange={handleChange}
      >
        <option value=''>Filter by owned subscription </option>
        {(loading && <Loading />) ??
          subscriptions.map((sub) => (
            <option key={sub.id} value={sub.id}>
              {sub.title}
            </option>
          ))}
      </select>
    </div>
  );
};

export default SubscriptionDropdown;
