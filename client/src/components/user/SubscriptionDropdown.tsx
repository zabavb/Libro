import React, { useCallback, useEffect, useState } from 'react';
import { BySubscription } from '../../types';
import { subscriptionsforFilteringService } from '../../services';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../state/redux';
import { addNotification } from '../../state/redux/slices/notificationSlice';

const SubscriptionDropdown: React.FC<{
  onSelect: (subscriptionId: string | null) => void;
}> = ({ onSelect }) => {
  const [subscriptions, setSubscriptions] = useState<BySubscription[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const dispatch = useDispatch<AppDispatch>();
  const [selectedId, setSelectedId] = useState<string | null>('');

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
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selected = e.target.value === '' ? null : e.target.value;
    setSelectedId(selected);
    onSelect(selected);
  };

  return (
    <div className='flex'>
      <select 
        className='filter-item'
        id='subscription-select'
        value={selectedId ?? ''}
        onChange={handleChange}
      >
        {loading ? (
          <option disabled value=''>
            Loading...
          </option>
        ) : (
          <>
            <option value=''>Filter by owned subscription</option>
            {subscriptions && subscriptions.length > 0 ? (
              subscriptions.map((sub) => (
                <option key={sub.id} value={sub.id}>
                  {sub.title}
                </option>
              ))
            ) : (
              <option disabled>No subscriptions available</option>
            )}
          </>
        )}
      </select>
    </div>
  );
};

export default SubscriptionDropdown;
