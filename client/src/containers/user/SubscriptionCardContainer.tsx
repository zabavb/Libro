import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../state/redux';
import { useNavigate } from 'react-router-dom';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import { removeSubscriptionService } from '../../services/subscriptionService';
import { SubscriptionCard as SubscriptionCardType } from '../../types';
import SubscriptionCard from '../../components/user/SubscriptionCard';

interface SubscriptionCardContainerProps {
  subscription: SubscriptionCardType;
}

const SubscriptionCardContainer: React.FC<SubscriptionCardContainerProps> = ({
  subscription,
}) => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const handleNavigate = () => {
    navigate(`/admin/subscriptions/${subscription.id}`);
  };

  const handleDelete = async (e: React.MouseEvent) => {
    e.stopPropagation();
    const response = await removeSubscriptionService(subscription.id);

    dispatch(
      response.error
        ? addNotification({
            message: response.error,
            type: 'error',
          })
        : addNotification({
            message: 'Subscription successfully deleted.',
            type: 'success',
          }),
    );
  };

  return (
    <SubscriptionCard
      subscription={subscription}
      onNavigate={handleNavigate}
      onDelete={handleDelete}
    />
  );
};

export default SubscriptionCardContainer;
