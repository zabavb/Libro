import { useParams } from 'react-router-dom';
import SubscriptionContainer from '../../containers/user/SubscriptionContainer';

const SubscriptionPage: React.FC = () => {
  const { subscriptionId } = useParams<{ subscriptionId: string }>();

  return <SubscriptionContainer id={subscriptionId ?? ''} />;
};

export default SubscriptionPage;
