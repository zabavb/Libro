import { useNavigate, useParams } from 'react-router-dom';
import SubscriptionFormContainer from '../../../../containers/user/SubscriptionFormContainer';

const SubscriptionFormPage: React.FC = () => {
  const { subscriptionId } = useParams<{ subscriptionId: string }>();
  const navigate = useNavigate();

  const handleGoBack = () => {
    navigate('/admin/subscriptions');
  };

  return (
    <div>
      <header>
        <h1>{subscriptionId ? 'Edit Subscription' : 'Add Subscription'}</h1>
        <button onClick={handleGoBack}>Back to Subscription List</button>
      </header>
      <main>
        <SubscriptionFormContainer id={subscriptionId ?? ''} />
      </main>
    </div>
  );
};

export default SubscriptionFormPage;
