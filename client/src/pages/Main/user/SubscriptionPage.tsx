import SubscriptionContainer from '@/containers/user/SubscriptionContainer';
import { useParams } from 'react-router-dom';

const SubscriptionPage: React.FC = () => {
  const { subscriptionId } = useParams<{ subscriptionId: string }>();

  return <SubscriptionContainer id={subscriptionId ?? ''} />;
};

export default SubscriptionPage;
